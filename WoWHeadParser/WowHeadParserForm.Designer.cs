namespace WoWHeadParser
{
    partial class WoWHeadParserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.parserBox = new System.Windows.Forms.ComboBox();
            this.localeBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rangeStart = new System.Windows.Forms.NumericUpDown();
            this.rangeEnd = new System.Windows.Forms.NumericUpDown();
            this.rangeEndLabel = new System.Windows.Forms.Label();
            this.rangeStartLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.startButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeEnd)).BeginInit();
            this.SuspendLayout();
            // 
            // parserBox
            // 
            this.parserBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parserBox.FormattingEnabled = true;
            this.parserBox.Location = new System.Drawing.Point(59, 30);
            this.parserBox.Name = "parserBox";
            this.parserBox.Size = new System.Drawing.Size(227, 21);
            this.parserBox.TabIndex = 1;
            // 
            // localeBox
            // 
            this.localeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.localeBox.FormattingEnabled = true;
            this.localeBox.Items.AddRange(new object[] {
            "",
            "ru.",
            "de.",
            "fr.",
            "es."});
            this.localeBox.Location = new System.Drawing.Point(6, 30);
            this.localeBox.Name = "localeBox";
            this.localeBox.Size = new System.Drawing.Size(47, 21);
            this.localeBox.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rangeStart);
            this.groupBox1.Controls.Add(this.rangeEnd);
            this.groupBox1.Controls.Add(this.rangeEndLabel);
            this.groupBox1.Controls.Add(this.rangeStartLabel);
            this.groupBox1.Controls.Add(this.localeBox);
            this.groupBox1.Controls.Add(this.parserBox);
            this.groupBox1.Location = new System.Drawing.Point(6, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 104);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // rangeStart
            // 
            this.rangeStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.rangeStart.Location = new System.Drawing.Point(87, 78);
            this.rangeStart.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.rangeStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rangeStart.Name = "rangeStart";
            this.rangeStart.Size = new System.Drawing.Size(53, 20);
            this.rangeStart.TabIndex = 11;
            this.rangeStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rangeEnd
            // 
            this.rangeEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.rangeEnd.Location = new System.Drawing.Point(232, 78);
            this.rangeEnd.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.rangeEnd.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rangeEnd.Name = "rangeEnd";
            this.rangeEnd.Size = new System.Drawing.Size(54, 20);
            this.rangeEnd.TabIndex = 14;
            this.rangeEnd.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rangeEndLabel
            // 
            this.rangeEndLabel.AutoSize = true;
            this.rangeEndLabel.Location = new System.Drawing.Point(172, 85);
            this.rangeEndLabel.Name = "rangeEndLabel";
            this.rangeEndLabel.Size = new System.Drawing.Size(63, 13);
            this.rangeEndLabel.TabIndex = 13;
            this.rangeEndLabel.Text = "Range end:";
            // 
            // rangeStartLabel
            // 
            this.rangeStartLabel.AutoSize = true;
            this.rangeStartLabel.Location = new System.Drawing.Point(6, 85);
            this.rangeStartLabel.Name = "rangeStartLabel";
            this.rangeStartLabel.Size = new System.Drawing.Size(65, 13);
            this.rangeStartLabel.TabIndex = 12;
            this.rangeStartLabel.Text = "Range start:";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 187);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(300, 23);
            this.progressBar.TabIndex = 5;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 137);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // WoWHeadParserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(313, 222);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox1);
            this.Name = "WoWHeadParserForm";
            this.Text = "WowHead Parser";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeEnd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox parserBox;
        private System.Windows.Forms.ComboBox localeBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label rangeStartLabel;
        private System.Windows.Forms.NumericUpDown rangeEnd;
        private System.Windows.Forms.Label rangeEndLabel;
        private System.Windows.Forms.NumericUpDown rangeStart;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button startButton;


    }
}

