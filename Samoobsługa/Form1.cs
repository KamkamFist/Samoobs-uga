using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Samoobsługa
{
    public partial class Form1 : Form
    {
        private const string ApiBaseUrl = "https://localhost:7217/";

        private List<Product> products = new List<Product>();
        public Dictionary<string, (decimal Price, int Quantity)> cart = new Dictionary<string, (decimal, int)>();
        private decimal totalSum = 0m;

        private User loggedUser = null;

        public Form1()
        {
            InitializeComponent();
            // ustawienia listview i imagelist (designer jest mega obsrany wiec zapytalem chata, blagam o wybaczenie za ten kod)
            imageList1.ImageSize = new Size(96, 96);

            listViewProducts.View = View.LargeIcon;
            listViewProducts.LargeImageList = imageList1;
            listViewProducts.MultiSelect = false;
            listViewProducts.SelectedIndexChanged += ListViewProducts_SelectedIndexChanged;

            listViewReceipt.View = View.Details;
            listViewReceipt.Columns.Clear();
            listViewReceipt.Columns.Add("Produkt", 130);
            listViewReceipt.Columns.Add("Cena", 70);
            listViewReceipt.Columns.Add("Ilość", 60);
            listViewReceipt.Columns.Add("Wartość", 80);
            listViewReceipt.FullRowSelect = true;
            listViewReceipt.GridLines = true;


            _ = LoadProductsFromApi();

            // ten wariat odsiweza co sekubde (troche malo ae dizla nawet git wiec zostawiam)
            System.Windows.Forms.Timer refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 1000;
            refreshTimer.Tick += async (s, e) =>
            {
                if (loggedUser != null)
                {
                    await RefreshLoggedUserData();
                }
            };
            refreshTimer.Start();




        }

        //mam 4 godzinna lobotomie przez te rozje**** zdjcia. poddaje sie 
        private async Task LoadProductsFromApi()
        {
            try
            {
                using HttpClient client = new HttpClient(); 
                var apiProducts = await client.GetFromJsonAsync<List<Product>>(ApiBaseUrl + "products");

                if (apiProducts != null)
                {
                    products = apiProducts;
                    await RefreshProductCatalog();
                }
                else
                {
                    MessageBox.Show("Brak produktów z API.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd przy pobieraniu produktów z API: " + ex.Message);
            }
        }

        private async Task RefreshProductCatalog()
        {
            imageList1.Images.Clear();
            listViewProducts.Items.Clear();

            using HttpClient client = new HttpClient();

            int index = 0;

            foreach (var p in products)
            {
                Image productImage = SystemIcons.Warning.ToBitmap(); 

                if (!string.IsNullOrWhiteSpace(p.ImageUrl))
                {
                    try
                    {
                        
                        byte[] bytes = await client.GetByteArrayAsync(p.ImageUrl);
                        using (var ms = new MemoryStream(bytes))
                        {
                            productImage = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        
                    }
                }

                imageList1.Images.Add(productImage);

                var lvi = new ListViewItem($"{p.Name} - {p.Price:0.00} zł")
                {
                    ImageIndex = index,
                    Tag = p 
                };

                listViewProducts.Items.Add(lvi);
                index++;
            }
        }






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

            decimal discountAmount = 0m;
            if (loggedUser != null && loggedUser.CardActive)
            {
                discountAmount = totalSum * loggedUser.Discount / 100m;
                labelDiscount.Text = $"Zniżka: {discountAmount:0.00} zł ({loggedUser.Discount}%)";
            }
            else
            {
                labelDiscount.Text = "Zniżka: 0.00 zł";
            }

            sumLabel.Text = $"Suma: {(totalSum - discountAmount):0.00} zł";
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            /*
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
            */
            MessageBox.Show("Usuwanie nie dziala bo bylo ruiboe ne fo pieczykolana(rżniete)");
        }

        private void ButtonPanelAdmin_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Panel admina zostal usuniety poniewaz byl on robiony do pana ktorego pieka kolana (nie dzialal, ale za to wygladal jak gowno)");
        }

        private void ButtonPay_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Koszyk jest pusty!");
                return;
            }

            decimal finalSum = totalSum;
            if (loggedUser != null && loggedUser.CardActive)
            {
                decimal discountAmount = totalSum * loggedUser.Discount / 100m;
                finalSum -= discountAmount;
            }


            cart.Clear();
            RefreshReceipt();
            MessageBox.Show("Płatność zakończona:3.");

        }



        private async void ButtonLogin_Click(object sender, EventArgs e)
        {
            string username = EnterNameTextbox.Text?.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Podaj login użytkownika.");
                return;
            }

            try
            {
                using HttpClient client = new HttpClient(); 
                loggedUser = await client.GetFromJsonAsync<User>(ApiBaseUrl + $"user/{username}");

                if (loggedUser != null)
                {
                    labelLoggedAs.Text = $"Zalogowany jako: {loggedUser.Name}";
                    RefreshReceipt();
                }
                else
                {
                    MessageBox.Show("Nie znaleziono użytkownika w API.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd przy pobieraniu danych z API: " + ex.Message);
            }
        }

        private async Task RefreshLoggedUserData()
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                using HttpClient client = new HttpClient(handler);

                var refreshedUser = await client.GetFromJsonAsync<User>(ApiBaseUrl + $"user/{loggedUser.Name}");
                if (refreshedUser != null)
                {
                    loggedUser.CardActive = refreshedUser.CardActive;
                    loggedUser.Discount = refreshedUser.Discount;
                    RefreshReceipt();
                }
            }
            catch
            {

            }
        }
    }



}
