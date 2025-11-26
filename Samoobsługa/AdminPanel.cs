// AdminPanel.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace Samoobsługa
{
    public partial class AdminPanel : Form
    {
        private List<Product> products;
        private const string ProductFile = "products.json";

        public AdminPanel(List<Product> products)
        {
            InitializeComponent();
            this.products = products ?? new List<Product>();

            // setup listView (columns are also in Designer but keeping refresh here)
            listViewAdmin.View = View.Details;
            listViewAdmin.FullRowSelect = true;
            listViewAdmin.Columns.Clear();
            listViewAdmin.Columns.Add("Nazwa", 160);
            listViewAdmin.Columns.Add("Kategoria", 100);
            listViewAdmin.Columns.Add("Cena", 70);

            RefreshList();
        }

        private void RefreshList()
        {
            listViewAdmin.Items.Clear();
            foreach (var p in products)
            {
                var lvi = new ListViewItem(p.Name);
                lvi.SubItems.Add(p.Category);
                lvi.SubItems.Add(p.Price.ToString("0.00"));
                lvi.Tag = p;
                listViewAdmin.Items.Add(lvi);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // open simple dialog to add product
            using (var form = new Form())
            {
                form.Text = "Dodaj produkt";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.ClientSize = new System.Drawing.Size(420, 200);

                var lblName = new Label { Text = "Nazwa:", Left = 10, Top = 15, AutoSize = true };
                var txtName = new TextBox { Left = 100, Top = 12, Width = 300 };

                var lblCategory = new Label { Text = "Kategoria:", Left = 10, Top = 50, AutoSize = true };
                var txtCategory = new TextBox { Left = 100, Top = 46, Width = 300 };

                var lblPrice = new Label { Text = "Cena:", Left = 10, Top = 85, AutoSize = true };
                var numPrice = new NumericUpDown { Left = 100, Top = 82, Width = 120, DecimalPlaces = 2, Maximum = 10000, Minimum = 0 };

                var lblUrl = new Label { Text = "URL obrazka:", Left = 10, Top = 120, AutoSize = true };
                var txtUrl = new TextBox { Left = 100, Top = 116, Width = 300 };

                var btnOk = new Button { Text = "Dodaj", Left = 100, Top = 150, DialogResult = DialogResult.OK };
                form.Controls.AddRange(new Control[] { lblName, txtName, lblCategory, txtCategory, lblPrice, numPrice, lblUrl, txtUrl, btnOk });
                form.AcceptButton = btnOk;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Podaj nazwę produktu.");
                        return;
                    }

                    var newP = new Product
                    {
                        Name = txtName.Text.Trim(),
                        Category = txtCategory.Text.Trim(),
                        Price = numPrice.Value,
                        ImageUrl = txtUrl.Text.Trim()
                    };

                    products.Add(newP);
                    SaveAndRefresh();
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listViewAdmin.SelectedItems.Count == 0) return;
            var sel = listViewAdmin.SelectedItems[0];
            var p = sel.Tag as Product;
            if (p == null) return;

            using (var form = new Form())
            {
                form.Text = "Edytuj produkt";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.ClientSize = new System.Drawing.Size(420, 200);

                var lblName = new Label { Text = "Nazwa:", Left = 10, Top = 15, AutoSize = true };
                var txtName = new TextBox { Left = 100, Top = 12, Width = 300, Text = p.Name };

                var lblCategory = new Label { Text = "Kategoria:", Left = 10, Top = 50, AutoSize = true };
                var txtCategory = new TextBox { Left = 100, Top = 46, Width = 300, Text = p.Category };

                var lblPrice = new Label { Text = "Cena:", Left = 10, Top = 85, AutoSize = true };
                var numPrice = new NumericUpDown { Left = 100, Top = 82, Width = 120, DecimalPlaces = 2, Maximum = 10000, Minimum = 0, Value = p.Price };

                var lblUrl = new Label { Text = "URL obrazka:", Left = 10, Top = 120, AutoSize = true };
                var txtUrl = new TextBox { Left = 100, Top = 116, Width = 300, Text = p.ImageUrl };

                var btnOk = new Button { Text = "Zapisz", Left = 100, Top = 150, DialogResult = DialogResult.OK };
                form.Controls.AddRange(new Control[] { lblName, txtName, lblCategory, txtCategory, lblPrice, numPrice, lblUrl, txtUrl, btnOk });
                form.AcceptButton = btnOk;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    p.Name = txtName.Text.Trim();
                    p.Category = txtCategory.Text.Trim();
                    p.Price = numPrice.Value;
                    p.ImageUrl = txtUrl.Text.Trim();
                    SaveAndRefresh();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listViewAdmin.SelectedItems.Count == 0) return;
            var sel = listViewAdmin.SelectedItems[0];
            var p = sel.Tag as Product;
            if (p == null) return;

            var res = MessageBox.Show($"Usunąć produkt '{p.Name}' ?", "Potwierdź", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                products.Remove(p);
                SaveAndRefresh();
            }
        }

        private void SaveAndRefresh()
        {
            try
            {
                string json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ProductFile, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu: " + ex.Message);
            }

            RefreshList();
            // close with OK so Form1 knows to reload (we'll return DialogResult.OK on close button)
        }

        // Close button handler (set DialogResult.OK to signal changes saved)
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
