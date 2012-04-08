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
            this.allowNullValCheckBox = new System.Windows.Forms.CheckBox();
            this.appendDeleteQueryCheckBox = new System.Windows.Forms.CheckBox();
            this.quertTypelabel = new System.Windows.Forms.Label();
            this.sqlQueryTypeBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancleButton = new System.Windows.Forms.Button();
            this.sortAscendingButton = new System.Windows.Forms.RadioButton();
            this.sortDescendingButton = new System.Windows.Forms.RadioButton();
            this.sortGroupBox = new System.Windows.Forms.GroupBox();
            this.sortGroupBox.SuspendLayout();
            this.SuspendLayout();
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
            // quertTypelabel
            // 
            this.quertTypelabel.AutoSize = true;
            this.quertTypelabel.Location = new System.Drawing.Point(10, 157);
            this.quertTypelabel.Name = "quertTypelabel";
            this.quertTypelabel.Size = new System.Drawing.Size(76, 13);
            this.quertTypelabel.TabIndex = 2;
            this.quertTypelabel.Text = "Sql Query type";
            // 
            // sqlQueryTypeBox
            // 
            this.sqlQueryTypeBox.FormattingEnabled = true;
            this.sqlQueryTypeBox.Location = new System.Drawing.Point(92, 154);
            this.sqlQueryTypeBox.Name = "sqlQueryTypeBox";
            this.sqlQueryTypeBox.Size = new System.Drawing.Size(113, 21);
            this.sqlQueryTypeBox.TabIndex = 3;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(11, 190);
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
            this.cancleButton.Location = new System.Drawing.Point(130, 190);
            this.cancleButton.Name = "cancleButton";
            this.cancleButton.Size = new System.Drawing.Size(75, 23);
            this.cancleButton.TabIndex = 5;
            this.cancleButton.Text = "Cancle";
            this.cancleButton.UseVisualStyleBackColor = true;
            this.cancleButton.Click += new System.EventHandler(this.CancleButtonClick);
            // 
            // sortAscendingButton
            // 
            this.sortAscendingButton.AutoSize = true;
            this.sortAscendingButton.Checked = !Settings.Default.SortDown;
            this.sortAscendingButton.Location = new System.Drawing.Point(6, 19);
            this.sortAscendingButton.Name = "sortAscendingButton";
            this.sortAscendingButton.Size = new System.Drawing.Size(75, 17);
            this.sortAscendingButton.TabIndex = 6;
            this.sortAscendingButton.TabStop = true;
            this.sortAscendingButton.Text = "Ascending";
            this.sortAscendingButton.UseVisualStyleBackColor = true;
            // 
            // sortDescendingButton
            // 
            this.sortDescendingButton.AutoSize = true;
            this.sortDescendingButton.Checked = Settings.Default.SortDown;
            this.sortDescendingButton.Location = new System.Drawing.Point(6, 42);
            this.sortDescendingButton.Name = "sortDescendingButton";
            this.sortDescendingButton.Size = new System.Drawing.Size(82, 17);
            this.sortDescendingButton.TabIndex = 7;
            this.sortDescendingButton.TabStop = true;
            this.sortDescendingButton.Text = "Descending";
            this.sortDescendingButton.UseVisualStyleBackColor = true;
            // 
            // sortGroupBox
            // 
            this.sortGroupBox.Controls.Add(this.sortAscendingButton);
            this.sortGroupBox.Controls.Add(this.sortDescendingButton);
            this.sortGroupBox.Location = new System.Drawing.Point(13, 60);
            this.sortGroupBox.Name = "sortGroupBox";
            this.sortGroupBox.Size = new System.Drawing.Size(192, 73);
            this.sortGroupBox.TabIndex = 8;
            this.sortGroupBox.TabStop = false;
            this.sortGroupBox.Text = "Sort";
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancleButton;
            this.ClientSize = new System.Drawing.Size(220, 230);
            this.Controls.Add(this.sortGroupBox);
            this.Controls.Add(this.cancleButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.sqlQueryTypeBox);
            this.Controls.Add(this.quertTypelabel);
            this.Controls.Add(this.appendDeleteQueryCheckBox);
            this.Controls.Add(this.allowNullValCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.sortGroupBox.ResumeLayout(false);
            this.sortGroupBox.PerformLayout();
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
        private System.Windows.Forms.RadioButton sortAscendingButton;
        private System.Windows.Forms.RadioButton sortDescendingButton;
        private System.Windows.Forms.GroupBox sortGroupBox;

    }
}