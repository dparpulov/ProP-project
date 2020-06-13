using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidget22;
using Phidget22.Events;

namespace SypnosisApp.Stores_classes
{
    public class Item : ItemType
    {
        #region properties
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public byte[] ItemPicture { get; set; }
        #endregion

        #region constructors
        public Item(int itemId, string itemName, int itemQuantity, byte[] itemPicture, int itemTypeId, string itemTypeName) : base(itemTypeId, itemTypeName)
        {
            this.ItemId = itemId;
            this.ItemName = itemName;
            this.ItemQuantity = itemQuantity;
            this.ItemPicture = itemPicture;
        }
        #endregion

        #region methods
        public override string ToString()
        {
            return this.ItemName.ToString();
        }
        #endregion
    }
}
