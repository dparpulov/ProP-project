using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Stores_classes
{
    public class StoreSaleItem : Item
    {
        #region properties
        public int StoreSaleItemQuantity { get; set; }
        public double StoreSaleItemPrice { get; set; }
        #endregion

        #region constructors
        public StoreSaleItem(int ssiQuantity, double ssiPrice, int itemId, string itemName, int itemQuantity, byte[] itemPicture, int itemTypeId, string itemTypeName) : base(itemId, itemName, itemQuantity, itemPicture, itemTypeId, itemTypeName)
        {
            this.StoreSaleItemQuantity = ssiQuantity;
            this.StoreSaleItemPrice = ssiPrice;
        }
        #endregion
    }
}
