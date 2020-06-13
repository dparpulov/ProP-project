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
//using ceTe.DynamicBarcode.Creator;


namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : MetroWindow
    {
        ConnectionToDB connection = new ConnectionToDB();

        public RegistrationWindow()
        {
            InitializeComponent();
            btnBuyTicket.IsEnabled = false;
            
        }
        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private double totalPrice = 0;

        public bool CheckRequiredValues()
        {
            if (tbFirstName.Text == "" || tbLastName.Text == "" || tbEmailRegistration.Text == "" || pbPass.Password == "")
            {
                return false;
            }
            else
            {
                firstName = tbFirstName.Text;
                lastName = tbLastName.Text;
                email = tbEmailRegistration.Text;
                password = pbPass.Password;
                return true;
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string email = tbEmailRegistration.Text;
            if (connection.emailAlreadyInDB(email) == 0)
            {
                if (CheckRequiredValues())
                {
                    string encodedPass = Encode(password);
                    if (connection.RegisterVisitor(firstName, lastName, email, encodedPass))
                    {
                        lbRegisterStatus.Content = "Success";
                        lbRegisterStatus.Background = Brushes.Green;
                    }
                    else
                    {
                        lbRegisterStatus.Content = "Try again";
                        lbRegisterStatus.Background = Brushes.Red;
                    }
                }
                else
                {
                    lbRegisterStatus.Content = "Fill all required fields";
                    lbRegisterStatus.Background = Brushes.Red;
                }
            }
            else
            {
                lbRegisterStatus.Content = "There is a user with that email adress";
                lbRegisterStatus.Background = Brushes.Red;
            }

        }

        public static string Encode(string value)
        {
            return BCrypt.Net.BCrypt.HashPassword(value);
        }

        private void btnBuyTicket_Click(object sender, RoutedEventArgs e)
        {
            email = tbEmailTicket.Text;
            int visitorId = connection.GetVisitorIdFromEmail(email);
            if (email == "")
            {
                lbRegisterStatus.Content = "Enter an email";
            }
            else
            {
                if (ticketToggleSwitch.IsChecked == true && campspotToggleSwitch.IsChecked == false)
                {
                    BuyTicket(email, visitorId);
                }
                else if (campspotToggleSwitch.IsChecked == true && ticketToggleSwitch.IsChecked == false)
                {
                    ReserveCampspot(visitorId);

                }
                else if (ticketToggleSwitch.IsChecked == true && campspotToggleSwitch.IsChecked == true)
                {
                    //ticket
                    BuyTicket(email, visitorId);

                    //campspot
                    ReserveCampspot(visitorId);
                }
            }
        }

        private void ReserveCampspot(int visitorId)
        {
            int ticketId = connection.GetTicketIdFromVisitorId(visitorId);
            int freeCampspotId = connection.GetFreeCampspot();
            double balance = connection.GetBalanceByEmail(email);
            if (balance - totalPrice >= 0)
            {
                if (connection.UpdateCampspot(ticketId, freeCampspotId))
                {
                    connection.AddCamper(freeCampspotId, ticketId);
                    lbRegisterStatus.Background = Brushes.Green;
                    lbRegisterStatus.Content = "Campspot reserved";
                }
                else
                {
                    lbRegisterStatus.Background = Brushes.Red;
                    lbRegisterStatus.Content = "Campspot not reserved";
                    lbRegisterStatus.FontSize = 60;
                }
            }
            else
                lbRegisterStatus.Content = "Not enough balance";
        }

        public void BuyTicket(string email, int visitorId)
        {
            if (connection.BuyTicket(visitorId, email, totalPrice))
            {
                lbRegisterStatus.Background = Brushes.Green;
                lbRegisterStatus.Content = "Ticket bought";
                btnBuyTicket.IsEnabled = true;
                campspotToggleSwitch.IsEnabled = true;


                //This code sends an email to the user with the QR code as an attachment, we just dont have an official email from which we can send the email

                //int ticketId = connection.GetTicketIdFromEmail(email);
                //QRCode barcode = new QRCode(ticketId.ToString());
                //barcode.XDimension = 30;
                //barcode.Draw(@"C:\Users\user\Desktop\Prop\ProP_Group_31\SypnosisApp\SypnosisApp\QRCodes\Visitor" + visitorId + "QR.Png", 300, ImageFormat.Png); //have to make the path to the local folder
                //connection.SendEmail(email, visitorId);
            }
            else
            {
                lbRegisterStatus.Background = Brushes.Red;
                lbRegisterStatus.Content = "Something went wrong/Not enough balance";
            }
        }

        private void tbEmailTicket_TextChanged(object sender, TextChangedEventArgs e)
        {
            string email = tbEmailTicket.Text;
            int ticketId = connection.GetTicketIdFromEmail(email);
            if (connection.IsRegistered(email))
            {
                if (connection.GetTicketIdFromEmail(email) != 0)
                {
                    lbRegisterStatus.Background = Brushes.Red;
                    lbRegisterStatus.Content = "Visitor has ticket";
                    ticketToggleSwitch.IsChecked = false;
                    ticketToggleSwitch.IsEnabled = false;

                    if (connection.IsCamper(ticketId))
                    {
                        campspotToggleSwitch.IsChecked = false;
                        campspotToggleSwitch.IsEnabled = false;
                        lbRegisterStatus.Content += " and campspot";

                    }
                    else
                    {
                        campspotToggleSwitch.IsChecked = true;
                        campspotToggleSwitch.IsEnabled = true;

                    }
                }
                else
                {
                    lbRegisterStatus.Content = "Buy ticket";
                    ticketToggleSwitch.IsChecked = true;
                    ticketToggleSwitch.IsEnabled = true;
                }
                
                if (email == "")
                {
                    lbRegisterStatus.Content = "Enter an email";
                }
            }
            else
            {
                campspotToggleSwitch.IsChecked = false;
                campspotToggleSwitch.IsEnabled = false;
                ticketToggleSwitch.IsChecked = false;
                ticketToggleSwitch.IsEnabled = false;
                btnBuyTicket.IsEnabled = false;
                lbRegisterStatus.Content = "Register first";
            }
        }

        private void ToggleSwitch_IsCheckChanged(object sender, EventArgs e)
        {
            totalPrice = 0;
            if ((bool)ticketToggleSwitch.IsChecked)
            {
                btnBuyTicket.IsEnabled = true;
                totalPrice += 55;
                if (email == "")
                {
                    lbRegisterStatus.Content = "Enter an email";
                }
            }
            if ((bool)campspotToggleSwitch.IsChecked)
            {
                btnBuyTicket.IsEnabled = true;
                totalPrice += 20;
                if (email == "")
                {
                    lbRegisterStatus.Content = "Enter an email";
                }
            }
            if (!(bool)ticketToggleSwitch.IsChecked && !(bool)campspotToggleSwitch.IsChecked)
                btnBuyTicket.IsEnabled = false;

            tbPrice.Text = "\u20AC" + totalPrice;
        }
    }
}