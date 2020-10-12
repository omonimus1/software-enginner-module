using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrderSystem.ViewModels
{
    class AddOrderViewModel : BaseViewModel
    {
        #region TextBlock Content
        public string ItemNameTextBlock { get; private set; }
        public string ItemPriceTextBlock { get; private set; }
        public string ItemPaidTextBlock { get; private set;  }
        #endregion

        // TextBox Content
        #region Paid CheckBox
        public bool IsPaid( get; set;)
        #endregion

        #region Constructor
        public AddOrderViewModel()
        {
            ItemNameTextBlock = "iteam Name";
            ItemPriceTextBlock = "Iteam price";
            ItemPaidTextBlock = "Paid";

    `       ItemNameTextBox = string.Empty;
            ItemPriceTextBox = string.Empty;

            IsPaid = false; 
        }


    }
}
