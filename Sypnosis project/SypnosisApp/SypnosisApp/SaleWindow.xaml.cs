using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using SypnosisApp.Stores_classes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MahApps.Metro.Controls.Dialogs;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for SaleWindow.xaml
    /// </summary>
    public partial class SaleWindow : MetroWindow
    {
        public List<Store> storeList;
        List<StoreSaleItem> storeItemList;
        ObservableCollection<StoreSaleItem> purchaseList = new ObservableCollection<StoreSaleItem>();
        ConnectionToDB connection = new ConnectionToDB();
        RFIDReader rfid = new RFIDReader();

        public SaleWindow()
        {
            InitializeComponent();
            this.GetStores();
        }

        private async void GetStores()
        {
            this.storeList = await connection.GetShops();
            this.StoresSplitButton.ItemsSource = this.storeList.FindAll(x => x.StoreTypeId == 1);

            SaleItemListView.ItemsSource = purchaseList;
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(SaleItemListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ItemName");
            collectionView.GroupDescriptions.Add(groupDescription);

            purchaseList.CollectionChanged += this.OnCollectionChanged;
        }

        private async void GetStoreSaleItems(Store store)
        {
            this.purchaseList.Clear();
            this.storeItemList = null;
            this.storeItemList = await connection.GetStoreSaleItems(store, this);

            // Reset TabItems visibility and remove children
            this.FoodTabItem.Visibility = Visibility.Hidden;
            this.BeverageTabItem.Visibility = Visibility.Hidden;
            this.HouseholdTabItem.Visibility = Visibility.Hidden;
            this.ElectronicTabItem.Visibility = Visibility.Hidden;
            this.FoodWrapPanel.Children.Clear();
            this.BeverageWrapPanel.Children.Clear();
            this.HouseholdWrapPanel.Children.Clear();
            this.ElectronicWrapPanel.Children.Clear();

            // Add StoreSaleItems as Tiles to TabItems by ItemType
            foreach (StoreSaleItem ssi in this.storeItemList)
            {
                ImageBrush brush;
                BitmapImage bi;
                using (var ms = new MemoryStream(ssi.ItemPicture))
                {
                    brush = new ImageBrush();
                    bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.StreamSource = ms;
                    bi.EndInit();
                }
                brush.ImageSource = bi;
                Tile tile = new Tile
                {
                    Title = ssi.ItemName,
                    Count = "\u20AC" + ssi.StoreSaleItemPrice.ToString(),
                    Foreground = Brushes.Black,
                    Background = brush,
                    Tag = ssi.ItemId,
                };
                tile.Click += AddToPurchaseList_Click;

                // Make TabItem visible at first Tile added. Empty TabItems will remain hidden. 
                switch (ssi.ItemTypeId)
                {
                    case 1:
                        if (!this.FoodTabItem.IsVisible)
                            this.FoodTabItem.Visibility = Visibility.Visible;
                        this.FoodWrapPanel.Children.Add(tile);
                        break;
                    case 2:
                        if (!this.BeverageTabItem.IsVisible)
                            this.BeverageTabItem.Visibility = Visibility.Visible;
                        this.BeverageWrapPanel.Children.Add(tile);
                        break;
                    case 3:
                        if (!this.HouseholdTabItem.IsVisible)
                            this.HouseholdTabItem.Visibility = Visibility.Visible;
                        this.HouseholdWrapPanel.Children.Add(tile);
                        break;
                    case 4:
                        if (!this.ElectronicTabItem.IsVisible)
                            this.ElectronicTabItem.Visibility = Visibility.Visible;
                        this.ElectronicWrapPanel.Children.Add(tile);
                        break;
                }
            }
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.WindowState = WindowState.Maximized;
            //mainWindow.ShowDialog();
            this.Close();
            if (rfid.TagConOpened)
            {
                rfid.Close();
            }
        }

        private void AddToPurchaseList_Click(object sender, RoutedEventArgs e)
        {
            Tile tile = (Tile)sender;
            this.purchaseList.Add(this.storeItemList.Find(x => x.ItemId.ToString() == tile.Tag.ToString()));
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SaleItemListView.SelectedItem != null)
            {
                this.purchaseList.Remove((StoreSaleItem)this.SaleItemListView.SelectedItem);
            }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            double totalPrice = 0;
            foreach (StoreSaleItem ssi in this.purchaseList)
            {
                totalPrice += ssi.StoreSaleItemPrice;
            }
            this.TotalPriceTextBlock.Text = "\u20AC" + totalPrice;
        }

        private void StoresSplitButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.GetStoreSaleItems(((Store)((SplitButton)sender).SelectedItem));
        }
        public void OnTagScanned(string message)
        {

            this.Dispatcher.Invoke(() =>
            {
                tbRfidTag.Text = message;
            });
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            rfid.StartUp();
            rfid.tagScannedEvent += OnTagScanned;
            if (tbRfidTag.Text != "")
            {
                try
                {
                    char[] charsToTrim = { '\u20AC', ' ', '€' };
                    string total = TotalPriceTextBlock.Text;
                    double price = Convert.ToDouble(total.Trim(charsToTrim));
                    connection.ShopPayRFID(price, tbRfidTag.Text);
                    int accountId = connection.GetVisitorIdFromRfid(tbRfidTag.Text);
                    Debug.WriteLine(accountId.ToString());
                    connection.InsertSaleItems(((Store)this.StoresSplitButton.SelectedItem).StoreId, accountId, this.purchaseList);
                    this.purchaseList.Clear();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}