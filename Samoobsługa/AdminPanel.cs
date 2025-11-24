using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Samoobsługa
{
    public partial class AdminPanel : Form
    {
        private List<Product> products;

        public AdminPanel(List<Product> products)
        {
            InitializeComponent();
            this.products = products;

            // ustawienia listView
            listViewAdmin.View = View.Details;
            listViewAdmin.FullRowSelect = true;
            listViewAdmin.Columns.Add("Nazwa", 150);
            listViewAdmin.Columns.Add("Kategoria", 100);
            listViewAdmin.Columns.Add("Cena", 70);

            RefreshListView();

            buttonAdd.Click += ButtonAdd_Click;
            buttonRemove.Click += ButtonRemove_Click;
        }

        private void RefreshListView()
        {
            listViewAdmin.Items.Clear();
            foreach (var p in products)
            {
                ListViewItem item = new ListViewItem(p.Name);
                item.SubItems.Add(p.Category);
                item.SubItems.Add(p.Price.ToString("0.00"));
                listViewAdmin.Items.Add(item);
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            // proste okienko do dodania nowego produktu
            using (Form addForm = new Form())
            {
                addForm.Text = "Dodaj produkt";
                addForm.Width = 300;
                addForm.Height = 220;

                Label nameLabel = new Label() { Text = "Nazwa:", Left = 10, Top = 20, Width = 80 };
                TextBox nameBox = new TextBox() { Left = 100, Top = 20, Width = 150 };

                Label categoryLabel = new Label() { Text = "Kategoria:", Left = 10, Top = 60, Width = 80 };
                TextBox categoryBox = new TextBox() { Left = 100, Top = 60, Width = 150 };

                Label priceLabel = new Label() { Text = "Cena:", Left = 10, Top = 100, Width = 80 };
                TextBox priceBox = new TextBox() { Left = 100, Top = 100, Width = 150 };

                Label urlLabel = new Label() { Text = "URL obrazu:", Left = 10, Top = 140, Width = 80 };
                TextBox urlBox = new TextBox() { Left = 100, Top = 140, Width = 150 };

                Button okButton = new Button() { Text = "Dodaj", Left = 100, Top = 170, Width = 80 };
                okButton.DialogResult = DialogResult.OK;

                addForm.Controls.Add(nameLabel);
                addForm.Controls.Add(nameBox);
                addForm.Controls.Add(categoryLabel);
                addForm.Controls.Add(categoryBox);
                addForm.Controls.Add(priceLabel);
                addForm.Controls.Add(priceBox);
                addForm.Controls.Add(urlLabel);
                addForm.Controls.Add(urlBox);
                addForm.Controls.Add(okButton);

                addForm.AcceptButton = okButton;

                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    if (decimal.TryParse(priceBox.Text, out decimal price))
                    {
                        Product newProduct = new Product()
                        {
                            Name = nameBox.Text,
                            Category = categoryBox.Text,
                            Price = price,
                            ImageUrl = urlBox.Text
                        };
                        products.Add(newProduct);
                        RefreshListView();
                    }
                    else
                    {
                        MessageBox.Show("Niepoprawna cena!");
                    }
                }
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (listViewAdmin.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewAdmin.SelectedItems[0];
            string name = selectedItem.Text;

            var productToRemove = products.Find(p => p.Name == name);
            if (productToRemove != null)
            {
                products.Remove(productToRemove);
                RefreshListView();
            }
        }
    }
}
