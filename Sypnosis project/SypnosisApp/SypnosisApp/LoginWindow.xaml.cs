using MahApps.Metro.Controls;
using SypnosisApp.Stores_classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        ConnectionToDB connection = new ConnectionToDB();
        RFIDReader theRfid = new RFIDReader();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = this.EmailLoginTextBox.Text;
            bool result = await this.connection.Login(email, this.PasswordLoginTextBox.Password);
            if (result)
            {
                List<Store> storeList = await connection.GetShops();
                Vendor vendor = await connection.GetVendor(email, storeList);

                // Shop
                if (vendor.VendorStore.StoreTypeId == 1)
                {
                    SaleWindow saleWindow = new SaleWindow();
                    saleWindow.StoresSplitButton.SelectedItem = saleWindow.storeList.Find(x => x.StoreId == vendor.VendorStore.StoreId);
                    saleWindow.Show();
                    this.Close();
                }
                // Rental
                else if (vendor.VendorStore.StoreTypeId == 2)
                {
                    RentalWindow rentalWindow = new RentalWindow();
                    rentalWindow.StoresSplitButton.SelectedItem = rentalWindow.storeList.Find(x => x.StoreId == vendor.VendorStore.StoreId);
                    rentalWindow.Show();
                    this.Close();
                }
                // Festival Check In
                else if (vendor.VendorStore.StoreTypeId == 3)
                {
                    FestivalCheckInWindow fciWindow = new FestivalCheckInWindow();
                    fciWindow.Show();
                    this.Close();
                }
                // Festival Check Out
                else if (vendor.VendorStore.StoreTypeId == 4)
                {
                    FestivalCheckOutWindow fcoWindow = new FestivalCheckOutWindow();
                    fcoWindow.Show();
                    this.Close();
                }
                // Camping Check In
                else if (vendor.VendorStore.StoreTypeId == 5)
                {
                    CampingCheckInWindow cciWindow = new CampingCheckInWindow();
                    cciWindow.Show();
                    this.Close();
                }
                // Camping Check Out
                else if (vendor.VendorStore.StoreTypeId == 6)
                {
                    CampingCheckOutWindow ccoWindow = new CampingCheckOutWindow();
                    ccoWindow.Show();
                    this.Close();
                }
            }
        }

        private void RfidLoginFlyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            theRfid.StartUp();
            theRfid.tagScannedEvent += OnTagScanned;
            //the rfid is assigned to an employee - which employee
            //which shop is this employee working at
            //open this shop

        }

        public void OnTagScanned(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                tbRfid.Text = message;
            });
        }

        private async void tbRfid_TextChanged(object sender, TextChangedEventArgs e)
        {
            int accId = connection.GetVisitorIdFromRfid(tbRfid.Text);
            string email = connection.GetVisitorEmailFromRfid(tbRfid.Text);
            
            if (connection.IsEmployee(accId))
            {
                List<Store> storeList = await connection.GetShops();
                Vendor vendor = await connection.GetVendor(email, storeList);

                if (vendor != null)
                {
                    // Shop
                    if (vendor.VendorStore.StoreTypeId == 1)
                    {
                        SaleWindow saleWindow = new SaleWindow();
                        saleWindow.StoresSplitButton.SelectedItem = saleWindow.storeList.Find(x => x.StoreId == vendor.VendorStore.StoreId);
                        saleWindow.Show();
                        this.Close();
                    }
                    // Rental
                    else if (vendor.VendorStore.StoreTypeId == 2)
                    {
                        RentalWindow rentalWindow = new RentalWindow();
                        rentalWindow.StoresSplitButton.SelectedItem = rentalWindow.storeList.Find(x => x.StoreId == vendor.VendorStore.StoreId);
                        rentalWindow.Show();
                        this.Close();
                    }
                    // Festival Check In
                    else if (vendor.VendorStore.StoreTypeId == 3)
                    {
                        FestivalCheckInWindow fciWindow = new FestivalCheckInWindow();
                        fciWindow.Show();
                        this.Close();
                    }
                    // Festival Check Out
                    else if (vendor.VendorStore.StoreTypeId == 4)
                    {
                        FestivalCheckOutWindow fcoWindow = new FestivalCheckOutWindow();
                        fcoWindow.Show();
                        this.Close();
                    }
                    // Camping Check In
                    else if (vendor.VendorStore.StoreTypeId == 5)
                    {
                        CampingCheckInWindow cciWindow = new CampingCheckInWindow();
                        cciWindow.Show();
                        this.Close();
                    }
                    // Camping Check Out
                    else if (vendor.VendorStore.StoreTypeId == 6)
                    {
                        CampingCheckOutWindow ccoWindow = new CampingCheckOutWindow();
                        ccoWindow.Show();
                        this.Close();
                    }
                }
            }
        }
    }
}