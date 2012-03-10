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
            this.components = new System.ComponentModel.Container();
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
            this.consoleGroupBox = new System.Windows.Forms.GroupBox();
            this.consoleBox = new System.Windows.Forms.RichTextBox();
            this.progressLabel = new System.Windows.Forms.Label();
            this.numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.abortButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.editMenuItem = new System.Windows.Forms.MenuItem();
            this.reloadMenuItem = new System.Windows.Forms.MenuItem();
            this.launchMenuItem = new System.Windows.Forms.MenuItem();
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
            this.consoleGroupBox.SuspendLayout();
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
            this.parserBox.Size = new System.Drawing.Size(290, 21);
            this.parserBox.TabIndex = 1;
            this.parserBox.SelectedIndexChanged += new System.EventHandler(this.ParserIndexChanged);
            // 
            // localeBox
            // 
            this.localeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.settingsBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsBox.Location = new System.Drawing.Point(0, 3);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new System.Drawing.Size(379, 145);
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
            this.entryCountLabel.Size = new System.Drawing.Size(13, 13);
            this.entryCountLabel.TabIndex = 1;
            this.entryCountLabel.Text = "0";
            // 
            // consoleGroupBox
            // 
            this.consoleGroupBox.Controls.Add(this.consoleBox);
            this.consoleGroupBox.Location = new System.Drawing.Point(0, 154);
            this.consoleGroupBox.Name = "consoleGroupBox";
            this.consoleGroupBox.Size = new System.Drawing.Size(379, 134);
            this.consoleGroupBox.TabIndex = 11;
            this.consoleGroupBox.TabStop = false;
            this.consoleGroupBox.Text = "Console";
            // 
            // consoleBox
            // 
            this.consoleBox.BackColor = System.Drawing.Color.Black;
            this.consoleBox.Font = new System.Drawing.Font("Lucida Console", 8.25F);
            this.consoleBox.ForeColor = System.Drawing.Color.Cyan;
            this.consoleBox.Location = new System.Drawing.Point(6, 12);
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.ReadOnly = true;
            this.consoleBox.Size = new System.Drawing.Size(367, 116);
            this.consoleBox.TabIndex = 0;
            this.consoleBox.Text = "";
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(191, 301);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(0, 13);
            this.progressLabel.TabIndex = 7;
            // 
            // numericUpDown
            // 
            this.numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown.Enabled = false;
            this.numericUpDown.Location = new System.Drawing.Point(281, 299);
            this.numericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            this.numericUpDown.Size = new System.Drawing.Size(98, 20);
            this.numericUpDown.TabIndex = 10;
            // 
            // abortButton
            // 
            this.abortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.abortButton.Enabled = false;
            this.abortButton.Location = new System.Drawing.Point(99, 294);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(75, 26);
            this.abortButton.TabIndex = 8;
            this.abortButton.Text = "Abort";
            this.abortButton.UseVisualStyleBackColor = true;
            this.abortButton.Click += new System.EventHandler(this.AbortButtonClick);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(3, 294);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 26);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 327);
            this.progressBar.Maximum = 2147483647;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(379, 26);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 5;
            // 
            // saveDialog
            // 
            this.saveDialog.Filter = "Structured Query Language| *.sql|Normal text file| *.txt|All Files| *.*";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerDoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerRunWorkerCompleted);
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenuItem,
            this.editMenuItem});
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.exitMenuItem});
            this.fileMenuItem.Text = "File";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 0;
            this.exitMenuItem.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuClick);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Index = 1;
            this.editMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.reloadMenuItem,
            this.launchMenuItem});
            this.editMenuItem.Text = "Edit";
            // 
            // reloadMenuItem
            // 
            this.reloadMenuItem.Index = 0;
            this.reloadMenuItem.Text = "Reload .welf files";
            this.reloadMenuItem.Click += new System.EventHandler(this.ReloadWelfFilesMenuClick);
            // 
            // launchMenuItem
            // 
            this.launchMenuItem.Index = 1;
            this.launchMenuItem.Text = "Launch .welf creator";
            this.launchMenuItem.Click += new System.EventHandler(this.WELFCreatorMenuClick);
            // 
            // WoWHeadParserForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.CancelButton = this.abortButton;
            this.ClientSize = new System.Drawing.Size(379, 353);
            this.Controls.Add(this.consoleGroupBox);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.settingsBox);
            this.Controls.Add(this.numericUpDown);
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
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
            this.consoleGroupBox.ResumeLayout(false);
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
        private BackgroundWorker backgroundWorker;
        private NumericUpDown numericUpDown;
        private ProgressBar progressBar;
        private Label rangeEndLabel;
        private Label rangeStartLabel;
        private MainMenu mainMenu;
        private MenuItem fileMenuItem;
        private MenuItem exitMenuItem;
        private MenuItem editMenuItem;
        private MenuItem reloadMenuItem;
        private MenuItem launchMenuItem;
        private GroupBox consoleGroupBox;
        private RichTextBox consoleBox;
    }
}

