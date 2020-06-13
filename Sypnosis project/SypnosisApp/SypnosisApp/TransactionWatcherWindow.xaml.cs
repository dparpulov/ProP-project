using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
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
using SypnosisApp.Transaction_Classes;

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for TransactionWatcherWindow.xaml
    /// </summary>
    public partial class TransactionWatcherWindow : MetroWindow
    {
        ConnectionToDB connection = new ConnectionToDB();
        FileSystemWatcher watcher;
        MTObservableCollection<TransactionDeposit> transactionDepositList = new MTObservableCollection<TransactionDeposit>();
        public TransactionWatcherWindow()
        {
            InitializeComponent();
            TransactionListView.ItemsSource = this.transactionDepositList;
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(TransactionListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("FileName");
            collectionView.GroupDescriptions.Add(groupDescription);
            StartWatcher();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void StartWatcher()
        {
            watcher = new FileSystemWatcher();
            watcher.Path = System.AppDomain.CurrentDomain.BaseDirectory + "transaction_log_files";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.txt";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            watcher.EnableRaisingEvents = false;
            string[] lines;
            List<string> list = new List<string>();

            Thread.Sleep(3000);
            using (FileStream fs = new FileStream("transaction_log_files\\" + e.Name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                    lines = list.ToArray();
                    foreach (string l in lines.Skip(4))
                    {
                        string[] lineValues = l.Split(' ');
                        int accountId = Convert.ToInt32(lineValues[0]);
                        double depositAmount = Convert.ToDouble(lineValues[1], CultureInfo.InvariantCulture);
                        TransactionDeposit transactionDeposit = new TransactionDeposit(accountId, depositAmount, e.Name, lines[0], lines[1], lines[2], Convert.ToInt32(lines[3]));

                        connection.DepositBalance(accountId, depositAmount);

                        this.transactionDepositList.Add(transactionDeposit);
                    }
                    watcher.EnableRaisingEvents = true;
                }
            }
            
            //try
            //{
            //    watcher.EnableRaisingEvents = false;

            //    string[] lines = File.ReadAllLines("transaction_log_files\\" + e.Name);

            //    foreach (string line in lines.Skip(4))
            //    {
            //        string[] lineValues = line.Split(' ');
            //        int accountId = Convert.ToInt32(lineValues[0]);
            //        double depositAmount = Convert.ToDouble(lineValues[1], CultureInfo.InvariantCulture);
            //        TransactionDeposit transactionDeposit = new TransactionDeposit(accountId, depositAmount, e.Name, lines[0], lines[1], lines[2], Convert.ToInt32(lines[3]));

            //        connection.DepositBalance(accountId, depositAmount);

            //        //this.transactionDepositList.Add(transactionDeposit);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //}
            //finally
            //{
            //    watcher.EnableRaisingEvents = true;
            //    if (watcher != null)
            //    {
            //        ((IDisposable)watcher).Dispose();
            //        StartWatcher();
            //    }
            //}
        }
    }
}
