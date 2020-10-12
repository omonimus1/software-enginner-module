using OrderSystem.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderSystem.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        public string AddOrderButtonContent { get; private set; }
        public string ViewOrderButtonContent { get; private set; }

        public ICommand AddOrderButtonCommand { get; private set; }
        public ICommand ViewOrderButtonCommand { get; private set; }

        public MainWindowViewModel()
        {
            AddOrderButtonContent = "Add order";
            ViewOrderButtonCommand = "View Orders";

            AddOrderButtonCommand = new RelayCommand(AddOrderButtonClick);
            ViewOrderButtonCommand = new RelayCommand(ViewOrderButtonClick); 
        }

        private void AddOrderButtonClick()
        {

        }

        private void ViewOrderButtonClick()
        {

        }
    }
}
