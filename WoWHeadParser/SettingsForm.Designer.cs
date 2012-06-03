using WoWHeadParser.Properties;
namespace WoWHeadParser
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.quertTypelabel = new System.Windows.Forms.Label();
            this.sqlQueryTypeBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancleButton = new System.Windows.Forms.Button();
            this.dirLabel = new System.Windows.Forms.Label();
            this.sortDirectionBox = new System.Windows.Forms.ComboBox();
            this.appendSqlCheckBox = new System.Windows.Forms.CheckBox();
            this.appendDeleteQueryCheckBox = new System.Windows.Forms.CheckBox();
            this.allowNullValCheckBox = new System.Windows.Forms.CheckBox();
            this.withoutHeaderCheckBox = new System.Windows.Forms.CheckBox();
            this.sqlBuilderGroupBox = new System.Windows.Forms.GroupBox();
            this.parserGroupBox = new System.Windows.Forms.GroupBox();
            this.dataCompressionCheckBox = new System.Windows.Forms.CheckBox();
            this.webRequestGroupBox = new System.Windows.Forms.GroupBox();
            this.sqlBuilderGroupBox.SuspendLayout();
            this.parserGroupBox.SuspendLayout();
            this.webRequestGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // quertTypelabel
            // 
            resources.ApplyResources(this.quertTypelabel, "quertTypelabel");
            this.quertTypelabel.Name = "quertTypelabel";
            // 
            // sqlQueryTypeBox
            // 
            this.sqlQueryTypeBox.FormattingEnabled = true;
            resources.ApplyResources(this.sqlQueryTypeBox, "sqlQueryTypeBox");
            this.sqlQueryTypeBox.Name = "sqlQueryTypeBox";
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // cancleButton
            // 
            this.cancleButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancleButton, "cancleButton");
            this.cancleButton.Name = "cancleButton";
            this.cancleButton.UseVisualStyleBackColor = true;
            this.cancleButton.Click += new System.EventHandler(this.CancleButtonClick);
            // 
            // dirLabel
            // 
            resources.ApplyResources(this.dirLabel, "dirLabel");
            this.dirLabel.Name = "dirLabel";
            // 
            // sortDirectionBox
            // 
            this.sortDirectionBox.FormattingEnabled = true;
            resources.ApplyResources(this.sortDirectionBox, "sortDirectionBox");
            this.sortDirectionBox.Name = "sortDirectionBox";
            // 
            // appendSqlCheckBox
            // 
            resources.ApplyResources(this.appendSqlCheckBox, "appendSqlCheckBox");
            this.appendSqlCheckBox.Checked = global::WoWHeadParser.Properties.Settings.Default.Append;
            this.appendSqlCheckBox.Name = "appendSqlCheckBox";
            this.appendSqlCheckBox.UseVisualStyleBackColor = true;
            // 
            // appendDeleteQueryCheckBox
            // 
            resources.ApplyResources(this.appendDeleteQueryCheckBox, "appendDeleteQueryCheckBox");
            this.appendDeleteQueryCheckBox.Checked = global::WoWHeadParser.Properties.Settings.Default.AppendDeleteQuery;
            this.appendDeleteQueryCheckBox.Name = "appendDeleteQueryCheckBox";
            this.appendDeleteQueryCheckBox.UseVisualStyleBackColor = true;
            // 
            // allowNullValCheckBox
            // 
            resources.ApplyResources(this.allowNullValCheckBox, "allowNullValCheckBox");
            this.allowNullValCheckBox.Checked = global::WoWHeadParser.Properties.Settings.Default.AllowEmptyValues;
            this.allowNullValCheckBox.Name = "allowNullValCheckBox";
            this.allowNullValCheckBox.UseVisualStyleBackColor = true;
            // 
            // withoutHeaderCheckBox
            // 
            resources.ApplyResources(this.withoutHeaderCheckBox, "withoutHeaderCheckBox");
            this.withoutHeaderCheckBox.Checked = global::WoWHeadParser.Properties.Settings.Default.WithoutHeader;
            this.withoutHeaderCheckBox.Name = "withoutHeaderCheckBox";
            this.withoutHeaderCheckBox.UseVisualStyleBackColor = true;
            // 
            // sqlBuilderGroupBox
            // 
            this.sqlBuilderGroupBox.Controls.Add(this.allowNullValCheckBox);
            this.sqlBuilderGroupBox.Controls.Add(this.withoutHeaderCheckBox);
            this.sqlBuilderGroupBox.Controls.Add(this.appendDeleteQueryCheckBox);
            this.sqlBuilderGroupBox.Controls.Add(this.appendSqlCheckBox);
            this.sqlBuilderGroupBox.Controls.Add(this.quertTypelabel);
            this.sqlBuilderGroupBox.Controls.Add(this.sqlQueryTypeBox);
            resources.ApplyResources(this.sqlBuilderGroupBox, "sqlBuilderGroupBox");
            this.sqlBuilderGroupBox.Name = "sqlBuilderGroupBox";
            this.sqlBuilderGroupBox.TabStop = false;
            // 
            // parserGroupBox
            // 
            this.parserGroupBox.Controls.Add(this.dirLabel);
            this.parserGroupBox.Controls.Add(this.sortDirectionBox);
            resources.ApplyResources(this.parserGroupBox, "parserGroupBox");
            this.parserGroupBox.Name = "parserGroupBox";
            this.parserGroupBox.TabStop = false;
            // 
            // dataCompressionCheckBox
            // 
            resources.ApplyResources(this.dataCompressionCheckBox, "dataCompressionCheckBox");
            this.dataCompressionCheckBox.Checked = global::WoWHeadParser.Properties.Settings.Default.DataCompression;
            this.dataCompressionCheckBox.Name = "dataCompressionCheckBox";
            this.dataCompressionCheckBox.UseVisualStyleBackColor = true;
            // 
            // webRequestGroupBox
            // 
            this.webRequestGroupBox.Controls.Add(this.dataCompressionCheckBox);
            resources.ApplyResources(this.webRequestGroupBox, "webRequestGroupBox");
            this.webRequestGroupBox.Name = "webRequestGroupBox";
            this.webRequestGroupBox.TabStop = false;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancleButton;
            this.Controls.Add(this.webRequestGroupBox);
            this.Controls.Add(this.parserGroupBox);
            this.Controls.Add(this.sqlBuilderGroupBox);
            this.Controls.Add(this.cancleButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.sqlBuilderGroupBox.ResumeLayout(false);
            this.sqlBuilderGroupBox.PerformLayout();
            this.parserGroupBox.ResumeLayout(false);
            this.parserGroupBox.PerformLayout();
            this.webRequestGroupBox.ResumeLayout(false);
            this.webRequestGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox allowNullValCheckBox;
        private System.Windows.Forms.CheckBox appendDeleteQueryCheckBox;
        private System.Windows.Forms.Label quertTypelabel;
        private System.Windows.Forms.ComboBox sqlQueryTypeBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancleButton;
        private System.Windows.Forms.Label dirLabel;
        private System.Windows.Forms.ComboBox sortDirectionBox;
        private System.Windows.Forms.CheckBox appendSqlCheckBox;
        private System.Windows.Forms.CheckBox withoutHeaderCheckBox;
        private System.Windows.Forms.GroupBox sqlBuilderGroupBox;
        private System.Windows.Forms.GroupBox parserGroupBox;
        private System.Windows.Forms.CheckBox dataCompressionCheckBox;
        private System.Windows.Forms.GroupBox webRequestGroupBox;

    }
}