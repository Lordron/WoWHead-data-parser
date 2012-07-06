using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WoWHeadParser.Properties;
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
            this.reloadButton = new System.Windows.Forms.Button();
            this.welfBox = new System.Windows.Forms.ComboBox();
            this.entryCountLabel = new System.Windows.Forms.Label();
            this.wowheadFilterTab = new System.Windows.Forms.TabPage();
            this.notifyLabel = new System.Windows.Forms.Label();
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
            this.launchMenuItem = new System.Windows.Forms.MenuItem();
            this.separatorMenuItem = new System.Windows.Forms.MenuItem();
            this.optionsMenuItem = new System.Windows.Forms.MenuItem();
            this.languageMenuItem = new System.Windows.Forms.MenuItem();
            this.aboutMenuItem = new System.Windows.Forms.MenuItem();
            this.subparsersListBox = new System.Windows.Forms.CheckedListBox();
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
            this.wowheadFilterTab.SuspendLayout();
            this.consoleGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // rangeEndLabel
            // 
            resources.ApplyResources(this.rangeEndLabel, "rangeEndLabel");
            this.rangeEndLabel.Name = "rangeEndLabel";
            // 
            // rangeStartLabel
            // 
            resources.ApplyResources(this.rangeStartLabel, "rangeStartLabel");
            this.rangeStartLabel.Name = "rangeStartLabel";
            // 
            // parserBox
            // 
            resources.ApplyResources(this.parserBox, "parserBox");
            this.parserBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parserBox.Name = "parserBox";
            this.parserBox.SelectedIndexChanged += new System.EventHandler(this.ParserBoxSelectedIndexChanged);
            // 
            // localeBox
            // 
            resources.ApplyResources(this.localeBox, "localeBox");
            this.localeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.localeBox.Name = "localeBox";
            this.localeBox.SelectedIndexChanged += new System.EventHandler(this.LocaleBoxSelectedIndexChanged);
            // 
            // settingsBox
            // 
            resources.ApplyResources(this.settingsBox, "settingsBox");
            this.settingsBox.Controls.Add(this.parsingControl);
            this.settingsBox.Controls.Add(this.localeBox);
            this.settingsBox.Controls.Add(this.parserBox);
            this.settingsBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.TabStop = false;
            // 
            // parsingControl
            // 
            this.parsingControl.Controls.Add(this.singleTab);
            this.parsingControl.Controls.Add(this.multipleTab);
            this.parsingControl.Controls.Add(this.listTab);
            this.parsingControl.Controls.Add(this.wowheadFilterTab);
            resources.ApplyResources(this.parsingControl, "parsingControl");
            this.parsingControl.Multiline = true;
            this.parsingControl.Name = "parsingControl";
            this.parsingControl.SelectedIndex = 0;
            this.parsingControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            // 
            // singleTab
            // 
            this.singleTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.singleTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.singleTab.Controls.Add(this.singleBox);
            resources.ApplyResources(this.singleTab, "singleTab");
            this.singleTab.Name = "singleTab";
            // 
            // singleBox
            // 
            this.singleBox.Controls.Add(this.valueBox);
            this.singleBox.Controls.Add(this.valueLabel);
            resources.ApplyResources(this.singleBox, "singleBox");
            this.singleBox.Name = "singleBox";
            this.singleBox.TabStop = false;
            // 
            // valueBox
            // 
            this.valueBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.valueBox, "valueBox");
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
            this.valueBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // valueLabel
            // 
            resources.ApplyResources(this.valueLabel, "valueLabel");
            this.valueLabel.Name = "valueLabel";
            // 
            // multipleTab
            // 
            this.multipleTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.multipleTab, "multipleTab");
            this.multipleTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.multipleTab.Controls.Add(this.multipleBox);
            this.multipleTab.ForeColor = System.Drawing.SystemColors.ControlText;
            this.multipleTab.Name = "multipleTab";
            // 
            // multipleBox
            // 
            this.multipleBox.Controls.Add(this.rangeStartLabel);
            this.multipleBox.Controls.Add(this.rangeStart);
            this.multipleBox.Controls.Add(this.rangeEndLabel);
            this.multipleBox.Controls.Add(this.rangeEnd);
            resources.ApplyResources(this.multipleBox, "multipleBox");
            this.multipleBox.Name = "multipleBox";
            this.multipleBox.TabStop = false;
            // 
            // rangeStart
            // 
            this.rangeStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.rangeStart, "rangeStart");
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
            this.rangeStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rangeEnd
            // 
            this.rangeEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.rangeEnd, "rangeEnd");
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
            resources.ApplyResources(this.listTab, "listTab");
            this.listTab.Name = "listTab";
            // 
            // listBox
            // 
            this.listBox.Controls.Add(this.reloadButton);
            this.listBox.Controls.Add(this.welfBox);
            this.listBox.Controls.Add(this.entryCountLabel);
            resources.ApplyResources(this.listBox, "listBox");
            this.listBox.Name = "listBox";
            this.listBox.TabStop = false;
            // 
            // reloadButton
            // 
            resources.ApplyResources(this.reloadButton, "reloadButton");
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.ReloadWelfFilesButtonClick);
            // 
            // welfBox
            // 
            resources.ApplyResources(this.welfBox, "welfBox");
            this.welfBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.welfBox.Name = "welfBox";
            this.welfBox.SelectedIndexChanged += new System.EventHandler(this.WelfBoxSelectedIndexChanged);
            // 
            // entryCountLabel
            // 
            resources.ApplyResources(this.entryCountLabel, "entryCountLabel");
            this.entryCountLabel.Name = "entryCountLabel";
            // 
            // wowheadFilterTab
            // 
            this.wowheadFilterTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.wowheadFilterTab.Controls.Add(this.notifyLabel);
            resources.ApplyResources(this.wowheadFilterTab, "wowheadFilterTab");
            this.wowheadFilterTab.Name = "wowheadFilterTab";
            // 
            // notifyLabel
            // 
            resources.ApplyResources(this.notifyLabel, "notifyLabel");
            this.notifyLabel.Name = "notifyLabel";
            // 
            // consoleGroupBox
            // 
            this.consoleGroupBox.Controls.Add(this.consoleBox);
            resources.ApplyResources(this.consoleGroupBox, "consoleGroupBox");
            this.consoleGroupBox.Name = "consoleGroupBox";
            this.consoleGroupBox.TabStop = false;
            // 
            // consoleBox
            // 
            this.consoleBox.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.consoleBox, "consoleBox");
            this.consoleBox.ForeColor = System.Drawing.Color.Cyan;
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.ReadOnly = true;
            // 
            // progressLabel
            // 
            resources.ApplyResources(this.progressLabel, "progressLabel");
            this.progressLabel.Name = "progressLabel";
            // 
            // numericUpDown
            // 
            resources.ApplyResources(this.numericUpDown, "numericUpDown");
            this.numericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            // 
            // abortButton
            // 
            this.abortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.abortButton, "abortButton");
            this.abortButton.Name = "abortButton";
            this.abortButton.UseVisualStyleBackColor = true;
            this.abortButton.Click += new System.EventHandler(this.AbortButtonClick);
            // 
            // startButton
            // 
            resources.ApplyResources(this.startButton, "startButton");
            this.startButton.Name = "startButton";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Maximum = 2147483647;
            this.progressBar.Name = "progressBar";
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // saveDialog
            // 
            resources.ApplyResources(this.saveDialog, "saveDialog");
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
            this.editMenuItem,
            this.languageMenuItem,
            this.aboutMenuItem});
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.exitMenuItem});
            resources.ApplyResources(this.fileMenuItem, "fileMenuItem");
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 0;
            resources.ApplyResources(this.exitMenuItem, "exitMenuItem");
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuClick);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Index = 1;
            this.editMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.launchMenuItem,
            this.separatorMenuItem,
            this.optionsMenuItem});
            resources.ApplyResources(this.editMenuItem, "editMenuItem");
            // 
            // launchMenuItem
            // 
            this.launchMenuItem.Index = 0;
            resources.ApplyResources(this.launchMenuItem, "launchMenuItem");
            this.launchMenuItem.Click += new System.EventHandler(this.WELFCreatorMenuClick);
            // 
            // separatorMenuItem
            // 
            this.separatorMenuItem.Index = 1;
            resources.ApplyResources(this.separatorMenuItem, "separatorMenuItem");
            // 
            // optionsMenuItem
            // 
            this.optionsMenuItem.Index = 2;
            resources.ApplyResources(this.optionsMenuItem, "optionsMenuItem");
            this.optionsMenuItem.Click += new System.EventHandler(this.OptionsMenuItemClick);
            // 
            // languageMenuItem
            // 
            this.languageMenuItem.Index = 2;
            resources.ApplyResources(this.languageMenuItem, "languageMenuItem");
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Index = 3;
            resources.ApplyResources(this.aboutMenuItem, "aboutMenuItem");
            this.aboutMenuItem.Click += new System.EventHandler(this.AboutMenuItemClick);
            // 
            // subparsersListBox
            // 
            this.subparsersListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.subparsersListBox.FormattingEnabled = true;
            resources.ApplyResources(this.subparsersListBox, "subparsersListBox");
            this.subparsersListBox.Name = "subparsersListBox";
            // 
            // WoWHeadParserForm
            // 
            this.AcceptButton = this.startButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.CancelButton = this.abortButton;
            this.Controls.Add(this.subparsersListBox);
            this.Controls.Add(this.consoleGroupBox);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.settingsBox);
            this.Controls.Add(this.numericUpDown);
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu;
            this.Name = "WoWHeadParserForm";
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
            this.wowheadFilterTab.ResumeLayout(false);
            this.wowheadFilterTab.PerformLayout();
            this.consoleGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Windows Form language reloading

        private void Reload(CultureInfo cultureInfo)
        {
            Resources.Culture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ComponentResourceManager resources = new ComponentResourceManager(typeof(WoWHeadParserForm));

            resources.ApplyResources(this.rangeEndLabel, "rangeEndLabel");
            resources.ApplyResources(this.rangeStartLabel, "rangeStartLabel");
            resources.ApplyResources(this.parserBox, "parserBox");
            resources.ApplyResources(this.localeBox, "localeBox");
            resources.ApplyResources(this.settingsBox, "settingsBox");
            resources.ApplyResources(this.parsingControl, "parsingControl");
            resources.ApplyResources(this.singleTab, "singleTab");
            resources.ApplyResources(this.singleBox, "singleBox");
            resources.ApplyResources(this.valueBox, "valueBox");
            resources.ApplyResources(this.valueLabel, "valueLabel");
            resources.ApplyResources(this.multipleTab, "multipleTab");
            resources.ApplyResources(this.multipleBox, "multipleBox");
            resources.ApplyResources(this.rangeStart, "rangeStart");
            resources.ApplyResources(this.rangeEnd, "rangeEnd");
            resources.ApplyResources(this.listTab, "listTab");
            resources.ApplyResources(this.listBox, "listBox");
            resources.ApplyResources(this.reloadButton, "reloadButton");
            resources.ApplyResources(this.welfBox, "welfBox");
            resources.ApplyResources(this.entryCountLabel, "entryCountLabel");
            resources.ApplyResources(this.wowheadFilterTab, "wowheadFilterTab");
            resources.ApplyResources(this.notifyLabel, "notifyLabel");
            resources.ApplyResources(this.consoleGroupBox, "consoleGroupBox");
            resources.ApplyResources(this.consoleBox, "consoleBox");
            resources.ApplyResources(this.progressLabel, "progressLabel");
            resources.ApplyResources(this.numericUpDown, "numericUpDown");
            resources.ApplyResources(this.abortButton, "abortButton");
            resources.ApplyResources(this.startButton, "startButton");
            resources.ApplyResources(this.progressBar, "progressBar");
            resources.ApplyResources(this.saveDialog, "saveDialog");
            resources.ApplyResources(this.mainMenu, "mainMenu");
            resources.ApplyResources(this.fileMenuItem, "fileMenuItem");
            resources.ApplyResources(this.exitMenuItem, "exitMenuItem");
            resources.ApplyResources(this.editMenuItem, "editMenuItem");
            resources.ApplyResources(this.launchMenuItem, "launchMenuItem");
            resources.ApplyResources(this.separatorMenuItem, "separatorMenuItem");
            resources.ApplyResources(this.optionsMenuItem, "optionsMenuItem");
            resources.ApplyResources(this.languageMenuItem, "languageMenuItem");
            resources.ApplyResources(this.aboutMenuItem, "aboutMenuItem");
            resources.ApplyResources(this, "$this");
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
        private MenuItem launchMenuItem;
        private GroupBox consoleGroupBox;
        private RichTextBox consoleBox;
        private TabPage wowheadFilterTab;
        private Label notifyLabel;
        private MenuItem optionsMenuItem;
        private MenuItem separatorMenuItem;
        private Button reloadButton;
        private MenuItem aboutMenuItem;
        private MenuItem languageMenuItem;
        private CheckedListBox subparsersListBox;
    }
}

