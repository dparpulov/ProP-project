using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Transaction_Classes
{
    public class TransactionFile
    {
        #region properties
        public string FileName { get; set; }
        public string BankAccount { get; set; }
        public string StartPeriod { get; set; }
        public string EndPeriod { get; set; }
        public int DepositQuantity { get; set; }
        #endregion

        #region constructors
        public TransactionFile(string fileName, string bankAccount, string startPeriod, string endPeriod, int depositQuantity)
        {
            this.FileName = fileName;
            this.BankAccount = bankAccount;
            this.StartPeriod = startPeriod;
            this.EndPeriod = endPeriod;
            this.DepositQuantity = depositQuantity;
        }
        #endregion
    }
}
