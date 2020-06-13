using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Transaction_Classes
{
    public class TransactionDeposit : TransactionFile
    {
        #region properties
        public int AccountId { get; set; }
        public double DepositAmount { get; set; }
        #endregion

        #region constructors
        public TransactionDeposit(int accountId, double depositAmount, string fileName, string bankAccount, string startPeriod, string endPeriod, int depositQuantity) : base(fileName, bankAccount, startPeriod, endPeriod, depositQuantity)
        {
            this.AccountId = accountId;
            this.DepositAmount = depositAmount;
        }
        #endregion
    }
}
