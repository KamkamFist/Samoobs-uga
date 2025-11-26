using System;
using System.Windows.Forms;
using System.IO;

namespace Samoobsługa
{
    public partial class PaymentForm : Form
    {
        private Form1 mainForm;

        public PaymentForm(Form1 form)
        {
            InitializeComponent();
            mainForm = form;
            Initialize();
        }

        private void Initialize()
        {
            this.radioCard = new RadioButton();
            this.radioCash = new RadioButton();
            this.buttonConfirm = new Button();
            this.SuspendLayout();

            // radioCard
            this.radioCard.AutoSize = true;
            this.radioCard.Location = new System.Drawing.Point(30, 30);
            this.radioCard.Text = "Karta płatnicza";

            // radioCash
            this.radioCash.AutoSize = true;
            this.radioCash.Location = new System.Drawing.Point(30, 60);
            this.radioCash.Text = "Gotówka";

            // buttonConfirm
            this.buttonConfirm.Location = new System.Drawing.Point(30, 100);
            this.buttonConfirm.Size = new System.Drawing.Size(150, 30);
            this.buttonConfirm.Text = "Zapłać";
            this.buttonConfirm.Click += ButtonConfirm_Click;

            // PaymentForm
            this.ClientSize = new System.Drawing.Size(250, 160);
            this.Controls.Add(this.radioCard);
            this.Controls.Add(this.radioCash);
            this.Controls.Add(this.buttonConfirm);
            this.Text = "Płatność";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private RadioButton radioCard;
        private RadioButton radioCash;
        private Button buttonConfirm;

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            if (!radioCard.Checked && !radioCash.Checked)
            {
                MessageBox.Show("Wybierz metodę płatności.");
                return;
            }

            // Zapisz paragon do pliku
            mainForm.SaveReceiptToFile();

            // Wyczyść koszyk i zaktualizuj listę
            mainForm.cart.Clear();
            mainForm.RefreshReceipt();

            MessageBox.Show("Płatność zakończona, paragon zapisany do pliku.");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
