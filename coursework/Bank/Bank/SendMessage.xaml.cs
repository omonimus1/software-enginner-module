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
        private string TextMessage;

        public SendMessage()
        {
            InitializeComponent();
        }

        /*
         * txtBoxSender_TextChanger(): Will keep the form open while user is typing the sender
         */
        private void txtBoxSender_TextChanged(object sender, EventArgs e)
        {
            this.TextMessage = txtBoxSender.Text;
        }

        /*
         * TextBoxMessage_TextChange(): Will keep the form open while user is typing
         */
        private void TextBoxMessage_TextChanged(object sender, EventArgs e)
        {
            this.TextMessage = txtBoxMessage.Text;
        }
        
        /*
         * isInputEmpty() : return true if the header, Message or Both are empty, false otherwise. 
         */
        private bool isInputEmpty()
        {
            if (txtBoxMessage.Text == "" || txtBoxSender.Text == "")
                return true;
            else
                return false; 
        }

        /*
         * 
         */
        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            if (isInputEmpty())
                MessageBox.Show("Make sure you have filled sender and message textboxes", "Validation Error");
            else
            {
                MessageBox.Show("All good", "Fine");
                // Start to validate now the messages
            }
                
        }

        /*
         * 
         */
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtBoxMessage.Text = "";
            txtBoxSender.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
