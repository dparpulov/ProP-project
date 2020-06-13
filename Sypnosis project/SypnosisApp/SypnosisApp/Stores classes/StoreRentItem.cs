using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Stores_classes
{
    public class StoreRentItem : Item
    {
        #region properties
        public int StoreRentItemQuantity { get; set; }
        public double StoreRentItemPrice { get; set; }
        #endregion

        #region constructors
        public StoreRentItem(int sriQuantity, double sriPrice, int itemId, string itemName, int itemQuantity, byte[] itemPicture, int itemTypeId, string itemTypeName) :
            base(itemId, itemName, itemQuantity, itemPicture, itemTypeId, itemTypeName)
        {
            this.StoreRentItemQuantity = sriQuantity;
            this.StoreRentItemPrice = sriPrice;
        }
        #endregion
    }
}
