using NetworkBasedImageManipulation.Workers;
using NetworkBasedImageManipulation.Listeners;
using System.Windows.Forms;
using System.Drawing;
using System;

namespace NetworkBasedImageManipulation
{
    public partial class FormImageManipulation : Form
    {
        private const string defaultSample = "sample.jpg";
        public string IP => formTcpListener.IpSetup == null ? "127.0.0.1" : formTcpListener.IpSetup;
        public int PORT => formTcpListener.PortSetup == 0 ? 1234 : formTcpListener.PortSetup;

        Bitmap bmp;
        string brightnessLabel;
        string contrastLabel;
        string gammaLabel;
        string scaleLabel;
        VideoWorker streamPlayer;
        UdpListener formUdpListener;
        TcpListener formTcpListener;

        public FormImageManipulation()
        {
            InitializeComponent();
            LoadPicture(defaultSample);

            brightnessLabel = labelBrightness.Text;
            gammaLabel = labelGammaCorrection.Text;
            contrastLabel = labelContrast.Text;
            scaleLabel = labelScale.Text;

            comboBoxGradationCorrection.SelectedIndex = 0;
            comboBoxScalingMethod.SelectedIndex = 0;

            UpdateLabels();

            richTextBoxNetworkLog.Text += $"Waiting for the TCP and UDP clients to connect...{Environment.NewLine}";

            formUdpListener = new UdpListener(networkLog: richTextBoxNetworkLog);
            formTcpListener = new TcpListener(networkLog: richTextBoxNetworkLog);
        }

        private void LoadPicture(string fileName)
        {
            try
            {
                bmp = new Bitmap(Image.FromFile(fileName));
                textBoxImagePath.Text = fileName;
                pictureBoxImageContainer.Image = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                textBoxImagePath.Text = "";
            }
        }

        private void ChangePicture(bool bilinear = false)
        {
            float scaleKoef = trackBarScale.Value / 100f;
            int oldWidth = bmp.Width;
            int oldHeight = bmp.Height;
            int newWidth = Convert.ToInt16(oldWidth * scaleKoef);
            int newHeight = Convert.ToInt16(oldHeight * scaleKoef);
            float calculatedHeight = (float)(oldHeight - 1) / (float)(newHeight - 1);
            float calculatedWidth = (float)(oldWidth - 1) / (float)(newWidth - 1);

            Bitmap greyed = new Bitmap(newWidth, newHeight);

            if (bilinear)
            {
                float x, y;
                int x1, y1, x2, y2;
                bool isGrey = checkBoxMakeGray.Checked;
                int brightness = trackBarBrightness.Value;
                int contrast = trackBarContrast.Value;
                int correctionIndex = comboBoxGradationCorrection.SelectedIndex;
                int gamma = trackBarGammaCorrection.Value;

                for (int newY = 0; newY < newHeight; newY++)
                {
                    for (int newX = 0; newX < newWidth; newX++)
                    {
                        x = newX * calculatedWidth;
                        y = newY * calculatedHeight;

                        x1 = Convert.ToInt16(Math.Floor(x));
                        if (x1 > oldWidth - 2) x1 = oldWidth - 2;
                        y1 = Convert.ToInt16(Math.Floor(y));
                        if (y1 > oldHeight - 2) y1 = oldHeight - 2;

                        x2 = x1 + 1;
                        y2 = y1 + 1;

                        Color Q11 = bmp.GetPixel(x1, y1);
                        Color Q12 = bmp.GetPixel(x1, y2);
                        Color Q21 = bmp.GetPixel(x2, y1);
                        Color Q22 = bmp.GetPixel(x2, y2);

                        int R = ImageWorker.BilinearValue(x, y, x1, y1, x2, y2, Q11.R, Q12.R, Q21.R, Q22.R);
                        int G = ImageWorker.BilinearValue(x, y, x1, y1, x2, y2, Q11.G, Q12.G, Q21.G, Q22.G);
                        int B = ImageWorker.BilinearValue(x, y, x1, y1, x2, y2, Q11.B, Q12.B, Q21.B, Q22.B);

                        Color pixel = Color.FromArgb(R, G, B);
                        pixel = ImageWorker.PreparePixel(isGrey, brightness, contrast, correctionIndex, gamma, pixel);
                        greyed.SetPixel(newX, newY, pixel);
                    }
                }
            }
            else
            {
                int x0, y0;
                bool isGrey = checkBoxMakeGray.Checked;
                int brightness = trackBarBrightness.Value;
                int contrast = trackBarContrast.Value;
                int correctionIndex = comboBoxGradationCorrection.SelectedIndex;
                int gamma = trackBarGammaCorrection.Value;

                for (int y = 0; y < newHeight; y++)
                {
                    y0 = Convert.ToInt16(y * calculatedHeight);
                    for (int x = 0; x < newWidth; x++)
                    {
                        x0 = Convert.ToInt16(x * calculatedWidth);
                        Color pixel = bmp.GetPixel(x0, y0);
                        pixel = ImageWorker.PreparePixel(isGrey, brightness, contrast, correctionIndex, gamma, pixel);
                        greyed.SetPixel(x, y, pixel);
                    }
                }
            }                
            pictureBoxImageContainer.Image = greyed;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            switch (comboBoxScalingMethod.SelectedIndex)
            {
                case 1:
                    ChangePicture(bilinear: true);
                    break;
                default:
                    ChangePicture();
                    break;
            }
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            openImageFileDialog.InitialDirectory = Environment.CurrentDirectory;
            DialogResult result = openImageFileDialog.ShowDialog();
            LoadPicture(openImageFileDialog.FileName);
        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            UpdateBrightnessLabel();
        }

        private void trackBarContrast_Scroll(object sender, EventArgs e)
        {
            UpdateContrastLabel();
        }

        private void trackBarGammaCorrection_Scroll(object sender, EventArgs e)
        {
            UpdateGammaLabel();
        }

        private void trackBarScale_Scroll(object sender, EventArgs e)
        {
            UpdateScaleLabel();
        }

        private void UpdateContrastLabel()
        {
            labelContrast.Text = $"{contrastLabel} {trackBarContrast.Value}%";
        }

        private void UpdateGammaLabel()
        {
            labelGammaCorrection.Text = $"{gammaLabel} {trackBarGammaCorrection.Value}%";
        }

        private void UpdateBrightnessLabel()
        {
            labelBrightness.Text = $"{brightnessLabel} {trackBarBrightness.Value}%";
        }

        private void UpdateScaleLabel()
        {
            labelScale.Text = $"{scaleLabel} {trackBarScale.Value}%";
        }

        private void UpdateLabels()
        {
            UpdateBrightnessLabel();
            UpdateContrastLabel();
            UpdateGammaLabel();
            UpdateScaleLabel();
        }

        private void buttonApplyUdpPayload_Click(object sender, EventArgs e)
        {
            streamPlayer = formUdpListener.GetVideoStream();
            streamPlayer.showStream();
        }

        private void FormImageManipulation_FormClosed(object sender, FormClosedEventArgs e)
        {
            // DISPOSE the video and listeners
        }

        private void buttonMakeScreenshot_Click(object sender, EventArgs e)
        {
            formUdpListener.CloseListener();
            streamPlayer.closeStream();
            bmp = streamPlayer.GetFrame();
            pictureBoxImageContainer.Image = bmp;
        }

        private void buttonCloseStream_Click(object sender, EventArgs e)
        {
            streamPlayer.closeStream();
        }

        private void buttonApplyStreamEndpt_Click(object sender, EventArgs e)
        {
            formUdpListener.ReceiveBroadcast(ipToListen: IP, portToListen: PORT, newStream: true);
        }

        private void buttonApplyCommandEndpt_Click(object sender, EventArgs e)
        {
            formTcpListener.StartListeningForIncomingConnection();
        }
    }
}
