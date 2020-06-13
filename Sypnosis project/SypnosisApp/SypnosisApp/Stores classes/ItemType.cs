using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp.Stores_classes
{
    public class ItemType
    {
        #region properties
        public int ItemTypeId { get; set; }
        public string ItemTypeName { get; set; }
        #endregion

        #region constructors
        public ItemType(int itemTypeId, string itemTypeName)
        {
            this.ItemTypeId = itemTypeId;
            this.ItemTypeName = itemTypeName;
        }
        #endregion
    }
}
