using System;
using System.Collections.Generic;
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
using System.IO;

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for FestivalCheckOutWindow.xaml
    /// </summary>
    public partial class FestivalCheckOutWindow : MetroWindow
    {
        RFIDReader theRfid = new RFIDReader();
        ConnectionToDB connection = new ConnectionToDB();
        ObservableCollection<StoreRentItem> rentalReturnList = new ObservableCollection<StoreRentItem>();
        ObservableCollection<StoreRentItem> rentalReturnConfirmList = new ObservableCollection<StoreRentItem>();

        public FestivalCheckOutWindow()
        {
            InitializeComponent();
            theRfid.StartUp();
            theRfid.tagScannedEvent += OnTagScanned;

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
        }

        public void OnTagScanned(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                tbRfidTag.Text = message;
            });
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (theRfid.TagConOpened)
            {
                theRfid.Close();
            }
        }

        private void tbRfidTag_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowInfo();
            if (tbRfidTag.Text != "")
            {
                int ticketID = connection.GetTicketIdFromRfid(tbRfidTag.Text);
                string hasTicket = connection.VisitorHasRfidAssigned(ticketID);
                ObservableCollection<StoreRentItem> tempRentalReturnList = connection.ShowReturnItems(tbRfidTag.Text);
                if (tempRentalReturnList.Count == 0)
                {
                    int ticketStatus = connection.GetTicketStatus(hasTicket);
                    if (ticketStatus == 2)
                    {
                        connection.SetTicketStatusOutside(hasTicket);
                        lbFestivalCheckOutStatus.Content = "You can leave";
                        lbFestivalCheckOutStatus.Background = Brushes.Green;

                    }
                    else if (ticketStatus == 3)
                    {
                        lbFestivalCheckOutStatus.Content = "Check out of the camping area first";
                        lbFestivalCheckOutStatus.Background = Brushes.Red;
                    }
                    else
                    {
                        lbFestivalCheckOutStatus.Content = "Already out";
                        lbFestivalCheckOutStatus.Background = Brushes.Red;
                    }
                }
                else
                {
                    foreach (var item in tempRentalReturnList)
                    {
                        //this.rentalReturnList.Add(item);
                        rentalReturnList.Add(item);
                    }
                    lbFestivalCheckOutStatus.Content = "Return all loaned items!";
                    lbFestivalCheckOutStatus.Background = Brushes.Red;
                }
            }
        }

        private void ShowInfo()
        {
            int ticketId = connection.GetTicketIdFromRfid(tbRfidTag.Text);
            string name = connection.GetVisitorNameRfid(tbRfidTag.Text);
            int accountId = connection.GetVisitorIdFromRfid(tbRfidTag.Text);
            double balance = connection.GetBalanceByRfid(tbRfidTag.Text);

            byte[] pic = connection.getVisitorPicture(accountId);
            MemoryStream strm = new MemoryStream();
            if (pic != null)
            {
                strm.Write(pic, 0, pic.Length);
                strm.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();
                pbPicture.Source = bi;
            }
            else
            {
                BitmapImage image = new BitmapImage(new Uri("img/no-profile.jpg", UriKind.Relative));
                pbPicture.Source = image;
            }


            tbTicketNum.Text = ticketId.ToString();
            tbName.Text = name;
            tbID.Text = accountId.ToString();
            tbBalance.Text = "\u20AC" + balance.ToString();
        } //Fills all the info when an rfid is scanned

        private void ReturnItemsButton_Click(object sender, RoutedEventArgs e)
        {
            this.connection.ReturnItems(this.rentalReturnConfirmList, this.tbRfidTag.Text);

            this.rentalReturnConfirmList.Clear();

            int ticketID = connection.GetTicketIdFromRfid(tbRfidTag.Text);
            string hasTicket = connection.VisitorHasRfidAssigned(ticketID);

            if (rentalReturnConfirmList.Count == 0 && rentalReturnList.Count == 0)
            {
                connection.SetTicketStatusOutside(hasTicket);
                lbFestivalCheckOutStatus.Content = "You can leave";
                lbFestivalCheckOutStatus.Background = Brushes.Green;
            }
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
