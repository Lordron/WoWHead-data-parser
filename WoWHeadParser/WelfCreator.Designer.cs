namespace WoWHeadParser
{
    partial class WelfCreator
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
            System.Windows.Forms.Label addressLabel;
            this.addressBox = new System.Windows.Forms.TextBox();
            this.welDataBox = new System.Windows.Forms.RichTextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.numericUpDown = new System.Windows.Forms.NumericUpDown();
            addressLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // addressLabel
            // 
            addressLabel.AutoSize = true;
            addressLabel.Location = new System.Drawing.Point(7, 13);
            addressLabel.Name = "addressLabel";
            addressLabel.Size = new System.Drawing.Size(74, 13);
            addressLabel.TabIndex = 1;
            addressLabel.Text = "Web Address:";
            // 
            // addressBox
            // 
            this.addressBox.BackColor = System.Drawing.SystemColors.Control;
            this.addressBox.Location = new System.Drawing.Point(87, 10);
            this.addressBox.Name = "addressBox";
            this.addressBox.Size = new System.Drawing.Size(303, 20);
            this.addressBox.TabIndex = 0;
            this.addressBox.TextChanged += new System.EventHandler(this.AddressBoxTextChanged);
            // 
            // welDataBox
            // 
            this.welDataBox.BackColor = System.Drawing.SystemColors.Control;
            this.welDataBox.Location = new System.Drawing.Point(10, 40);
            this.welDataBox.Name = "welDataBox";
            this.welDataBox.Size = new System.Drawing.Size(380, 108);
            this.welDataBox.TabIndex = 2;
            this.welDataBox.Text = "";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(10, 154);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(95, 154);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // saveDialog
            // 
            this.saveDialog.Filter = "WoWHead Entry List File (.welf)| *.welf|All Files|*.*";
            // 
            // numericUpDown
            // 
            this.numericUpDown.Enabled = false;
            this.numericUpDown.Location = new System.Drawing.Point(270, 157);
            this.numericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            this.numericUpDown.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown.TabIndex = 6;
            // 
            // WelfCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(402, 188);
            this.Controls.Add(this.numericUpDown);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.welDataBox);
            this.Controls.Add(addressLabel);
            this.Controls.Add(this.addressBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "WelfCreator";
            this.Text = "Welf Creator";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox addressBox;
        private System.Windows.Forms.RichTextBox welDataBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.NumericUpDown numericUpDown;
    }
}