using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace WoWHeadParser
{
    partial class WoWHeadParserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            Label rangeEndLabel;
            Label rangeStartLabel;
            Label threadCountLabel;
            ComponentResourceManager resources = new ComponentResourceManager(typeof(WoWHeadParserForm));
            this.parserBox = new ComboBox();
            this.localeBox = new ComboBox();
            this.settingsBox = new GroupBox();
            this.threadCountBox = new NumericUpDown();
            this.rangeStart = new NumericUpDown();
            this.rangeEnd = new NumericUpDown();
            this.progressBar = new ProgressBar();
            this.startButton = new Button();
            this.progressLabel = new Label();
            this.saveDialog = new SaveFileDialog();
            this.backgroundWorker = new BackgroundWorker();
            this.stopButton = new Button();
            rangeEndLabel = new Label();
            rangeStartLabel = new Label();
            threadCountLabel = new Label();
            this.settingsBox.SuspendLayout();
            ((ISupportInitialize)(this.threadCountBox)).BeginInit();
            ((ISupportInitialize)(this.rangeStart)).BeginInit();
            ((ISupportInitialize)(this.rangeEnd)).BeginInit();
            this.SuspendLayout();
            // 
            // rangeEndLabel
            // 
            rangeEndLabel.AutoSize = true;
            rangeEndLabel.Location = new Point(103, 80);
            rangeEndLabel.Name = "rangeEndLabel";
            rangeEndLabel.Size = new Size(29, 13);
            rangeEndLabel.TabIndex = 13;
            rangeEndLabel.Text = "End:";
            // 
            // rangeStartLabel
            // 
            rangeStartLabel.AutoSize = true;
            rangeStartLabel.Location = new Point(6, 80);
            rangeStartLabel.Name = "rangeStartLabel";
            rangeStartLabel.Size = new Size(32, 13);
            rangeStartLabel.TabIndex = 12;
            rangeStartLabel.Text = "Start:";
            // 
            // threadCountLabel
            // 
            threadCountLabel.AutoSize = true;
            threadCountLabel.Location = new Point(198, 80);
            threadCountLabel.Name = "threadCountLabel";
            threadCountLabel.Size = new Size(71, 13);
            threadCountLabel.TabIndex = 14;
            threadCountLabel.Text = "Thread count";
            // 
            // parserBox
            // 
            this.parserBox.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.parserBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.parserBox.FormattingEnabled = true;
            this.parserBox.Location = new Point(59, 30);
            this.parserBox.Name = "parserBox";
            this.parserBox.Size = new Size(227, 21);
            this.parserBox.TabIndex = 1;
            this.parserBox.SelectedIndexChanged += new System.EventHandler(this.ParserIndexChanged);
            // 
            // localeBox
            // 
            this.localeBox.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.localeBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.localeBox.FormattingEnabled = true;
            this.localeBox.Items.AddRange(new object[] {
            "",
            "ru.",
            "de.",
            "fr.",
            "es."});
            this.localeBox.Location = new Point(6, 30);
            this.localeBox.Name = "localeBox";
            this.localeBox.Size = new Size(47, 21);
            this.localeBox.TabIndex = 3;
            // 
            // settingsBox
            // 
            this.settingsBox.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.settingsBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.settingsBox.Controls.Add(this.threadCountBox);
            this.settingsBox.Controls.Add(threadCountLabel);
            this.settingsBox.Controls.Add(this.rangeStart);
            this.settingsBox.Controls.Add(this.rangeEnd);
            this.settingsBox.Controls.Add(rangeEndLabel);
            this.settingsBox.Controls.Add(rangeStartLabel);
            this.settingsBox.Controls.Add(this.localeBox);
            this.settingsBox.Controls.Add(this.parserBox);
            this.settingsBox.Location = new Point(6, 4);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new Size(361, 104);
            this.settingsBox.TabIndex = 4;
            this.settingsBox.TabStop = false;
            this.settingsBox.Text = "Settings";
            // 
            // threadCountBox
            // 
            this.threadCountBox.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.threadCountBox.Location = new Point(275, 78);
            this.threadCountBox.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.threadCountBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threadCountBox.Name = "threadCountBox";
            this.threadCountBox.Size = new Size(54, 20);
            this.threadCountBox.TabIndex = 15;
            this.threadCountBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rangeStart
            // 
            this.rangeStart.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.rangeStart.Location = new Point(44, 78);
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
            this.rangeStart.Size = new Size(53, 20);
            this.rangeStart.TabIndex = 11;
            this.rangeStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rangeEnd
            // 
            this.rangeEnd.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.rangeEnd.Location = new Point(138, 78);
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
            this.rangeEnd.Size = new Size(54, 20);
            this.rangeEnd.TabIndex = 14;
            this.rangeEnd.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.progressBar.Location = new Point(12, 166);
            this.progressBar.Maximum = 500000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(354, 23);
            this.progressBar.Style = ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 5;
            // 
            // startButton
            // 
            this.startButton.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.startButton.Enabled = false;
            this.startButton.Location = new Point(12, 137);
            this.startButton.Name = "startButton";
            this.startButton.Size = new Size(75, 23);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new EventHandler(this.StartButtonClick);
            // 
            // progressLabel
            // 
            this.progressLabel.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new Point(249, 142);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new Size(43, 13);
            this.progressLabel.TabIndex = 7;
            this.progressLabel.Text = "<none>";
            // 
            // saveDialog
            // 
            this.saveDialog.Filter = "Structured Query Language (.sql)| *.sql|All Files|*.*";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.stopButton.Enabled = false;
            this.stopButton.Location = new Point(112, 137);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new Size(75, 23);
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new EventHandler(this.StopButtonClick);
            // 
            // WoWHeadParserForm
            // 
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new Size(372, 197);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.settingsBox);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WoWHeadParserForm";
            this.Text = "WowHead Parser";
            this.settingsBox.ResumeLayout(false);
            this.settingsBox.PerformLayout();
            ((ISupportInitialize)(this.threadCountBox)).EndInit();
            ((ISupportInitialize)(this.rangeStart)).EndInit();
            ((ISupportInitialize)(this.rangeEnd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox parserBox;
        private ComboBox localeBox;
        private GroupBox settingsBox;
        private NumericUpDown rangeEnd;
        private NumericUpDown rangeStart;
        private ProgressBar progressBar;
        private Button startButton;
        private NumericUpDown threadCountBox;
        private Label progressLabel;
        private SaveFileDialog saveDialog;
        private BackgroundWorker backgroundWorker;
        private Button stopButton;
    }
}

