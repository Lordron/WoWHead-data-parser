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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WoWHeadParserForm));
            this.rangeEndLabel = new System.Windows.Forms.Label();
            this.rangeStartLabel = new System.Windows.Forms.Label();
            this.parserBox = new System.Windows.Forms.ComboBox();
            this.localeBox = new System.Windows.Forms.ComboBox();
            this.settingsBox = new System.Windows.Forms.GroupBox();
            this.parsingControl = new System.Windows.Forms.TabControl();
            this.singleTab = new System.Windows.Forms.TabPage();
            this.singleBox = new System.Windows.Forms.GroupBox();
            this.valueBox = new System.Windows.Forms.NumericUpDown();
            this.valueLabel = new System.Windows.Forms.Label();
            this.multipleTab = new System.Windows.Forms.TabPage();
            this.multipleBox = new System.Windows.Forms.GroupBox();
            this.rangeStart = new System.Windows.Forms.NumericUpDown();
            this.rangeEnd = new System.Windows.Forms.NumericUpDown();
            this.listTab = new System.Windows.Forms.TabPage();
            this.listBox = new System.Windows.Forms.GroupBox();
            this.welfBox = new System.Windows.Forms.ComboBox();
            this.entryCountLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.startButton = new System.Windows.Forms.Button();
            this.progressLabel = new System.Windows.Forms.Label();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.abortButton = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wELFCreatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadwelfFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.settingsBox.SuspendLayout();
            this.parsingControl.SuspendLayout();
            this.singleTab.SuspendLayout();
            this.singleBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueBox)).BeginInit();
            this.multipleTab.SuspendLayout();
            this.multipleBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeEnd)).BeginInit();
            this.listTab.SuspendLayout();
            this.listBox.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // rangeEndLabel
            // 
            this.rangeEndLabel.AutoSize = true;
            this.rangeEndLabel.Location = new System.Drawing.Point(135, 21);
            this.rangeEndLabel.Name = "rangeEndLabel";
            this.rangeEndLabel.Size = new System.Drawing.Size(29, 13);
            this.rangeEndLabel.TabIndex = 13;
            this.rangeEndLabel.Text = "End:";
            // 
            // rangeStartLabel
            // 
            this.rangeStartLabel.AutoSize = true;
            this.rangeStartLabel.Location = new System.Drawing.Point(6, 21);
            this.rangeStartLabel.Name = "rangeStartLabel";
            this.rangeStartLabel.Size = new System.Drawing.Size(32, 13);
            this.rangeStartLabel.TabIndex = 12;
            this.rangeStartLabel.Text = "Start:";
            // 
            // parserBox
            // 
            this.parserBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parserBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parserBox.Location = new System.Drawing.Point(80, 19);
            this.parserBox.Name = "parserBox";
            this.parserBox.Size = new System.Drawing.Size(253, 21);
            this.parserBox.TabIndex = 1;
            this.parserBox.SelectedIndexChanged += new System.EventHandler(this.ParserIndexChanged);
            // 
            // localeBox
            // 
            this.localeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.localeBox.Items.AddRange(new object[] {
            "",
            "ru.",
            "de.",
            "fr.",
            "es."});
            this.localeBox.Location = new System.Drawing.Point(6, 19);
            this.localeBox.Name = "localeBox";
            this.localeBox.Size = new System.Drawing.Size(68, 21);
            this.localeBox.TabIndex = 3;
            // 
            // settingsBox
            // 
            this.settingsBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.settingsBox.Controls.Add(this.parsingControl);
            this.settingsBox.Controls.Add(this.localeBox);
            this.settingsBox.Controls.Add(this.parserBox);
            this.settingsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsBox.Location = new System.Drawing.Point(0, 24);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new System.Drawing.Size(387, 219);
            this.settingsBox.TabIndex = 4;
            this.settingsBox.TabStop = false;
            this.settingsBox.Text = "Settings";
            // 
            // parsingControl
            // 
            this.parsingControl.Controls.Add(this.singleTab);
            this.parsingControl.Controls.Add(this.multipleTab);
            this.parsingControl.Controls.Add(this.listTab);
            this.parsingControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.parsingControl.Location = new System.Drawing.Point(6, 46);
            this.parsingControl.Multiline = true;
            this.parsingControl.Name = "parsingControl";
            this.parsingControl.SelectedIndex = 0;
            this.parsingControl.Size = new System.Drawing.Size(364, 88);
            this.parsingControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.parsingControl.TabIndex = 9;
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
            -2147483648,
            0,
            0,
            0});
            this.valueBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valueBox.Name = "valueBox";
            this.valueBox.Size = new System.Drawing.Size(69, 20);
            this.valueBox.TabIndex = 12;
            this.valueBox.ThousandsSeparator = true;
            this.valueBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(6, 21);
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
            this.multipleBox.Controls.Add(this.rangeStartLabel);
            this.multipleBox.Controls.Add(this.rangeStart);
            this.multipleBox.Controls.Add(this.rangeEndLabel);
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
            -2147483648,
            0,
            0,
            0});
            this.rangeStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rangeStart.Name = "rangeStart";
            this.rangeStart.Size = new System.Drawing.Size(69, 20);
            this.rangeStart.TabIndex = 11;
            this.rangeStart.ThousandsSeparator = true;
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
            -2147483648,
            0,
            0,
            0});
            this.rangeEnd.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rangeEnd.Name = "rangeEnd";
            this.rangeEnd.Size = new System.Drawing.Size(69, 20);
            this.rangeEnd.TabIndex = 14;
            this.rangeEnd.ThousandsSeparator = true;
            this.rangeEnd.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // listTab
            // 
            this.listTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.listTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listTab.Controls.Add(this.listBox);
            this.listTab.Location = new System.Drawing.Point(4, 22);
            this.listTab.Name = "listTab";
            this.listTab.Padding = new System.Windows.Forms.Padding(3);
            this.listTab.Size = new System.Drawing.Size(356, 62);
            this.listTab.TabIndex = 2;
            this.listTab.Text = "List";
            // 
            // listBox
            // 
            this.listBox.Controls.Add(this.welfBox);
            this.listBox.Controls.Add(this.entryCountLabel);
            this.listBox.Location = new System.Drawing.Point(6, 6);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(338, 49);
            this.listBox.TabIndex = 15;
            this.listBox.TabStop = false;
            this.listBox.Text = "Settings";
            // 
            // welfBox
            // 
            this.welfBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.welfBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 217);
            this.progressBar.Maximum = 2147483647;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(387, 26);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 5;
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
            this.progressLabel.Location = new System.Drawing.Point(196, 188);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(43, 13);
            this.progressLabel.TabIndex = 7;
            this.progressLabel.Text = "<none>";
            // 
            // saveDialog
            // 
            this.saveDialog.Filter = "Structured Query Language| *.sql|Normal text file| *.txt|All Files| *.*";
            // 
            // abortButton
            // 
            this.abortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.abortButton.Enabled = false;
            this.abortButton.Location = new System.Drawing.Point(103, 178);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(75, 26);
            this.abortButton.TabIndex = 8;
            this.abortButton.Text = "Abort";
            this.abortButton.UseVisualStyleBackColor = true;
            this.abortButton.Click += new System.EventHandler(this.AbortButtonClick);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(387, 24);
            this.menuStrip.TabIndex = 9;
            this.menuStrip.Text = "menuStrip1";
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
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerDoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerRunWorkerCompleted);
            // 
            // numericUpDown
            // 
            this.numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown.Enabled = false;
            this.numericUpDown.Location = new System.Drawing.Point(276, 183);
            this.numericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            this.numericUpDown.Size = new System.Drawing.Size(106, 20);
            this.numericUpDown.TabIndex = 10;
            // 
            // WoWHeadParserForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.CancelButton = this.abortButton;
            this.ClientSize = new System.Drawing.Size(387, 243);
            this.Controls.Add(this.numericUpDown);
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.settingsBox);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "WoWHeadParserForm";
            this.Text = "WowHead Parser";
            this.settingsBox.ResumeLayout(false);
            this.parsingControl.ResumeLayout(false);
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
            this.listBox.ResumeLayout(false);
            this.listBox.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox parserBox;
        private ComboBox localeBox;
        private GroupBox settingsBox;
        private Button startButton;
        private Label progressLabel;
        private SaveFileDialog saveDialog;
        private Button abortButton;
        private TabControl parsingControl;
        private TabPage singleTab;
        private TabPage multipleTab;
        private GroupBox multipleBox;
        private NumericUpDown rangeStart;
        private NumericUpDown rangeEnd;
        private GroupBox singleBox;
        private Label valueLabel;
        private NumericUpDown valueBox;
        private TabPage listTab;
        private GroupBox listBox;
        private Label entryCountLabel;
        private ComboBox welfBox;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem wELFCreatorToolStripMenuItem;
        private ToolStripMenuItem reloadwelfFilesToolStripMenuItem;
        private BackgroundWorker backgroundWorker;
        private NumericUpDown numericUpDown;
        private ProgressBar progressBar;
        private Label rangeEndLabel;
        private Label rangeStartLabel;
    }
}

