using System.ComponentModel;
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
            System.Windows.Forms.Label rangeEndLabel;
            System.Windows.Forms.Label rangeStartLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WoWHeadParserForm));
            this.parserBox = new System.Windows.Forms.ComboBox();
            this.localeBox = new System.Windows.Forms.ComboBox();
            this.settingsBox = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.singleTab = new System.Windows.Forms.TabPage();
            this.singleBox = new System.Windows.Forms.GroupBox();
            this.valueBox = new System.Windows.Forms.NumericUpDown();
            this.valueLabel = new System.Windows.Forms.Label();
            this.multipleTab = new System.Windows.Forms.TabPage();
            this.multipleBox = new System.Windows.Forms.GroupBox();
            this.rangeStart = new System.Windows.Forms.NumericUpDown();
            this.rangeEnd = new System.Windows.Forms.NumericUpDown();
            this.listTab = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.welfBox = new System.Windows.Forms.ComboBox();
            this.entryCountLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.startButton = new System.Windows.Forms.Button();
            this.progressLabel = new System.Windows.Forms.Label();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.stopButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wELFCreatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadwelfFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            rangeEndLabel = new System.Windows.Forms.Label();
            rangeStartLabel = new System.Windows.Forms.Label();
            this.settingsBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.singleTab.SuspendLayout();
            this.singleBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueBox)).BeginInit();
            this.multipleTab.SuspendLayout();
            this.multipleBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeEnd)).BeginInit();
            this.listTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rangeEndLabel
            // 
            rangeEndLabel.AutoSize = true;
            rangeEndLabel.Location = new System.Drawing.Point(135, 26);
            rangeEndLabel.Name = "rangeEndLabel";
            rangeEndLabel.Size = new System.Drawing.Size(29, 13);
            rangeEndLabel.TabIndex = 13;
            rangeEndLabel.Text = "End:";
            // 
            // rangeStartLabel
            // 
            rangeStartLabel.AutoSize = true;
            rangeStartLabel.Location = new System.Drawing.Point(6, 26);
            rangeStartLabel.Name = "rangeStartLabel";
            rangeStartLabel.Size = new System.Drawing.Size(32, 13);
            rangeStartLabel.TabIndex = 12;
            rangeStartLabel.Text = "Start:";
            // 
            // parserBox
            // 
            this.parserBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parserBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parserBox.FormattingEnabled = true;
            this.parserBox.Location = new System.Drawing.Point(66, 19);
            this.parserBox.Name = "parserBox";
            this.parserBox.Size = new System.Drawing.Size(242, 21);
            this.parserBox.TabIndex = 1;
            this.parserBox.SelectedIndexChanged += new System.EventHandler(this.ParserIndexChanged);
            // 
            // localeBox
            // 
            this.localeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.localeBox.FormattingEnabled = true;
            this.localeBox.Items.AddRange(new object[] {
            "",
            "ru.",
            "de.",
            "fr.",
            "es."});
            this.localeBox.Location = new System.Drawing.Point(6, 19);
            this.localeBox.Name = "localeBox";
            this.localeBox.Size = new System.Drawing.Size(54, 21);
            this.localeBox.TabIndex = 3;
            // 
            // settingsBox
            // 
            this.settingsBox.Controls.Add(this.tabControl1);
            this.settingsBox.Controls.Add(this.localeBox);
            this.settingsBox.Controls.Add(this.parserBox);
            this.settingsBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsBox.Location = new System.Drawing.Point(6, 28);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new System.Drawing.Size(376, 144);
            this.settingsBox.TabIndex = 4;
            this.settingsBox.TabStop = false;
            this.settingsBox.Text = "Settings";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.singleTab);
            this.tabControl1.Controls.Add(this.multipleTab);
            this.tabControl1.Controls.Add(this.listTab);
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl1.Location = new System.Drawing.Point(6, 46);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(364, 88);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 9;
            // 
            // singleTab
            // 
            this.singleTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.singleTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.singleTab.Controls.Add(this.singleBox);
            this.singleTab.Location = new System.Drawing.Point(4, 22);
            this.singleTab.Name = "singleTab";
            this.singleTab.Padding = new System.Windows.Forms.Padding(3);
            this.singleTab.Size = new System.Drawing.Size(356, 62);
            this.singleTab.TabIndex = 0;
            this.singleTab.Text = "Single";
            // 
            // singleBox
            // 
            this.singleBox.Controls.Add(this.valueBox);
            this.singleBox.Controls.Add(this.valueLabel);
            this.singleBox.Location = new System.Drawing.Point(6, 6);
            this.singleBox.Name = "singleBox";
            this.singleBox.Size = new System.Drawing.Size(338, 49);
            this.singleBox.TabIndex = 14;
            this.singleBox.TabStop = false;
            this.singleBox.Text = "Settings";
            // 
            // valueBox
            // 
            this.valueBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.valueBox.Location = new System.Drawing.Point(49, 19);
            this.valueBox.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.valueBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valueBox.Name = "valueBox";
            this.valueBox.Size = new System.Drawing.Size(53, 20);
            this.valueBox.TabIndex = 12;
            this.valueBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(6, 26);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(37, 13);
            this.valueLabel.TabIndex = 13;
            this.valueLabel.Text = "Value:";
            // 
            // multipleTab
            // 
            this.multipleTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.multipleTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.multipleTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.multipleTab.Controls.Add(this.multipleBox);
            this.multipleTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.multipleTab.ForeColor = System.Drawing.SystemColors.ControlText;
            this.multipleTab.Location = new System.Drawing.Point(4, 22);
            this.multipleTab.Name = "multipleTab";
            this.multipleTab.Padding = new System.Windows.Forms.Padding(3);
            this.multipleTab.Size = new System.Drawing.Size(356, 62);
            this.multipleTab.TabIndex = 1;
            this.multipleTab.Text = "Multiple";
            // 
            // multipleBox
            // 
            this.multipleBox.Controls.Add(rangeStartLabel);
            this.multipleBox.Controls.Add(this.rangeStart);
            this.multipleBox.Controls.Add(rangeEndLabel);
            this.multipleBox.Controls.Add(this.rangeEnd);
            this.multipleBox.Location = new System.Drawing.Point(6, 6);
            this.multipleBox.Name = "multipleBox";
            this.multipleBox.Size = new System.Drawing.Size(338, 49);
            this.multipleBox.TabIndex = 0;
            this.multipleBox.TabStop = false;
            this.multipleBox.Text = "Settings";
            // 
            // rangeStart
            // 
            this.rangeStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.rangeStart.Location = new System.Drawing.Point(44, 19);
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
            this.rangeEnd.Location = new System.Drawing.Point(170, 19);
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
            // listTab
            // 
            this.listTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.listTab.Controls.Add(this.groupBox1);
            this.listTab.Location = new System.Drawing.Point(4, 22);
            this.listTab.Name = "listTab";
            this.listTab.Padding = new System.Windows.Forms.Padding(3);
            this.listTab.Size = new System.Drawing.Size(356, 62);
            this.listTab.TabIndex = 2;
            this.listTab.Text = "List";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.welfBox);
            this.groupBox1.Controls.Add(this.entryCountLabel);
            this.groupBox1.Location = new System.Drawing.Point(9, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 49);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // welfBox
            // 
            this.welfBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.welfBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.welfBox.FormattingEnabled = true;
            this.welfBox.Location = new System.Drawing.Point(16, 19);
            this.welfBox.Name = "welfBox";
            this.welfBox.Size = new System.Drawing.Size(187, 21);
            this.welfBox.TabIndex = 2;
            this.welfBox.SelectedIndexChanged += new System.EventHandler(this.WelfBoxSelectedIndexChanged);
            // 
            // entryCountLabel
            // 
            this.entryCountLabel.AutoSize = true;
            this.entryCountLabel.Location = new System.Drawing.Point(230, 27);
            this.entryCountLabel.Name = "entryCountLabel";
            this.entryCountLabel.Size = new System.Drawing.Size(43, 13);
            this.entryCountLabel.TabIndex = 1;
            this.entryCountLabel.Text = "<none>";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 210);
            this.progressBar.Maximum = 500000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(376, 26);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 5;
            this.progressBar.Visible = false;
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(6, 178);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 26);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(271, 191);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(43, 13);
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
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorkerProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerRunWorkerCompleted);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(103, 178);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 26);
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButtonClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(387, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.fileToolStripMenuItem.Text = "File...";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wELFCreatorToolStripMenuItem,
            this.reloadwelfFilesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // wELFCreatorToolStripMenuItem
            // 
            this.wELFCreatorToolStripMenuItem.Name = "wELFCreatorToolStripMenuItem";
            this.wELFCreatorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.wELFCreatorToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.wELFCreatorToolStripMenuItem.Text = "WELF Creator...";
            this.wELFCreatorToolStripMenuItem.Click += new System.EventHandler(this.WELFCreatorToolStripMenuItemClick);
            // 
            // reloadwelfFilesToolStripMenuItem
            // 
            this.reloadwelfFilesToolStripMenuItem.Name = "reloadwelfFilesToolStripMenuItem";
            this.reloadwelfFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.reloadwelfFilesToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.reloadwelfFilesToolStripMenuItem.Text = "Reload .welf files...";
            this.reloadwelfFilesToolStripMenuItem.Click += new System.EventHandler(this.ReloadWelfFilesToolStripMenuItemClick);
            // 
            // WoWHeadParserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(387, 243);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.settingsBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "WoWHeadParserForm";
            this.Text = "WowHead Parser";
            this.settingsBox.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.singleTab.ResumeLayout(false);
            this.singleBox.ResumeLayout(false);
            this.singleBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueBox)).EndInit();
            this.multipleTab.ResumeLayout(false);
            this.multipleBox.ResumeLayout(false);
            this.multipleBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeEnd)).EndInit();
            this.listTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox parserBox;
        private ComboBox localeBox;
        private GroupBox settingsBox;
        private ProgressBar progressBar;
        private Button startButton;
        private Label progressLabel;
        private SaveFileDialog saveDialog;
        private BackgroundWorker backgroundWorker;
        private Button stopButton;
        private TabControl tabControl1;
        private TabPage singleTab;
        private TabPage multipleTab;
        private GroupBox multipleBox;
        private NumericUpDown rangeStart;
        private NumericUpDown rangeEnd;
        private GroupBox singleBox;
        private Label valueLabel;
        private NumericUpDown valueBox;
        private TabPage listTab;
        private GroupBox groupBox1;
        private Label entryCountLabel;
        private ComboBox welfBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem wELFCreatorToolStripMenuItem;
        private ToolStripMenuItem reloadwelfFilesToolStripMenuItem;
    }
}

