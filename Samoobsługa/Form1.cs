using System;
using System.Collections.Generic;
using System.Drawing;
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

        // lista produktów dla AdminPanel i ListView
        private List<Product> products;

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

            // przycisk panel admina
            buttonPanelAdmin.Click += ButtonPanelAdmin_Click;

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

            // lista produktów
            products = new List<Product>
            {
                new Product { Name = "Chleb pszenny", Category = "Pieczywo", Price = 4.20m, ImageUrl = "https://pngimg.com/uploads/bread/bread_PNG2271.png" },
                new Product { Name = "Bułka kajzerka", Category = "Pieczywo", Price = 0.70m, ImageUrl = "https://pngimg.com/uploads/bread/small/bread_PNG2277.png" },
                new Product { Name = "Chleb żytni", Category = "Pieczywo", Price = 5.80m, ImageUrl = "https://pngimg.com/uploads/bread/small/bread_PNG2297.png" },
                new Product { Name = "Bagietka", Category = "Pieczywo", Price = 3.10m, ImageUrl = "https://pngimg.com/uploads/baguette/baguette_PNG30.png" },
                new Product { Name = "Czekolada mleczna", Category = "Słodycze", Price = 3.50m, ImageUrl = "https://pngimg.com/uploads/chocolate/small/chocolate_PNG97202.png" },
                new Product { Name = "Baton karmelowy", Category = "Słodycze", Price = 2.20m, ImageUrl = "https://pngimg.com/uploads/chocolate/chocolate_PNG97193.png" },
                new Product { Name = "Ciastka maślane", Category = "Słodycze", Price = 4.80m, ImageUrl = "https://pngimg.com/uploads/cookie/small/cookie_PNG97341.png" },
                new Product { Name = "Wafel czekoladowy", Category = "Słodycze", Price = 1.80m, ImageUrl = "https://pngimg.com/uploads/waffle/waffle_PNG23.png" },
                new Product { Name = "Marchew", Category = "Warzywa", Price = 2.70m, ImageUrl = "https://pngimg.com/uploads/carrot/carrot_PNG4986.png" },
                new Product { Name = "Pomidor", Category = "Warzywa", Price = 3.90m, ImageUrl = "https://pngimg.com/uploads/tomato/tomato_PNG12592.png" },
                new Product { Name = "Cebula", Category = "Warzywa", Price = 1.40m, ImageUrl = "https://pngimg.com/uploads/onion/small/onion_PNG3824.png" },
                new Product { Name = "Ogórek", Category = "Warzywa", Price = 2.10m, ImageUrl = "https://pngimg.com/uploads/cucumber/small/cucumber_PNG12617.png" },
                new Product { Name = "Jabłko", Category = "Owoce", Price = 1.90m, ImageUrl = "https://pngimg.com/uploads/apple/apple_PNG12406.png" },
                new Product { Name = "Banany", Category = "Owoce", Price = 4.40m, ImageUrl = "https://pngimg.com/uploads/banana/banana_PNG842.png" },
                new Product { Name = "Winogrona", Category = "Owoce", Price = 7.80m, ImageUrl = "https://pngimg.com/uploads/aston_martin/aston_martin_PNG50.png" },
                new Product { Name = "Pomarańcza", Category = "Owoce", Price = 3.10m, ImageUrl = "https://pngimg.com/uploads/orange/orange_PNG800.png" },
                new Product { Name = "Bułka słodka", Category = "Pieczywo", Price = 1.50m, ImageUrl = "https://pngimg.com/uploads/croissant/small/croissant_PNG46720.png" },
                new Product { Name = "Drożdżówka", Category = "Pieczywo", Price = 2.30m, ImageUrl = "https://pngimg.com/uploads/easter_cake/small/easter_cake_PNG14.png" },
                new Product { Name = "Czekoladki", Category = "Słodycze", Price = 6.90m, ImageUrl = "https://pngimg.com/uploads/m_m/small/m_m_PNG60.png" },
                new Product { Name = "Sałata", Category = "Warzywa", Price = 3.00m, ImageUrl = "https://image.shutterstock.com/image-photo/fresh-ripe-cabbage-vegetable-isolated-260nw-2624180029.jpg" }
            };

            WebClient wc = new WebClient();
            int index = 0;

            foreach (var p in products)
            {
                try
                {
                    byte[] bytes = wc.DownloadData(p.ImageUrl);
                    using (var ms = new System.IO.MemoryStream(bytes))
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
            if (listViewProducts.SelectedItems.Count == 0) return;

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
            if (listViewReceipt.SelectedItems.Count == 0) return;

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

        private void ButtonPanelAdmin_Click(object sender, EventArgs e)
        {
            AdminPanel ap = new AdminPanel(products); // przekazujemy listę produktów
            ap.Show();
        }

        
        }

        
    }



    // klasa Product
    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
