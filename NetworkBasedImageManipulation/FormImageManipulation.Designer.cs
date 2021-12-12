namespace NetworkBasedImageManipulation
{
    partial class FormImageManipulation
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxImageContainer = new System.Windows.Forms.PictureBox();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.textBoxImagePath = new System.Windows.Forms.TextBox();
            this.openImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxMakeGray = new System.Windows.Forms.CheckBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.trackBarBrightness = new System.Windows.Forms.TrackBar();
            this.labelBrightness = new System.Windows.Forms.Label();
            this.labelContrast = new System.Windows.Forms.Label();
            this.trackBarContrast = new System.Windows.Forms.TrackBar();
            this.comboBoxGradationCorrection = new System.Windows.Forms.ComboBox();
            this.labelGradationCorrection = new System.Windows.Forms.Label();
            this.labelGammaCorrection = new System.Windows.Forms.Label();
            this.trackBarGammaCorrection = new System.Windows.Forms.TrackBar();
            this.labelScale = new System.Windows.Forms.Label();
            this.trackBarScale = new System.Windows.Forms.TrackBar();
            this.comboBoxScalingMethod = new System.Windows.Forms.ComboBox();
            this.labelMethod = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImageContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGammaCorrection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxImageContainer
            // 
            this.pictureBoxImageContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxImageContainer.Location = new System.Drawing.Point(24, 22);
            this.pictureBoxImageContainer.Name = "pictureBoxImageContainer";
            this.pictureBoxImageContainer.Size = new System.Drawing.Size(586, 546);
            this.pictureBoxImageContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxImageContainer.TabIndex = 0;
            this.pictureBoxImageContainer.TabStop = false;
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.Location = new System.Drawing.Point(616, 22);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(228, 34);
            this.buttonLoadImage.TabIndex = 1;
            this.buttonLoadImage.Text = "Load image";
            this.buttonLoadImage.UseVisualStyleBackColor = true;
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // textBoxImagePath
            // 
            this.textBoxImagePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxImagePath.Location = new System.Drawing.Point(616, 73);
            this.textBoxImagePath.Name = "textBoxImagePath";
            this.textBoxImagePath.Size = new System.Drawing.Size(227, 20);
            this.textBoxImagePath.TabIndex = 2;
            // 
            // openImageFileDialog
            // 
            this.openImageFileDialog.FileName = "openImageFileDialog";
            // 
            // checkBoxMakeGray
            // 
            this.checkBoxMakeGray.AutoSize = true;
            this.checkBoxMakeGray.Location = new System.Drawing.Point(619, 512);
            this.checkBoxMakeGray.Name = "checkBoxMakeGray";
            this.checkBoxMakeGray.Size = new System.Drawing.Size(76, 17);
            this.checkBoxMakeGray.TabIndex = 3;
            this.checkBoxMakeGray.Text = "Make grey";
            this.checkBoxMakeGray.UseVisualStyleBackColor = true;
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(616, 535);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(230, 23);
            this.buttonApply.TabIndex = 4;
            this.buttonApply.Text = "Apply settings";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // trackBarBrightness
            // 
            this.trackBarBrightness.LargeChange = 10;
            this.trackBarBrightness.Location = new System.Drawing.Point(615, 120);
            this.trackBarBrightness.Maximum = 50;
            this.trackBarBrightness.Minimum = -50;
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Size = new System.Drawing.Size(225, 45);
            this.trackBarBrightness.TabIndex = 5;
            this.trackBarBrightness.Scroll += new System.EventHandler(this.trackBarBrightness_Scroll);
            // 
            // labelBrightness
            // 
            this.labelBrightness.AutoSize = true;
            this.labelBrightness.Location = new System.Drawing.Point(699, 104);
            this.labelBrightness.Name = "labelBrightness";
            this.labelBrightness.Size = new System.Drawing.Size(56, 13);
            this.labelBrightness.TabIndex = 6;
            this.labelBrightness.Text = "Brightness";
            // 
            // labelContrast
            // 
            this.labelContrast.AutoSize = true;
            this.labelContrast.Location = new System.Drawing.Point(699, 168);
            this.labelContrast.Name = "labelContrast";
            this.labelContrast.Size = new System.Drawing.Size(46, 13);
            this.labelContrast.TabIndex = 8;
            this.labelContrast.Text = "Contrast";
            // 
            // trackBarContrast
            // 
            this.trackBarContrast.LargeChange = 10;
            this.trackBarContrast.Location = new System.Drawing.Point(618, 189);
            this.trackBarContrast.Maximum = 50;
            this.trackBarContrast.Minimum = -50;
            this.trackBarContrast.Name = "trackBarContrast";
            this.trackBarContrast.Size = new System.Drawing.Size(225, 45);
            this.trackBarContrast.TabIndex = 7;
            this.trackBarContrast.Scroll += new System.EventHandler(this.trackBarContrast_Scroll);
            // 
            // comboBoxGradationCorrection
            // 
            this.comboBoxGradationCorrection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGradationCorrection.FormattingEnabled = true;
            this.comboBoxGradationCorrection.Items.AddRange(new object[] {
            "Linear",
            "Sinusoid",
            "Exponential",
            "Logarithmic"});
            this.comboBoxGradationCorrection.Location = new System.Drawing.Point(621, 320);
            this.comboBoxGradationCorrection.Name = "comboBoxGradationCorrection";
            this.comboBoxGradationCorrection.Size = new System.Drawing.Size(224, 21);
            this.comboBoxGradationCorrection.TabIndex = 9;
            // 
            // labelGradationCorrection
            // 
            this.labelGradationCorrection.AutoSize = true;
            this.labelGradationCorrection.Location = new System.Drawing.Point(678, 301);
            this.labelGradationCorrection.Name = "labelGradationCorrection";
            this.labelGradationCorrection.Size = new System.Drawing.Size(103, 13);
            this.labelGradationCorrection.TabIndex = 10;
            this.labelGradationCorrection.Text = "Gradation correction";
            // 
            // labelGammaCorrection
            // 
            this.labelGammaCorrection.AutoSize = true;
            this.labelGammaCorrection.Location = new System.Drawing.Point(678, 237);
            this.labelGammaCorrection.Name = "labelGammaCorrection";
            this.labelGammaCorrection.Size = new System.Drawing.Size(93, 13);
            this.labelGammaCorrection.TabIndex = 12;
            this.labelGammaCorrection.Text = "Gamma correction";
            // 
            // trackBarGammaCorrection
            // 
            this.trackBarGammaCorrection.LargeChange = 10;
            this.trackBarGammaCorrection.Location = new System.Drawing.Point(618, 253);
            this.trackBarGammaCorrection.Maximum = 190;
            this.trackBarGammaCorrection.Minimum = 10;
            this.trackBarGammaCorrection.Name = "trackBarGammaCorrection";
            this.trackBarGammaCorrection.Size = new System.Drawing.Size(225, 45);
            this.trackBarGammaCorrection.TabIndex = 11;
            this.trackBarGammaCorrection.Value = 100;
            this.trackBarGammaCorrection.Scroll += new System.EventHandler(this.trackBarGammaCorrection_Scroll);
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(712, 347);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(34, 13);
            this.labelScale.TabIndex = 14;
            this.labelScale.Text = "Scale";
            // 
            // trackBarScale
            // 
            this.trackBarScale.LargeChange = 10;
            this.trackBarScale.Location = new System.Drawing.Point(616, 363);
            this.trackBarScale.Maximum = 190;
            this.trackBarScale.Minimum = 10;
            this.trackBarScale.Name = "trackBarScale";
            this.trackBarScale.Size = new System.Drawing.Size(225, 45);
            this.trackBarScale.TabIndex = 13;
            this.trackBarScale.Value = 100;
            this.trackBarScale.Scroll += new System.EventHandler(this.trackBarScale_Scroll);
            // 
            // comboBoxScalingMethod
            // 
            this.comboBoxScalingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxScalingMethod.FormattingEnabled = true;
            this.comboBoxScalingMethod.Items.AddRange(new object[] {
            "Closest neighbour method",
            "Bilinear interpolation method"});
            this.comboBoxScalingMethod.Location = new System.Drawing.Point(625, 456);
            this.comboBoxScalingMethod.Name = "comboBoxScalingMethod";
            this.comboBoxScalingMethod.Size = new System.Drawing.Size(220, 21);
            this.comboBoxScalingMethod.TabIndex = 15;
            // 
            // labelMethod
            // 
            this.labelMethod.AutoSize = true;
            this.labelMethod.Location = new System.Drawing.Point(622, 426);
            this.labelMethod.Name = "labelMethod";
            this.labelMethod.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelMethod.Size = new System.Drawing.Size(43, 13);
            this.labelMethod.TabIndex = 16;
            this.labelMethod.Text = "Method";
            // 
            // FormImageManipulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 570);
            this.Controls.Add(this.labelMethod);
            this.Controls.Add(this.comboBoxScalingMethod);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.trackBarScale);
            this.Controls.Add(this.labelGammaCorrection);
            this.Controls.Add(this.trackBarGammaCorrection);
            this.Controls.Add(this.labelGradationCorrection);
            this.Controls.Add(this.comboBoxGradationCorrection);
            this.Controls.Add(this.labelContrast);
            this.Controls.Add(this.trackBarContrast);
            this.Controls.Add(this.labelBrightness);
            this.Controls.Add(this.trackBarBrightness);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.checkBoxMakeGray);
            this.Controls.Add(this.textBoxImagePath);
            this.Controls.Add(this.buttonLoadImage);
            this.Controls.Add(this.pictureBoxImageContainer);
            this.Name = "FormImageManipulation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image manipulation";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImageContainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGammaCorrection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxImageContainer;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.TextBox textBoxImagePath;
        private System.Windows.Forms.OpenFileDialog openImageFileDialog;
        private System.Windows.Forms.CheckBox checkBoxMakeGray;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.TrackBar trackBarBrightness;
        private System.Windows.Forms.Label labelBrightness;
        private System.Windows.Forms.Label labelContrast;
        private System.Windows.Forms.TrackBar trackBarContrast;
        private System.Windows.Forms.ComboBox comboBoxGradationCorrection;
        private System.Windows.Forms.Label labelGradationCorrection;
        private System.Windows.Forms.Label labelGammaCorrection;
        private System.Windows.Forms.TrackBar trackBarGammaCorrection;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.TrackBar trackBarScale;
        private System.Windows.Forms.ComboBox comboBoxScalingMethod;
        private System.Windows.Forms.Label labelMethod;
    }
}

