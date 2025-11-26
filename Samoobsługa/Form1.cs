// Form1.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;

namespace Samoobsługa
{
    public partial class Form1 : Form
    {
        private const string ProductFile = "products.json";

        private List<Product> products = new List<Product>();
        public Dictionary<string, (decimal Price, int Quantity)> cart = new Dictionary<string, (decimal, int)>();
        public Dictionary<string, decimal> productPrices = new Dictionary<string, decimal>();
        private decimal totalSum = 0m;

        public Form1()
        {
            InitializeComponent();

            // ensure ImageList size (optional, can be set in Designer)
            imageList1.ImageSize = new Size(96, 96);

            // configure list views
            listViewProducts.View = View.LargeIcon;
            listViewProducts.LargeImageList = imageList1;
            listViewProducts.MultiSelect = false;
            listViewProducts.SelectedIndexChanged += ListViewProducts_SelectedIndexChanged;

            listViewReceipt.View = View.Details;
            listViewReceipt.Columns.Clear();
            listViewReceipt.Columns.Add("Produkt", 160);
            listViewReceipt.Columns.Add("Cena", 70);
            listViewReceipt.Columns.Add("Ilość", 60);
            listViewReceipt.Columns.Add("Wartość", 80);
            listViewReceipt.FullRowSelect = true;
            listViewReceipt.GridLines = true;

            // events
            buttonRemove.Click += ButtonRemove_Click;
            buttonPanelAdmin.Click += ButtonPanelAdmin_Click;

            // load
            LoadProductsFromFile();
            RefreshProductCatalog();
        }

        // --- FILE IO: load / save products.json ---

        private void LoadProductsFromFile()
        {
            try
            {
                if (!File.Exists(ProductFile))
                {
                    products = GetDefaultProducts();
                    SaveProductsToFile();
                    return;
                }

                string json = File.ReadAllText(ProductFile);
                var read = JsonSerializer.Deserialize<List<Product>>(json);
                products = read ?? GetDefaultProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas wczytywania products.json:\n" + ex.Message);
                products = GetDefaultProducts();
            }
        }

        private void SaveProductsToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ProductFile, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisu products.json:\n" + ex.Message);
            }
        }

        private List<Product> GetDefaultProducts()
        {
            return new List<Product>
            {
                new Product { Name="Chleb pszenny", Category="Pieczywo", Price=4.20m, ImageUrl="https://pngimg.com/uploads/bread/bread_PNG2271.png" },
                new Product { Name="Bułka kajzerka", Category="Pieczywo", Price=0.70m, ImageUrl="https://pngimg.com/uploads/bread/small/bread_PNG2277.png" },
                new Product { Name="Chleb żytni", Category="Pieczywo", Price=5.80m, ImageUrl="https://pngimg.com/uploads/bread/small/bread_PNG2297.png" },
                new Product { Name="Bagietka", Category="Pieczywo", Price=3.10m, ImageUrl="https://pngimg.com/uploads/baguette/baguette_PNG30.png" },
                new Product { Name="Czekolada mleczna", Category="Słodycze", Price=3.50m, ImageUrl="https://pngimg.com/uploads/chocolate/small/chocolate_PNG97202.png" },
                new Product { Name="Baton karmelowy", Category="Słodycze", Price=2.20m, ImageUrl="https://pngimg.com/uploads/chocolate/chocolate_PNG97193.png" },
                new Product { Name="Ciastka maślane", Category="Słodycze", Price=4.80m, ImageUrl="https://pngimg.com/uploads/cookie/small/cookie_PNG97341.png" },
                new Product { Name="Wafel czekoladowy", Category="Słodycze", Price=1.80m, ImageUrl="https://pngimg.com/uploads/waffle/waffle_PNG23.png" },
                new Product { Name="Marchew", Category="Warzywa", Price=2.70m, ImageUrl="https://pngimg.com/uploads/carrot/carrot_PNG4986.png" },
                new Product { Name="Pomidor", Category="Warzywa", Price=3.90m, ImageUrl="https://pngimg.com/uploads/tomato/tomato_PNG12592.png" },
                new Product { Name="Cebula", Category="Warzywa", Price=1.40m, ImageUrl="https://pngimg.com/uploads/onion/small/onion_PNG3824.png" },
                new Product { Name="Ogórek", Category="Warzywa", Price=2.10m, ImageUrl="https://pngimg.com/uploads/cucumber/small/cucumber_PNG12617.png" },
                new Product { Name="Jabłko", Category="Owoce", Price=1.90m, ImageUrl="https://pngimg.com/uploads/apple/apple_PNG12406.png" },
                new Product { Name="Banany", Category="Owoce", Price=4.40m, ImageUrl="https://pngimg.com/uploads/banana/banana_PNG842.png" },
                new Product { Name="Winogrona", Category="Owoce", Price=7.80m, ImageUrl="https://pngimg.com/uploads/aston_martin/aston_martin_PNG50.png" },
                new Product { Name="Pomarańcza", Category="Owoce", Price=3.10m, ImageUrl="https://pngimg.com/uploads/orange/orange_PNG800.png" },
                new Product { Name="Bułka słodka", Category="Pieczywo", Price=1.50m, ImageUrl="https://pngimg.com/uploads/croissant/small/croissant_PNG46720.png" },
                new Product { Name="Drożdżówka", Category="Pieczywo", Price=2.30m, ImageUrl="https://pngimg.com/uploads/easter_cake/small/easter_cake_PNG14.png" },
                new Product { Name="Czekoladki", Category="Słodycze", Price=6.90m, ImageUrl="https://pngimg.com/uploads/m_m/small/m_m_PNG60.png" },
                new Product { Name="Sałata", Category="Warzywa", Price=3.00m, ImageUrl="https://image.shutterstock.com/image-photo/fresh-ripe-cabbage-vegetable-isolated-260nw-2624180029.jpg" }
            };
        }

        // --- RefreshProductCatalog: build imageList + product listview ---
        public void RefreshProductCatalog()
        {
            listViewProducts.Items.Clear();
            imageList1.Images.Clear();
            productPrices.Clear();

            using (WebClient wc = new WebClient())
            {
                int index = 0;
                foreach (var p in products)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(p.ImageUrl))
                        {
                            byte[] bytes = wc.DownloadData(p.ImageUrl);
                            using (var ms = new MemoryStream(bytes))
                            {
                                imageList1.Images.Add(Image.FromStream(ms));
                            }
                        }
                        else
                        {
                            imageList1.Images.Add(SystemIcons.Warning.ToBitmap());
                        }
                    }
                    catch
                    {
                        imageList1.Images.Add(SystemIcons.Warning.ToBitmap());
                    }

                    var lvi = new ListViewItem(p.Name) { ImageIndex = index };
                    lvi.SubItems.Add(p.Price.ToString("0.00"));
                    lvi.SubItems.Add(p.Category);
                    lvi.Tag = p; // store product reference
                    listViewProducts.Items.Add(lvi);

                    productPrices[p.Name] = p.Price;
                    index++;
                }
            }
        }

        // --- Adding product to cart when selected ---
        private void ListViewProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewProducts.SelectedItems.Count == 0) return;

            var item = listViewProducts.SelectedItems[0];
            var product = item.Tag as Product;
            if (product == null) { listViewProducts.SelectedItems.Clear(); return; }

            string name = product.Name;
            decimal price = product.Price;

            if (cart.ContainsKey(name))
                cart[name] = (price, cart[name].Quantity + 1);
            else
                cart[name] = (price, 1);

            RefreshReceipt();
            listViewProducts.SelectedItems.Clear();
        }

        public void RefreshReceipt()
        {
            listViewReceipt.Items.Clear();
            totalSum = 0m;

            foreach (var kv in cart)
            {
                decimal lineTotal = kv.Value.Price * kv.Value.Quantity;
                totalSum += lineTotal;

                var lvi = new ListViewItem(kv.Key);
                lvi.SubItems.Add(kv.Value.Price.ToString("0.00"));
                lvi.SubItems.Add(kv.Value.Quantity.ToString());
                lvi.SubItems.Add(lineTotal.ToString("0.00"));
                listViewReceipt.Items.Add(lvi);
            }

            sumLabel.Text = $"Suma: {totalSum:0.00} zł";
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (listViewReceipt.SelectedItems.Count == 0) return;

            var sel = listViewReceipt.SelectedItems[0];
            string name = sel.Text;

            if (cart.ContainsKey(name))
            {
                if (cart[name].Quantity > 1)
                    cart[name] = (cart[name].Price, cart[name].Quantity - 1);
                else
                    cart.Remove(name);
            }

            RefreshReceipt();
        }

        // --- Open admin panel (pass reference to products list) ---
        private void ButtonPanelAdmin_Click(object sender, EventArgs e)
        {
            using (var ap = new AdminPanel(products))
            {
                var dr = ap.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    // Admin made changes and saved — reload from file and refresh UI
                    LoadProductsFromFile();
                    RefreshProductCatalog();
                }
            }
        }

        private void ButtonPay_Click(object sender, EventArgs e)
        {
            using (PaymentForm pf = new PaymentForm(this))
            {
                if (pf.ShowDialog() == DialogResult.OK)
                {
                    SaveReceiptToFile(); // generujemy i otwieramy paragon
                    cart.Clear();
                    RefreshReceipt();
                    MessageBox.Show("Płatność zakończona, paragon zapisany i otwarty.");
                }
            }
        }

        public void SaveReceiptToFile()
        {
            string fileName = $"paragon_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("        PARAGON FISKALNY");
                sw.WriteLine("----------------------------------------");
                sw.WriteLine($"Data: {DateTime.Now}");
                sw.WriteLine("----------------------------------------");
                foreach (var kvp in cart)
                {
                    sw.WriteLine($"{kvp.Key}");
                    sw.WriteLine($"  {kvp.Value.Price:0.00} zł x {kvp.Value.Quantity}  =  {(kvp.Value.Price * kvp.Value.Quantity):0.00} zł");
                    sw.WriteLine();
                }
                sw.WriteLine("----------------------------------------");
                sw.WriteLine($"SUMA: {totalSum:0.00} zł");
                sw.WriteLine("----------------------------------------");
                sw.WriteLine("Dziękujemy za zakupy!");
            }

            // --- automatyczne otwarcie pliku paragonu w Notatniku ---
            System.Diagnostics.Process.Start("notepad.exe", fileName);
        }


    }
}
