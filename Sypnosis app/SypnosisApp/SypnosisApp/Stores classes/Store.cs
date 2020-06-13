using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Stores_classes
{
    public class Store : StoreType
    {
        #region properties
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public List<StoreSaleItem> StoreItemsList { get; set; }
        #endregion

        #region constructors
        public Store(int storeId, string storeName, int storeTypeId, string storeTypeName) : base(storeTypeId, storeTypeName)
        {
            this.StoreId = storeId;
            this.StoreName = storeName;
        }
        #endregion
    }
}