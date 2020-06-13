using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Stores_classes
{
    public class Vendor : Employee
    {
        #region properties
        public Store VendorStore { get; set; }
        #endregion

        #region constructors
        public Vendor(Store vendorStore, int employeeId, Role employeeRole, 
            int accountId, string accountFirstName, string accountLastName, string accountEmail) : 
            base(employeeId, employeeRole, accountId, accountFirstName, accountLastName, accountEmail)
        {
            this.VendorStore = vendorStore;
        }
        #endregion
    }
}
