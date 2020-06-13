using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Stores_classes
{
    public class Account
    {
        #region properties
        public int AccountId { get; set; }
        // Left out nationality, dob, gender and balance for now
        public string AccountFirstName { get; set; }
        public string AccountLastName { get; set; }
        public string AccountEmail { get; set; }
        #endregion

        #region constructors
        public Account(int accountId, string accountFirstName, string accountLastName, string accountEmail)
        {
            this.AccountId = accountId;
            this.AccountFirstName = accountFirstName;
            this.AccountLastName = accountLastName;
            this.AccountEmail = accountEmail;
        }
        #endregion
    }
}
