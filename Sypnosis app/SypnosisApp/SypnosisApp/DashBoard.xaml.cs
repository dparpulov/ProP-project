using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : MetroWindow
    {
        private DispatcherTimer showTime;
        ConnectionToDB connection = new ConnectionToDB();

        public DashBoard()
        {
            InitializeComponent();

            #region GetData Code
            connection.GetShopName(comboShop);
            connection.GetRental(comboLoan);
            connection.GetShopEmployees(comboShopEmployee);
            connection.GetRentalEmployees(comboRentalEmp);

            int NrCamp= connection.GetNrCamp();
            int NrChecks = connection.GetNrChecksIn();
            int NrLeft = connection.GetNrLeft();
            int NrTicket = connection.GetNrTicket();
            int NrRegistered = connection.GetNrRegistered();

            int ICCamp = connection.GetCampIncome();
            int ICTicket = connection.GetTicketIncome();
            double ICShop = connection.GetShopIncome();
            double ICTotal = connection.GetTotalIncome();
            double ICRented = connection.GetRentalIncome()+connection.GetRentalIncome1();

            int nrShops = connection.GetNrTotalShop();
            int nrLoans = connection.GetNrTotalLoan();
            int nrTotalShopsLoans = connection.GetNrTotalShopLoan();
            int nrItems = connection.GetNrItems();
            int nrItemType = connection.GetNrItemTypes();
            int nrEmployee = connection.GetNrEmployees();
            int nrAdmins = connection.GetNrAdmins();
            int nrSalesItems = connection.GetNrSaleItems();
            int nrRentedItems = connection.GetNrRentedItems();
            int nrSubscriber = connection.GetNrSubscriber();

            tbSubscriber.Text = nrSubscriber.ToString();
            tbSaledItems.Text = nrSalesItems.ToString();
            tbRentedItems.Text = nrRentedItems.ToString();
            tbNradmins.Text = nrAdmins.ToString();
            tbNrEmployees.Text = nrEmployee.ToString();
            tbItemTypes.Text = nrItemType.ToString();
            tbItem.Text = nrItems.ToString();
            tbNrTotalLoan.Text = nrLoans.ToString();
            tbNrTotalShops.Text = nrShops.ToString();
            tbNrTotalShopsLoan.Text = nrTotalShopsLoans.ToString();
            tbNrCamp.Text = NrCamp.ToString();
            tbNrChecksin.Text = NrChecks.ToString();
            tbNrLeft.Text = NrLeft.ToString();
            tbNrTicket.Text = NrTicket.ToString();
            tbNrRegistered.Text = NrRegistered.ToString();
            tbLoanI.Text = ICRented.ToString();

            tbCampI.Text= ICCamp.ToString();
            tbTicketI.Text= ICTicket.ToString();
            tbShopI.Text= ICShop.ToString();
            tbTotalI.Text = connection.GetTotalIncome().ToString();

            connection.GetNotAssignEmployee(comboEmployeeNotAssigned);
            connection.GetShopID(comboShopsLoanedID);
            connection.GetLoanID(comboShopsLoanedID);
            #endregion
            showTime = new DispatcherTimer();
            showTime.Tick += new EventHandler(ShowTime_Tick);
            showTime.Interval = TimeSpan.FromSeconds(1);
            showTime.Start();
            connection.Search("", datagridTable);

        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
            this.Close();
        }
      
        private void ShowTime_Tick(object sender, EventArgs e)
        {
            lbTime.Text ="Time:"+ DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
        }

        private void tbnAddEmployee_Click(object sender, RoutedEventArgs e)
        {

            if (tbAddEmp.Text != "" && comboAddType.SelectedValue != null)
                {
                    int type = Convert.ToInt32(((ComboBoxItem)comboAddType.SelectedValue).Content);
                    connection.AddShops(type, tbAddEmp.Text);
                    MessageBox.Show("Add succeesfully");
                }
                else
                {
                    MessageBox.Show("Enter the information");
                }
          


        }
    
        private void btnPie_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "My User Control Dialog",
                Content = new Graph()
            };

            window.ShowDialog();
        }

        private void btnPie1_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "My User Control Dialog",
                Content = new Graph1()
            };

            window.ShowDialog();
        }

        private void btnSerach_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearchID.Text!="")
            {
                connection.Search(tbSearchID.Text, datagridTable);
            }
            else if (tbSearchFirstName.Text!="")
            {
                connection.Search(tbSearchFirstName.Text, datagridTable);
            }
            else if (tbSearchLastName.Text!="")
            {
                connection.Search(tbSearchLastName.Text, datagridTable);

            }
            else
            {
                MessageBox.Show("Please enter correct info");
            }


        }

        private void tbSearchID_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbSearchLastName.Text = "";
            tbSearchFirstName.Text = "";
        }

        private void tbSearchFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbSearchID.Text = "";
            tbSearchLastName.Text = "";
        }

        private void tbSearchLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbSearchID.Text = "";
            tbSearchFirstName.Text = "";
        }

        private void datagridTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected!=null)
            {
                btSelectedLastName.Text = row_selected["id"].ToString();
                btSelectedFirstName.Text = row_selected["first_name"].ToString();
                btSelectedId.Text = row_selected["last_name"].ToString();
                btSelectedBalance.Text = row_selected["balance"].ToString();

                try
                {
                    byte[] data = (byte[])row_selected["picture"];

                    MemoryStream strm = new MemoryStream();

                    strm.Write(data, 0, data.Length);

                    strm.Position = 0;

                    System.Drawing.Image img = System.Drawing.Image.FromStream(strm);

                    BitmapImage bi = new BitmapImage();

                    bi.BeginInit();

                    MemoryStream ms = new MemoryStream();

                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                    ms.Seek(0, SeekOrigin.Begin);

                    bi.StreamSource = ms;

                    bi.EndInit();

                    imgUsers.Source = bi;
                }
                catch (Exception)
                {

                    MessageBox.Show("No pictures");
                }
            }
        }

        private void tbnAssignEmployee_Click(object sender, RoutedEventArgs e)
        {
           
                if (comboShopsLoanedID.SelectedValue != null && comboShopsLoanedID.SelectedValue != null)
                {
                    int fid1 = int.Parse(comboShopsLoanedID.SelectedValue.ToString());
                    int fid = int.Parse(comboEmployeeNotAssigned.SelectedValue.ToString());
                    connection.AssignEmployee(fid1, fid);
                    MessageBox.Show("Assign succeesfully");
                }
                else
                {
                    MessageBox.Show("Enter the information");
                }
           
        }
    }
}
