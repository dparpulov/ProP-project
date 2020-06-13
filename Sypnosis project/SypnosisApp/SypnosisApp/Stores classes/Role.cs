using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Stores_classes
{
    public class Role
    {
        #region properties
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        #endregion

        #region constructors
        public Role(int roleId, string roleName)
        {
            this.RoleId = roleId;
            this.RoleName = roleName;
        }
        #endregion
    }
}
