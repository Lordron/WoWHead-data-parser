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
            this.SuspendLayout();
            // 
            // quertTypelabel
            // 
            this.quertTypelabel.AutoSize = true;
            this.quertTypelabel.Location = new System.Drawing.Point(10, 147);
            this.quertTypelabel.Name = "quertTypelabel";
            this.quertTypelabel.Size = new System.Drawing.Size(76, 13);
            this.quertTypelabel.TabIndex = 2;
            this.quertTypelabel.Text = "Sql Query type";
            // 
            // sqlQueryTypeBox
            // 
            this.sqlQueryTypeBox.FormattingEnabled = true;
            this.sqlQueryTypeBox.Location = new System.Drawing.Point(95, 144);
            this.sqlQueryTypeBox.Name = "sqlQueryTypeBox";
            this.sqlQueryTypeBox.Size = new System.Drawing.Size(113, 21);
            this.sqlQueryTypeBox.TabIndex = 3;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(13, 171);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // cancleButton
            // 
            this.cancleButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancleButton.Location = new System.Drawing.Point(133, 171);
            this.cancleButton.Name = "cancleButton";
            this.cancleButton.Size = new System.Drawing.Size(75, 23);
            this.cancleButton.TabIndex = 5;
            this.cancleButton.Text = "Cancle";
            this.cancleButton.UseVisualStyleBackColor = true;
            this.cancleButton.Click += new System.EventHandler(this.CancleButtonClick);
            // 
            // dirLabel
            // 
            this.dirLabel.AutoSize = true;
            this.dirLabel.Location = new System.Drawing.Point(10, 120);
            this.dirLabel.Name = "dirLabel";
            this.dirLabel.Size = new System.Drawing.Size(69, 13);
            this.dirLabel.TabIndex = 6;
            this.dirLabel.Text = "Sort direction";
            // 
            // sortDirectionBox
            // 
            this.sortDirectionBox.FormattingEnabled = true;
            this.sortDirectionBox.Location = new System.Drawing.Point(95, 117);
            this.sortDirectionBox.Name = "sortDirectionBox";
            this.sortDirectionBox.Size = new System.Drawing.Size(113, 21);
            this.sortDirectionBox.TabIndex = 7;
            // 
            // appendSqlCheckBox
            // 
            this.appendSqlCheckBox.AutoSize = true;
            this.appendSqlCheckBox.Checked = global::WoWHeadParser.Properties.Settings.Default.Append;
            this.appendSqlCheckBox.Location = new System.Drawing.Point(13, 60);
            this.appendSqlCheckBox.Name = "appendSqlCheckBox";
            this.appendSqlCheckBox.Size = new System.Drawing.Size(136, 17);
            this.appendSqlCheckBox.TabIndex = 8;
            this.appendSqlCheckBox.Text = "Append sql query to file";
            this.appendSqlCheckBox.UseVisualStyleBackColor = true;
            // 
            // appendDeleteQueryCheckBox
            // 
            this.appendDeleteQueryCheckBox.AutoSize = true;
            this.appendDeleteQueryCheckBox.Checked = global::WoWHeadParser.Properties.Settings.Default.AppendDeleteQuery;
            this.appendDeleteQueryCheckBox.Location = new System.Drawing.Point(13, 37);
            this.appendDeleteQueryCheckBox.Name = "appendDeleteQueryCheckBox";
            this.appendDeleteQueryCheckBox.Size = new System.Drawing.Size(124, 17);
            this.appendDeleteQueryCheckBox.TabIndex = 1;
            this.appendDeleteQueryCheckBox.Text = "Append delete query";
            this.appendDeleteQueryCheckBox.UseVisualStyleBackColor = true;
            // 
            // allowNullValCheckBox
            // 
            this.allowNullValCheckBox.AutoSize = true;
            this.allowNullValCheckBox.Checked = global::WoWHeadParser.Properties.Settings.Default.AllowEmptyValues;
            this.allowNullValCheckBox.Location = new System.Drawing.Point(13, 13);
            this.allowNullValCheckBox.Name = "allowNullValCheckBox";
            this.allowNullValCheckBox.Size = new System.Drawing.Size(104, 17);
            this.allowNullValCheckBox.TabIndex = 0;
            this.allowNullValCheckBox.Text = "Allow null values";
            this.allowNullValCheckBox.UseVisualStyleBackColor = true;
            // 
            // withoutHeaderCheckBox
            // 
            this.withoutHeaderCheckBox.AutoSize = true;
            this.withoutHeaderCheckBox.Checked = global::WoWHeadParser.Properties.Settings.Default.WithoutHeader;
            this.withoutHeaderCheckBox.Location = new System.Drawing.Point(13, 83);
            this.withoutHeaderCheckBox.Name = "withoutHeaderCheckBox";
            this.withoutHeaderCheckBox.Size = new System.Drawing.Size(173, 17);
            this.withoutHeaderCheckBox.TabIndex = 9;
            this.withoutHeaderCheckBox.Text = "Without header (insert\\replace)";
            this.withoutHeaderCheckBox.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancleButton;
            this.ClientSize = new System.Drawing.Size(224, 206);
            this.Controls.Add(this.withoutHeaderCheckBox);
            this.Controls.Add(this.appendSqlCheckBox);
            this.Controls.Add(this.sortDirectionBox);
            this.Controls.Add(this.dirLabel);
            this.Controls.Add(this.cancleButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.sqlQueryTypeBox);
            this.Controls.Add(this.quertTypelabel);
            this.Controls.Add(this.appendDeleteQueryCheckBox);
            this.Controls.Add(this.allowNullValCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

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

    }
}