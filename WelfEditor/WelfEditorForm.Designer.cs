namespace WelfEditor
{
    partial class WelfEditorForm
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
            this.components = new System.ComponentModel.Container();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.newMenuItem = new System.Windows.Forms.MenuItem();
            this.openMenuItem = new System.Windows.Forms.MenuItem();
            this.saveMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.editMenuItem = new System.Windows.Forms.MenuItem();
            this.convertMenuItem = new System.Windows.Forms.MenuItem();
            this.idListView = new System.Windows.Forms.ListView();
            this.idColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.editGroupBox = new System.Windows.Forms.GroupBox();
            this.IdTextBox = new System.Windows.Forms.TextBox();
            this.totalIdBox = new System.Windows.Forms.NumericUpDown();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.editGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.totalIdBox)).BeginInit();
            this.SuspendLayout();
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
            this.newMenuItem,
            this.openMenuItem,
            this.saveMenuItem,
            this.exitMenuItem});
            this.fileMenuItem.Text = "File";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Index = 0;
            this.newMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.newMenuItem.Text = "New";
            this.newMenuItem.Click += new System.EventHandler(this.NewMenuItemClick);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Index = 1;
            this.openMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Click += new System.EventHandler(this.OpenMenuItemClick);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Index = 2;
            this.saveMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.saveMenuItem.Text = "Save";
            this.saveMenuItem.Click += new System.EventHandler(this.SaveMenuItemClick);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 3;
            this.exitMenuItem.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItemClick);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Index = 1;
            this.editMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.convertMenuItem});
            this.editMenuItem.Text = "Edit";
            // 
            // convertMenuItem
            // 
            this.convertMenuItem.Index = 0;
            this.convertMenuItem.Text = "Convert";
            this.convertMenuItem.Click += new System.EventHandler(this.ConvertMenuItemClick);
            // 
            // idListView
            // 
            this.idListView.BackColor = System.Drawing.Color.AliceBlue;
            this.idListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.idColumnHeader});
            this.idListView.FullRowSelect = true;
            this.idListView.GridLines = true;
            this.idListView.Location = new System.Drawing.Point(12, 12);
            this.idListView.Name = "idListView";
            this.idListView.Size = new System.Drawing.Size(118, 259);
            this.idListView.TabIndex = 0;
            this.idListView.UseCompatibleStateImageBehavior = false;
            this.idListView.View = System.Windows.Forms.View.Details;
            this.idListView.VirtualMode = true;
            this.idListView.CacheVirtualItems += ListCacheVirtualItems;
            this.idListView.RetrieveVirtualItem += ListRetrieveVirtualItem;
            this.idListView.ColumnClick += IdListViewColumnClick;
            // 
            // idColumnHeader
            // 
            this.idColumnHeader.Text = "Id";
            this.idColumnHeader.Width = 114;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(17, 45);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButtonClick);
            // 
            // removeButton
            // 
            this.removeButton.Enabled = false;
            this.removeButton.Location = new System.Drawing.Point(98, 45);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 2;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.RemoveButtonClick);
            // 
            // editGroupBox
            // 
            this.editGroupBox.Controls.Add(this.IdTextBox);
            this.editGroupBox.Controls.Add(this.addButton);
            this.editGroupBox.Controls.Add(this.removeButton);
            this.editGroupBox.Location = new System.Drawing.Point(136, 12);
            this.editGroupBox.Name = "editGroupBox";
            this.editGroupBox.Size = new System.Drawing.Size(203, 80);
            this.editGroupBox.TabIndex = 3;
            this.editGroupBox.TabStop = false;
            this.editGroupBox.Text = "Edit";
            // 
            // IdTextBox
            // 
            this.IdTextBox.Location = new System.Drawing.Point(17, 19);
            this.IdTextBox.Name = "IdTextBox";
            this.IdTextBox.Size = new System.Drawing.Size(156, 20);
            this.IdTextBox.TabIndex = 3;
            this.IdTextBox.Text = "0";
            // 
            // totalIdBox
            // 
            this.totalIdBox.Enabled = false;
            this.totalIdBox.Location = new System.Drawing.Point(219, 98);
            this.totalIdBox.Name = "totalIdBox";
            this.totalIdBox.Size = new System.Drawing.Size(120, 20);
            this.totalIdBox.TabIndex = 3;
            this.totalIdBox.Maximum = int.MaxValue;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Welf File| *.welf|All Files| *.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Welf File| *.welf|All Files| *.*";
            // 
            // WelfEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(355, 279);
            this.Controls.Add(this.totalIdBox);
            this.Controls.Add(this.editGroupBox);
            this.Controls.Add(this.idListView);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu;
            this.Name = "WelfEditorForm";
            this.Text = "Welf Editor";
            this.editGroupBox.ResumeLayout(false);
            this.editGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.totalIdBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem fileMenuItem;
        private System.Windows.Forms.MenuItem newMenuItem;
        private System.Windows.Forms.MenuItem openMenuItem;
        private System.Windows.Forms.MenuItem saveMenuItem;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.ListView idListView;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.GroupBox editGroupBox;
        private System.Windows.Forms.NumericUpDown totalIdBox;
        private System.Windows.Forms.TextBox IdTextBox;
        private System.Windows.Forms.ColumnHeader idColumnHeader;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.MenuItem editMenuItem;
        private System.Windows.Forms.MenuItem convertMenuItem;
    }
}

