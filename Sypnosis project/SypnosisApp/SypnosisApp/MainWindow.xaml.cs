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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnFestivalCheckIn_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            FestivalCheckInWindow fciWindow = new FestivalCheckInWindow();
            fciWindow.ShowDialog();
            ///this.Close();
        }

        private void btnFestivalCheckOut_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            FestivalCheckOutWindow fcoWindow = new FestivalCheckOutWindow();
            fcoWindow.ShowDialog();
            //this.Close();
        }
        private void btnCampingCheckIn_Click(object sender, RoutedEventArgs e)
        {
           // this.Hide();
            CampingCheckInWindow cciWindow = new CampingCheckInWindow();
            cciWindow.ShowDialog();
           // this.Close();
        }

        private void btnCampingCheckOut_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            CampingCheckOutWindow ccoWindow = new CampingCheckOutWindow();
            ccoWindow.ShowDialog();
            //this.Close();
        }

        private void btnSale_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            SaleWindow saleWindow = new SaleWindow();
            saleWindow.ShowDialog();
            //this.Close();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
            //this.Close();
        }

        private void btnDashBoard_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            DashBoard dashBoard = new DashBoard();
            dashBoard.ShowDialog();
            //this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            //this.Close();
        }

        private void btnRental_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            RentalWindow rental = new RentalWindow();
            rental.ShowDialog();
            //this.Close();
        }

        private void btnTransaction_Click(object sender, RoutedEventArgs e)
        {
            TransactionWatcherWindow transactionWatcherWindow = new TransactionWatcherWindow();
            transactionWatcherWindow.Show();
        }
    }
}
