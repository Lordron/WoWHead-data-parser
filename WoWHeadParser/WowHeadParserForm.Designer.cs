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
            this.threadCount = new System.Windows.Forms.NumericUpDown();
            this.delay = new System.Windows.Forms.NumericUpDown();
            this.delayLabel = new System.Windows.Forms.Label();
            this.threadLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.startButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delay)).BeginInit();
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
            this.localeBox.Location = new System.Drawing.Point(6, 30);
            this.localeBox.Name = "localeBox";
            this.localeBox.Items.Add(new object[] {
            "",
            "ru.",
            "de.",
            "fr.",
            "es."});
            this.localeBox.Size = new System.Drawing.Size(47, 21);
            this.localeBox.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.threadCount);
            this.groupBox1.Controls.Add(this.delay);
            this.groupBox1.Controls.Add(this.delayLabel);
            this.groupBox1.Controls.Add(this.threadLabel);
            this.groupBox1.Controls.Add(this.localeBox);
            this.groupBox1.Controls.Add(this.parserBox);
            this.groupBox1.Location = new System.Drawing.Point(6, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 104);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // threadCount
            // 
            this.threadCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.threadCount.Location = new System.Drawing.Point(87, 78);
            this.threadCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.threadCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threadCount.Name = "threadCount";
            this.threadCount.Size = new System.Drawing.Size(53, 20);
            this.threadCount.TabIndex = 11;
            this.threadCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // delay
            // 
            this.delay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.delay.Location = new System.Drawing.Point(232, 78);
            this.delay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.delay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.delay.Name = "delay";
            this.delay.Size = new System.Drawing.Size(54, 20);
            this.delay.TabIndex = 14;
            this.delay.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(172, 85);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(37, 13);
            this.delayLabel.TabIndex = 13;
            this.delayLabel.Text = "Delay:";
            // 
            // threadLabel
            // 
            this.threadLabel.AutoSize = true;
            this.threadLabel.Location = new System.Drawing.Point(6, 85);
            this.threadLabel.Name = "threadLabel";
            this.threadLabel.Size = new System.Drawing.Size(75, 13);
            this.threadLabel.TabIndex = 12;
            this.threadLabel.Text = "Thread Count:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 187);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(300, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 137);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            // 
            // WoWHeadParserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(327, 222);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Name = "WoWHeadParserForm";
            this.Text = "WowHead Parser";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox parserBox;
        private System.Windows.Forms.ComboBox localeBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label threadLabel;
        private System.Windows.Forms.NumericUpDown delay;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.NumericUpDown threadCount;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button startButton;


    }
}

