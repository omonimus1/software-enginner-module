using OrderSystem.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrderSystem.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        Button Content;



        public MainWindowViewModel()
        {
            AddOrderButtonContent = "Add Order";
            ViewOrderButtonContent = "View Order";

            AddOrderButtonCommand = new RelayCommand(AddOrderButtonClick);
            ViewOrderButtonCommand = new RelayCommand(ViewOrderButtonClick);
        }

        public string AddOrderButtonContent { get; private set; }
        public string ViewOrderButtonContent { get; private set; }

        public ICommand AddOrderButtonCommand { get; private set; }
        public ICommand ViewOrderButtonCommand { get; private set; }



        private void AddOrderButtonClick()
        {

        }

        private void ViewOrderButtonClick()
        {

        }
    }
}
