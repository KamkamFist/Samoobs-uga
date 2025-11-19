using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Samoobsługa
{
    public partial class Form1 : Form
    {
        private decimal totalSum = 0m;

        // koszyk: nazwa produktu -> (cena, ilość)
        private Dictionary<string, (decimal Price, int Quantity)> cart = new Dictionary<string, (decimal, int)>();
        private Dictionary<string, decimal> productPrices = new Dictionary<string, decimal>();

        public Form1()
        {
            InitializeComponent();

            // ustawienia ListView produktów
            listViewProducts.View = View.LargeIcon;
            listViewProducts.LargeImageList = imageList1;
            listViewProducts.SelectedIndexChanged += ListViewProducts_SelectedIndexChanged;

            // ListView paragonowy
            listViewReceipt.View = View.Details;
            listViewReceipt.Columns.Add("Produkt", 150);
            listViewReceipt.Columns.Add("Cena", 70);
            listViewReceipt.Columns.Add("Ilość", 50);
            listViewReceipt.Columns.Add("Wartość", 70);
            listViewReceipt.FullRowSelect = true;
            listViewReceipt.GridLines = true;

            // przycisk usuń
            buttonRemove.Click += ButtonRemove_Click;

            LoadProducts();
        }

        private void LoadProducts()
        {
            imageList1.Images.Clear();
            listViewProducts.Items.Clear();
            listViewReceipt.Items.Clear();
            cart.Clear();
            productPrices.Clear();
            totalSum = 0m;
            sumLabel.Text = "Suma: 0.00 zł";

            var products = new List<(string Name, string Category, decimal Price, string ImageUrl)>
            {
                ("Chleb pszenny", "Pieczywo", 4.20m, "https://pngimg.com/uploads/bread/bread_PNG2271.png"),
                ("Bułka kajzerka", "Pieczywo", 0.70m, "https://pngimg.com/uploads/bread/small/bread_PNG2277.png"),
                ("Chleb żytni", "Pieczywo", 5.80m, "https://pngimg.com/uploads/bread/small/bread_PNG2297.png"),
                ("Bagietka", "Pieczywo", 3.10m, "https://pngimg.com/uploads/baguette/baguette_PNG30.png"),
                ("Czekolada mleczna", "Słodycze", 3.50m, "https://pngimg.com/uploads/chocolate/small/chocolate_PNG97202.png"),
                ("Baton karmelowy", "Słodycze", 2.20m, "https://pngimg.com/uploads/chocolate/chocolate_PNG97193.png"),
                ("Ciastka maślane", "Słodycze", 4.80m, "https://pngimg.com/uploads/cookie/small/cookie_PNG97341.png"),
                ("Wafel czekoladowy", "Słodycze", 1.80m, "https://pngimg.com/uploads/waffle/waffle_PNG23.png"),
                ("Marchew", "Warzywa", 2.70m, "https://pngimg.com/uploads/carrot/carrot_PNG4986.png"),
                ("Pomidor", "Warzywa", 3.90m, "https://pngimg.com/uploads/tomato/tomato_PNG12592.png"),
                ("Cebula", "Warzywa", 1.40m, "https://pngimg.com/uploads/onion/small/onion_PNG3824.png"),
                ("Ogórek", "Warzywa", 2.10m, "https://pngimg.com/uploads/cucumber/small/cucumber_PNG12617.png"),
                ("Jabłko", "Owoce", 1.90m, "https://pngimg.com/uploads/apple/apple_PNG12406.png"),
                ("Banany", "Owoce", 4.40m, "https://pngimg.com/uploads/banana/banana_PNG842.png"),
                ("Winogrona", "Owoce", 7.80m, "https://pngimg.com/uploads/aston_martin/aston_martin_PNG50.png"),
                ("Pomarańcza", "Owoce", 3.10m, "https://pngimg.com/uploads/orange/orange_PNG800.png"),
                ("Bułka słodka", "Pieczywo", 1.50m, "https://pngimg.com/uploads/croissant/small/croissant_PNG46720.png"),
                ("Drożdżówka", "Pieczywo", 2.30m, "https://pngimg.com/uploads/easter_cake/small/easter_cake_PNG14.png"),
                ("Czekoladki", "Słodycze", 6.90m, "https://pngimg.com/uploads/m_m/small/m_m_PNG60.png"),
                ("Sałata", "Warzywa", 3.00m, "https://image.shutterstock.com/image-photo/fresh-ripe-cabbage-vegetable-isolated-260nw-2624180029.jpg")
            };

            WebClient wc = new WebClient();
            int index = 0;

            foreach (var p in products)
            {
                try
                {
                    byte[] bytes = wc.DownloadData(p.ImageUrl);
                    using (var ms = new MemoryStream(bytes))
                    {
                        imageList1.Images.Add(Image.FromStream(ms));
                    }
                }
                catch
                {
                    imageList1.Images.Add(SystemIcons.Warning);
                }

                ListViewItem item = new ListViewItem
                {
                    Text = $"{p.Name}\n{p.Price:0.00} zł",
                    ImageIndex = index
                };

                listViewProducts.Items.Add(item);
                productPrices[p.Name] = p.Price;
                index++;
            }
        }

        private void ListViewProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewProducts.SelectedItems.Count == 0)
                return;

            var item = listViewProducts.SelectedItems[0];
            string name = item.Text.Split('\n')[0];

            if (productPrices.TryGetValue(name, out decimal price))
            {
                if (cart.ContainsKey(name))
                    cart[name] = (price, cart[name].Quantity + 1);
                else
                    cart[name] = (price, 1);

                RefreshReceipt();
            }

            listViewProducts.SelectedItems.Clear();
        }

        private void RefreshReceipt()
        {
            listViewReceipt.Items.Clear();
            totalSum = 0m;

            foreach (var kvp in cart)
            {
                decimal lineTotal = kvp.Value.Price * kvp.Value.Quantity;
                totalSum += lineTotal;

                ListViewItem lvItem = new ListViewItem(kvp.Key);
                lvItem.SubItems.Add(kvp.Value.Price.ToString("0.00"));
                lvItem.SubItems.Add(kvp.Value.Quantity.ToString());
                lvItem.SubItems.Add(lineTotal.ToString("0.00"));

                listViewReceipt.Items.Add(lvItem);
            }

            sumLabel.Text = $"Suma: {totalSum:0.00} zł";
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (listViewReceipt.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewReceipt.SelectedItems[0];
            string name = selectedItem.Text;

            if (cart.ContainsKey(name))
            {
                if (cart[name].Quantity > 1)
                    cart[name] = (cart[name].Price, cart[name].Quantity - 1);
                else
                    cart.Remove(name);
            }

            RefreshReceipt();
        }
    }
}
