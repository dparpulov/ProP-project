using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Stores_classes
{
    public class StoreType
    {
        #region properties
        public int StoreTypeId { get; set; }
        public string StoreTypeName { get; set; }
        #endregion

        #region constructors
        public StoreType(int storeTypeId, string storeTypeName)
        {
            this.StoreTypeId = storeTypeId;
            this.StoreTypeName = storeTypeName;
        }
        #endregion
    }
}
