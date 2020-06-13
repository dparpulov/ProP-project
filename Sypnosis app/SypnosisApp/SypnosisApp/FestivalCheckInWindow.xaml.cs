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
using System.Threading;
using System.IO;

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for FestivalCheckInWindow.xaml
    /// </summary>
    public partial class FestivalCheckInWindow : MetroWindow
    {
        BarcodeReader bcr;
        ConnectionToDB connection = new ConnectionToDB();
        RFIDReader theRfid = new RFIDReader();
        //bool visitorCheckedIn = false;

        public FestivalCheckInWindow()
        {
            InitializeComponent();
            bcr = new BarcodeReader(this, this.cbVideoDevices, this.imgBarcodeReader, tbQrValue);
            RfidButtonsDisable();
        }

        public void OnTagScanned(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                tbRfidTag.Text = message;
            });
        } //adds the method that writes the RFID value to the eventhandler

        private void btnToggleBarcodeReader_Click(object sender, RoutedEventArgs e)
        {
            tbQrValue.Text = "";
            bcr.OnOff();
        } //starts the camera so visitors can scan their ticket QR codes

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            if (bcr.ShowFrames == true)
            {
                bcr.OnOff();
            }
            this.Close();
            if (theRfid.TagConOpened)
            {
                theRfid.Close();
            }

        } //goes back to the main window

        private void tbQrValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            int QrValue = 0;
            
            if (tbQrValue.Text != "")
            {
                QrValue = Convert.ToInt32(tbQrValue.Text);
                FillTextBoxInfo(QrValue);
                if (!connection.IsAlreadyCheckedFestivalQR(QrValue))
                {
                    if (connection.VisitorCheckIn(QrValue))
                    {
                        lbFestivalCheckInStatus.Background = Brushes.Green;
                        lbFestivalCheckInStatus.Content = "Success";
                        //visitorCheckedIn = true;
                        connection.SetTicketStatusFestivalQr(QrValue);
                    }
                    else
                    {
                        tbVisitorName.Text = "";
                        tbVisitorTicketId.Text = "";
                        RfidButtonsDisable();
                        lbFestivalCheckInStatus.Background = Brushes.Red;
                        lbFestivalCheckInStatus.Content = "No such visitor ID.";
                    }
                }
                else
                {
                    lbFestivalCheckInStatus.Background = Brushes.Red;
                    lbFestivalCheckInStatus.Content = "Already checked in.";
                }
            }
        }/*Checks if the QRValue texbox is not empty -> then it fills
        the textboxes with information about the visitor and checks if he has already checked in 
        -> if the visitor has not checked in yet then it checks him in and changes his ticket status otherwise it shows why he is not allowed in*/

        public void RfidButtonsDisable()
        {
            btnAssign.IsEnabled = false;
            tbRfidTag.IsEnabled = false;
            tbRfidTag.Opacity = 0;
            btnAssign.Opacity = 0;
        }//disables and hides the textbox and rfidAssign button
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (bcr.ShowFrames == true)
            {
                bcr.OnOff();
            }
        } //stops the camera if it is not closed when the app is closed

        private void btnRFIDScan_Click(object sender, RoutedEventArgs e)
        {
            tbRfidTag.Text = "";
            if (tbQrValue.Text != "")
            {
                btnAssign.IsEnabled = true;
                tbRfidTag.Opacity = 100;
                btnAssign.Opacity = 100;
                theRfid.StartUp();
                theRfid.tagScannedEvent += OnTagScanned;
            }
        } //makes the btnAssign and tbTagValue visible
        
        private void FillTextBoxInfo(int ticketId)
        {
            string name = connection.GetVisitorName(ticketId);
            int visitorId = connection.GetVisitorIdFromTicketId(ticketId);
            int campspotId = 0;
            tbVisitorTicketId.Text = tbQrValue.Text;
            string rfidValue = connection.VisitorHasRfidAssigned(ticketId);
            if (connection.IsCamper(ticketId))
            {
                //hasCampspot = "Yes";
                campspotId = connection.GetCampspotIdFromTicketId(ticketId);
            }
            else
                //hasCampspot = "No";
            if (rfidValue == "")
                tbRfidAlreadyAssigned.Text = "Not assigned";
            else
                tbRfidAlreadyAssigned.Text = rfidValue;

            byte[] pic = connection.getVisitorPicture(visitorId);
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

            tbVisitorName.Text = name;
            tbVisitorId.Text = visitorId.ToString();
            //tbVisitorHasCamp.Text = hasCampspot;
            tbVisitorCampspot.Text = campspotId.ToString();

        }//fills the textboxes that give visitor information when the QR is scanned
        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            string rfidValue = theRfid.GetTagValue;
            int id = Convert.ToInt32(tbQrValue.Text);

            if (connection.SetRFID(rfidValue, id))
            {
                lbFestivalCheckInStatus.Content = "RFID assigned";
                lbFestivalCheckInStatus.Background = Brushes.Green;
                btnAssign.IsEnabled = false;
            }
            else
            {
                lbFestivalCheckInStatus.Content = "RFID taken";
                lbFestivalCheckInStatus.Background = Brushes.Red;
            }
        } //assigns an rfid to an account if the rfid is free
    }
}