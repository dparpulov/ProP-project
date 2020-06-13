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
using System.IO;

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for CampingCheckOutWindow.xaml
    /// </summary>
    public partial class CampingCheckOutWindow : MetroWindow
    {
        RFIDReader theRfid = new RFIDReader();
        ConnectionToDB connection = new ConnectionToDB();

        public CampingCheckOutWindow()
        {
            InitializeComponent();
            theRfid.StartUp();
            theRfid.tagScannedEvent += OnTagScanned;

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
            //this.Hide();
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.ShowDialog();
            this.Close();
            if (theRfid.TagConOpened)
            {
                theRfid.Close();
            }

        }

        private void fillTextBoxInfo(string rfidTag)
        {
            int ticketId = connection.GetTicketIdFromRfid(rfidTag);
            string name = connection.GetVisitorNameRfid(rfidTag);
            int accountId = connection.GetVisitorIdFromRfid(rfidTag);
            int campspotId = connection.GetCampspotIdFromRfid(rfidTag);

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
            tbAccountId.Text = accountId.ToString();
            tbCampspotId.Text = campspotId.ToString();
        } // gets TicketID, Name, AccountId and campspotID

        private void tbRfidTag_TextChanged(object sender, TextChangedEventArgs e)
        {
            string rfidTag = tbRfidTag.Text;
            int ticketId = connection.GetTicketIdFromRfid(rfidTag);

            if (tbRfidTag.Text != "")
            {
                fillTextBoxInfo(rfidTag);
                if (connection.IsCamper(ticketId))
                {
                    if (connection.GetTicketStatus(rfidTag) == 3)
                    {
                        connection.SetTicketStatusFestivalRfid(rfidTag);
                        lbCampOutStatus.Content = "Success";
                        lbCampOutStatus.Background = Brushes.Green;

                    }
                    else if (connection.IsAlreadyCheckedFestival(rfidTag))
                    {
                        lbCampOutStatus.Content = "Already checked out";
                        lbCampOutStatus.Background = Brushes.Red;
                    }
                }
                else
                {
                    lbCampOutStatus.Content = "No access to camping";
                    lbCampOutStatus.Background = Brushes.Red;

                }
            }
            else
            {
                lbCampOutStatus.Content = "Scan your bracelet.";
                lbCampOutStatus.Background = Brushes.Red;
            }
        }
    }
}