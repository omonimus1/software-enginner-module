using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bank
{
    /// <summary>
    /// Interaction logic for SendMessage.xaml
    /// </summary>
    public partial class SendMessage : Window
    {
        public SendMessage()
        {
            InitializeComponent();
        }

        // Clear botton event 
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtBoxMessage.Text = "";
            txtBoxSender.Text = "";
        }

        private bool isInputEmpty()
        {
            if (txtBoxMessage.Text == "" || txtBoxSender.Text == "")
                return false; 
        }

        // Send button Event 
        private void Button_Send_Click(object sender, TextChangedEventArgs e)
        {
            if (!isInputEmpty())
                MessageBox.Show("Make sure you have filled sender and message textboxes", "Validation Error");
        }
    }
}
