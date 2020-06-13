using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Stores_classes
{
    public class Employee : Account
    {
        #region properties
        public int EmployeeId { get; set; }
        public Role EmployeeRole { get; set; }
        #endregion

        #region constructors
        public Employee(int employeeId, Role employeeRole, int accountId, string accountFirstName, string accountLastName, string accountEmail) : 
            base(accountId, accountFirstName, accountLastName, accountEmail)
        {
            this.EmployeeId = employeeId;
            this.EmployeeRole = employeeRole;
        }
        #endregion
    }
}
