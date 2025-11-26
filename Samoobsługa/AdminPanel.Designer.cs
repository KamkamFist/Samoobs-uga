// AdminPanel.Designer.cs
namespace Samoobsługa
{
    partial class AdminPanel
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView listViewAdmin;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listViewAdmin = new System.Windows.Forms.ListView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewAdmin
            // 
            this.listViewAdmin.Location = new System.Drawing.Point(12, 12);
            this.listViewAdmin.Name = "listViewAdmin";
            this.listViewAdmin.Size = new System.Drawing.Size(520, 360);
            this.listViewAdmin.TabIndex = 0;
            this.listViewAdmin.UseCompatibleStateImageBehavior = false;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(12, 380);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(120, 30);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Dodaj";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(138, 380);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(140, 30);
            this.buttonEdit.TabIndex = 2;
            this.buttonEdit.Text = "Edytuj";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(284, 380);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(120, 30);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "Usuń";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(414, 380);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(118, 30);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Zamknij (Zapisz)";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // AdminPanel
            // 
            this.ClientSize = new System.Drawing.Size(544, 422);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listViewAdmin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AdminPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Panel administratora";
            this.ResumeLayout(false);
        }
    }
}
