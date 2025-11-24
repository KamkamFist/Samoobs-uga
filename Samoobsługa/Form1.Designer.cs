namespace Samoobsługa
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            listViewProducts = new ListView();
            imageList1 = new ImageList(components);
            Checkout = new Panel();
            buttonPanelAdmin = new Button();
            buttonRemove = new Button();
            listViewReceipt = new ListView();
            sumLabel = new Label();
            ZaplacButton = new Button();
            Znizka = new Label();
            listView1 = new ListView();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            ZaloguhButton = new Button();
            EnterNameTextbox = new TextBox();
            Checkout.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // listViewProducts
            // 
            listViewProducts.LargeImageList = imageList1;
            listViewProducts.Location = new Point(3, 48);
            listViewProducts.Name = "listViewProducts";
            listViewProducts.Size = new Size(792, 399);
            listViewProducts.TabIndex = 2;
            listViewProducts.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(64, 64);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // Checkout
            // 
            Checkout.Controls.Add(buttonPanelAdmin);
            Checkout.Controls.Add(buttonRemove);
            Checkout.Controls.Add(listViewReceipt);
            Checkout.Controls.Add(sumLabel);
            Checkout.Controls.Add(ZaplacButton);
            Checkout.Controls.Add(Znizka);
            Checkout.Controls.Add(listView1);
            Checkout.Location = new Point(801, 48);
            Checkout.Name = "Checkout";
            Checkout.Size = new Size(353, 399);
            Checkout.TabIndex = 1;
            // 
            // buttonPanelAdmin
            // 
            buttonPanelAdmin.Location = new Point(143, 347);
            buttonPanelAdmin.Name = "buttonPanelAdmin";
            buttonPanelAdmin.Size = new Size(100, 23);
            buttonPanelAdmin.TabIndex = 6;
            buttonPanelAdmin.Text = "admin UwU \U0001f979";
            buttonPanelAdmin.UseVisualStyleBackColor = true;
            // 
            // buttonRemove
            // 
            buttonRemove.Location = new Point(249, 347);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(95, 23);
            buttonRemove.TabIndex = 5;
            buttonRemove.Text = "Usuń produkt";
            buttonRemove.UseVisualStyleBackColor = true;
            // 
            // listViewReceipt
            // 
            listViewReceipt.Location = new Point(0, 0);
            listViewReceipt.Name = "listViewReceipt";
            listViewReceipt.Size = new Size(344, 335);
            listViewReceipt.TabIndex = 4;
            listViewReceipt.UseCompatibleStateImageBehavior = false;
            // 
            // sumLabel
            // 
            sumLabel.AutoSize = true;
            sumLabel.Location = new Point(3, 358);
            sumLabel.Name = "sumLabel";
            sumLabel.Size = new Size(68, 15);
            sumLabel.TabIndex = 3;
            sumLabel.Text = "Do zapłaty: ";
            // 
            // ZaplacButton
            // 
            ZaplacButton.Dock = DockStyle.Bottom;
            ZaplacButton.Location = new Point(0, 376);
            ZaplacButton.Name = "ZaplacButton";
            ZaplacButton.Size = new Size(353, 23);
            ZaplacButton.TabIndex = 1;
            ZaplacButton.Text = "Zaplac ";
            ZaplacButton.UseVisualStyleBackColor = true;
            // 
            // Znizka
            // 
            Znizka.AutoSize = true;
            Znizka.Location = new Point(3, 338);
            Znizka.MinimumSize = new Size(100, 0);
            Znizka.Name = "Znizka";
            Znizka.Size = new Size(100, 15);
            Znizka.TabIndex = 2;
            Znizka.Text = "Znizka: 30%";
            // 
            // listView1
            // 
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listView1.Location = new Point(29, 17);
            listView1.Name = "listView1";
            listView1.Size = new Size(93, 97);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68.97148F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31.0285225F));
            tableLayoutPanel1.Controls.Add(Checkout, 1, 1);
            tableLayoutPanel1.Controls.Add(listViewProducts, 0, 1);
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            tableLayoutPanel1.Size = new Size(1157, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(ZaloguhButton);
            panel2.Controls.Add(EnterNameTextbox);
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(792, 39);
            panel2.TabIndex = 3;
            // 
            // ZaloguhButton
            // 
            ZaloguhButton.Location = new Point(169, 2);
            ZaloguhButton.Name = "ZaloguhButton";
            ZaloguhButton.Size = new Size(75, 23);
            ZaloguhButton.TabIndex = 1;
            ZaloguhButton.Text = "Zaloguj";
            ZaloguhButton.UseVisualStyleBackColor = true;
            // 
            // EnterNameTextbox
            // 
            EnterNameTextbox.Location = new Point(3, 3);
            EnterNameTextbox.Name = "EnterNameTextbox";
            EnterNameTextbox.Size = new Size(160, 23);
            EnterNameTextbox.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1157, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "Form1";
            Checkout.ResumeLayout(false);
            Checkout.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListView listViewProducts;
        private Panel Checkout;
        private Button ZaplacButton;
        private Label Znizka;
        private ListView listView1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private Button ZaloguhButton;
        private TextBox EnterNameTextbox;
        private ImageList imageList1;
        private Label sumLabel;
        private ListView listViewReceipt;
        private Button buttonRemove;
        private Button buttonPanelAdmin;
    }
}
