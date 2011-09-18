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
            System.Windows.Forms.Label rangeEndLabel;
            System.Windows.Forms.Label rangeStartLabel;
            System.Windows.Forms.Label threadCountLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WoWHeadParserForm));
            this.parserBox = new System.Windows.Forms.ComboBox();
            this.localeBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.threadCount = new System.Windows.Forms.NumericUpDown();
            this.rangeStart = new System.Windows.Forms.NumericUpDown();
            this.rangeEnd = new System.Windows.Forms.NumericUpDown();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.startButton = new System.Windows.Forms.Button();
            rangeEndLabel = new System.Windows.Forms.Label();
            rangeStartLabel = new System.Windows.Forms.Label();
            threadCountLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeEnd)).BeginInit();
            this.SuspendLayout();
            // 
            // rangeEndLabel
            // 
            rangeEndLabel.AutoSize = true;
            rangeEndLabel.Location = new System.Drawing.Point(103, 80);
            rangeEndLabel.Name = "rangeEndLabel";
            rangeEndLabel.Size = new System.Drawing.Size(29, 13);
            rangeEndLabel.TabIndex = 13;
            rangeEndLabel.Text = "End:";
            // 
            // rangeStartLabel
            // 
            rangeStartLabel.AutoSize = true;
            rangeStartLabel.Location = new System.Drawing.Point(6, 80);
            rangeStartLabel.Name = "rangeStartLabel";
            rangeStartLabel.Size = new System.Drawing.Size(32, 13);
            rangeStartLabel.TabIndex = 12;
            rangeStartLabel.Text = "Start:";
            // 
            // threadCountLabel
            // 
            threadCountLabel.AutoSize = true;
            threadCountLabel.Location = new System.Drawing.Point(198, 80);
            threadCountLabel.Name = "threadCountLabel";
            threadCountLabel.Size = new System.Drawing.Size(71, 13);
            threadCountLabel.TabIndex = 14;
            threadCountLabel.Text = "Thread count";
            // 
            // parserBox
            // 
            this.parserBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parserBox.FormattingEnabled = true;
            this.parserBox.Location = new System.Drawing.Point(59, 30);
            this.parserBox.Name = "parserBox";
            this.parserBox.Size = new System.Drawing.Size(227, 21);
            this.parserBox.TabIndex = 1;
            this.parserBox.SelectedIndexChanged += new System.EventHandler(this.ParserIndexChanged);
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
            this.groupBox1.Controls.Add(this.threadCount);
            this.groupBox1.Controls.Add(threadCountLabel);
            this.groupBox1.Controls.Add(this.rangeStart);
            this.groupBox1.Controls.Add(this.rangeEnd);
            this.groupBox1.Controls.Add(rangeEndLabel);
            this.groupBox1.Controls.Add(rangeStartLabel);
            this.groupBox1.Controls.Add(this.localeBox);
            this.groupBox1.Controls.Add(this.parserBox);
            this.groupBox1.Location = new System.Drawing.Point(6, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 104);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // threadCount
            // 
            this.threadCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.threadCount.Location = new System.Drawing.Point(275, 78);
            this.threadCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threadCount.Name = "threadCount";
            this.threadCount.Size = new System.Drawing.Size(54, 20);
            this.threadCount.TabIndex = 15;
            this.threadCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rangeStart
            // 
            this.rangeStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.rangeStart.Location = new System.Drawing.Point(44, 78);
            this.rangeStart.Maximum = new decimal(new int[] {
            500000,
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
            this.rangeEnd.Location = new System.Drawing.Point(138, 78);
            this.rangeEnd.Maximum = new decimal(new int[] {
            500000,
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
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 166);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(354, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
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
            this.startButton.Enabled = false;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // WoWHeadParserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(372, 197);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WoWHeadParserForm";
            this.Text = "WowHead Parser";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeEnd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox parserBox;
        private System.Windows.Forms.ComboBox localeBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown rangeEnd;
        private System.Windows.Forms.NumericUpDown rangeStart;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.NumericUpDown threadCount;


    }
}

