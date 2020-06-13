using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;
using SypnosisApp.Stores_classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for LoanWindow.xaml
    /// </summary>
    public partial class RentalWindow : MetroWindow
    {
        public List<Store> storeList;
        List<StoreRentItem> storeRentalItems;
        ObservableCollection<StoreRentItem> rentalList = new ObservableCollection<StoreRentItem>();
        ObservableCollection<StoreRentItem> rentalReturnList = new ObservableCollection<StoreRentItem>();
        ObservableCollection<StoreRentItem> rentalReturnConfirmList = new ObservableCollection<StoreRentItem>();
        ConnectionToDB connection = new ConnectionToDB();
        RFIDReader rfid = new RFIDReader();


        public RentalWindow()
        {
            InitializeComponent();
         
            this.GetStores();

            // Temporarily placed here
            // Return list view
            RentalItemReturnListView.ItemsSource = rentalReturnList;
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(RentalItemReturnListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ItemName");
            collectionView.GroupDescriptions.Add(groupDescription);
            // Return list view confirm
            RentalItemReturnConfirmListView.ItemsSource = rentalReturnConfirmList;
            CollectionView collectionView2 = (CollectionView)CollectionViewSource.GetDefaultView(RentalItemReturnConfirmListView.ItemsSource);
            PropertyGroupDescription groupDescription2 = new PropertyGroupDescription("ItemName");
            collectionView2.GroupDescriptions.Add(groupDescription2);

            rentalList.CollectionChanged += this.OnCollectionChanged;
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.ShowDialog();
            this.Close();
            if (rfid.TagConOpened)
            {
                rfid.Close();
            }

        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            double totalPrice = 0;
            foreach (StoreRentItem sri in this.rentalList)
            {
                totalPrice += sri.StoreRentItemPrice;
            }
            this.TotalPriceTextBlock.Text = "\u20AC" + totalPrice;
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.RentalItemListView.SelectedItem != null)
            {
                this.rentalList.Remove((StoreRentItem)this.RentalItemListView.SelectedItem);
            }
        }

        private void AddToPurchaseList_Click(object sender, RoutedEventArgs e)
        {
            Tile tile = (Tile)sender;
            this.rentalList.Add(this.storeRentalItems.Find(x => x.ItemId.ToString() == tile.Tag.ToString()));
        }

        private async void GetStores()
        {
            this.storeList = await connection.GetShops();
            this.StoresSplitButton.ItemsSource = this.storeList.FindAll(x => x.StoreTypeId == 2);

            RentalItemListView.ItemsSource = rentalList;
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(RentalItemListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ItemName");
            collectionView.GroupDescriptions.Add(groupDescription);
        }

        private async void GetStoreRentItems(Store store)
        {
            this.storeList.Clear();
            this.storeRentalItems = await connection.GetStoreRentItems(store, this);

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
            foreach (StoreRentItem ssi in this.storeRentalItems)
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
                    Count = "\u20AC" + ssi.StoreRentItemPrice.ToString(),
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

        private void StoresSplitButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.GetStoreRentItems(((Store)((SplitButton)sender).SelectedItem));
        }

        public void OnTagScanned(string message)
        {

            this.Dispatcher.Invoke(() =>
            {
                tbRfidTag.Text = message;
            });
        }

        private void btnConfrim_Click(object sender, RoutedEventArgs e)
        {
            rfid.StartUp();
            rfid.tagScannedEvent += OnTagScanned;
            if (tbRfidTag.Text != "")
            {
                try
                {
                    int accountId = connection.GetVisitorIdFromRfid(tbRfidTag.Text);
                    //Debug.WriteLine(accountId.ToString());
                    connection.InsertRentItems(((Store)this.StoresSplitButton.SelectedItem).StoreId, accountId, this.rentalList);
                    this.rentalList.Clear();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

    

        private void btnShow_Click_1(object sender, RoutedEventArgs e)
        {
            rfid.StartUp();
            rfid.tagScannedEvent += OnTagScanned;
            if (tbRfidTag.Text!="")
            {
                this.rentalReturnList.Clear();
                this.rentalReturnConfirmList.Clear();
                ObservableCollection<StoreRentItem> tempRentalReturnList = connection.ShowReturnItems(tbRfidTag.Text);
                foreach (var item in tempRentalReturnList)
                {
                    this.rentalReturnList.Add(item);
                }
                
            }
        }

        private void ReturnItemsButton_Click(object sender, RoutedEventArgs e)
        {
            this.connection.ReturnItems(this.rentalReturnConfirmList, this.tbRfidTag.Text);
            this.rentalReturnList.Clear();
            this.rentalReturnConfirmList.Clear();
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.RentalItemReturnConfirmListView.SelectedItems != null)
            {
                List<StoreRentItem> temp = new List<StoreRentItem>();
                foreach (StoreRentItem item in this.RentalItemReturnConfirmListView.SelectedItems)
                {
                    temp.Add(item);
                }
                foreach (StoreRentItem item in temp)
                {
                    this.rentalReturnList.Add(item);
                    this.rentalReturnConfirmList.Remove(item);
                }
            }
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.RentalItemReturnListView.SelectedItems != null)
            {
                List<StoreRentItem> temp = new List<StoreRentItem>();
                foreach (StoreRentItem item in this.RentalItemReturnListView.SelectedItems)
                {
                    temp.Add(item);
                } 
                foreach (StoreRentItem item in temp)
                {
                    this.rentalReturnConfirmList.Add(item);
                    this.rentalReturnList.Remove(item);
                }
            }
        }
    }
}
