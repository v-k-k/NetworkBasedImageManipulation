using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkBasedImageManipulation
{
    public partial class Form1 : Form
    {
        private const string defaultSample = "sample.jpg";

        Bitmap bmp;
        string brightnessLabel;
        string contrastLabel;
        string gammaLabel;
        private readonly double CorrectionKoef = 8 * Math.Log(2) / 255d;

        public Form1()
        {
            InitializeComponent();
            LoadPicture(defaultSample);
            brightnessLabel = labelBrightness.Text;
            contrastLabel = labelContrast.Text;
            UpdateBrightnessLabel();
            UpdateContrastLabel();
            UpdateGammaLabel();
            comboBoxGradationCorrection.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            openImageFileDialog.InitialDirectory = Environment.CurrentDirectory;
            DialogResult result = openImageFileDialog.ShowDialog();
            LoadPicture(openImageFileDialog.FileName);
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

        private void ChangePicture()
        {
            Bitmap greyed = new Bitmap(bmp);
            bool isGrey = checkBoxMakeGray.Checked;
            int brightness = trackBarBrightness.Value;
            int contrast = trackBarContrast.Value;
            int correctionIndex = comboBoxGradationCorrection.SelectedIndex;
            int gamma = trackBarGammaCorrection.Value;

            for (int y = 0; y < greyed.Height; y++)
                for (int x = 0; x < greyed.Width; x++)
                {
                    Color pixel = bmp.GetPixel(x, y);

                    if (isGrey)
                        pixel = GreyoutPixel(pixel);
                    if (brightness != 0)
                        pixel = ChangeBrightness(pixel, brightness);
                    if (contrast != 0)
                        pixel = ChangeContrast(pixel, contrast);
                    switch (correctionIndex)
                    {
                        case 0: break;
                        case 1:
                            pixel = ChangeCorrection(pixel, CorrectionBySin);
                            break;
                        case 2:
                            pixel = ChangeCorrection(pixel, CorrectionByExp);
                            break;
                        case 3:
                            pixel = ChangeCorrection(pixel, CorrectionByLog);
                            break;
                    }
                    if (gamma != 100)
                        pixel = ChangeGammaCorrection(pixel, gamma);

                    greyed.SetPixel(x, y, pixel);
                }
            pictureBoxImageContainer.Image = greyed;
        }

        private Color ChangeGammaCorrection(Color pixel, int gamma)
        {
            double calculatedGamma = gamma / 100d;
            return Color.FromArgb(
                ChangeColorGamma(pixel.R, calculatedGamma),
                ChangeColorGamma(pixel.G, calculatedGamma),
                ChangeColorGamma(pixel.B, calculatedGamma)
            );
        }

        private int ChangeColorGamma(int color, double calculatedGamma)
        {
            double result = Math.Pow(color / 255d, calculatedGamma) * 255;
            if (result > 255) return 255;
            return Convert.ToInt16(result);
        }

        delegate int CorrectionFunction(int color);

        int CorrectionBySin(int color)
        {
            double result = (255 / 2d) * Math.Sin(Math.PI / 255 * color - Math.PI / 2) + (255 / 2d);
            return Convert.ToInt16(result);
        }

        int CorrectionByExp(int color)
        {
            double result = Math.Exp(CorrectionKoef * color) - 1;
            return Convert.ToInt16(result);
        }

        int CorrectionByLog(int color)
        {
            double result = Math.Log(color + 1) / CorrectionKoef;
            return Convert.ToInt16(result);
        }

        private Color ChangeCorrection(Color pixel, CorrectionFunction fn)
        {
            return Color.FromArgb(fn(pixel.R), fn(pixel.G), fn(pixel.B));
        }

        private Color ChangeContrast(Color pixel, int value)
        {
            float percent = value / 100f;
            return Color.FromArgb(
                ChangeColorContrast(pixel.R, percent),
                ChangeColorContrast(pixel.G, percent),
                ChangeColorContrast(pixel.B, percent)
            );
        }

        private int ChangeColorContrast(int color, float percent)
        {
            int result;
            if (percent < 0)
                result = Convert.ToInt16(color + percent * (color - 128));
            else
                result = Convert.ToInt16(128 + (color - 128) / (1 - percent));

            if (result > 255) return 255;
            if (result < 0) return 0;
            return result;
        }

        private Color ChangeBrightness(Color pixel, int value)
        {
            float percent = value / 100f;
            return Color.FromArgb(
                ChangeColorBrightness(pixel.R, percent),
                ChangeColorBrightness(pixel.G, percent),
                ChangeColorBrightness(pixel.B, percent)
            );
        }

        private int ChangeColorBrightness(int color, float percent)
        {
            int result = Convert.ToInt16(color + percent * 128);
            if (result > 255) return 255;
            if (result < 0) return 0;
            return result;
        }

        private Color GreyoutPixel(Color pixel)
        {
            int avg = (pixel.R + pixel.G + pixel.B + 1) / 3;
            return Color.FromArgb(avg, avg, avg);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            ChangePicture();
        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            UpdateBrightnessLabel();
        }

        private void UpdateBrightnessLabel()
        {
            labelBrightness.Text = $"{brightnessLabel} {trackBarBrightness.Value}%";
        }

        private void trackBarContrast_Scroll(object sender, EventArgs e)
        {
            UpdateContrastLabel();
        }

        private void UpdateContrastLabel()
        {
            labelContrast.Text = $"{contrastLabel} {trackBarContrast.Value}%";
        }

        private void UpdateGammaLabel()
        {
            labelGammaCorrection.Text = $"{gammaLabel} {trackBarGammaCorrection.Value}%";
        }

        private void trackBarGammaCorrection_Scroll(object sender, EventArgs e)
        {
            UpdateGammaLabel();
        }
    }
}
