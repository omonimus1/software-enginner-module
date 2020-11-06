using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coursework2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        // Message Botton Event
        public void Button_Message_Click(object sender, RoutedEventArgs e)
        {
            SendMessage objectSendMessage = new SendMessage();
            this.Visibility = Visibility.Hidden;
            objectSendMessage.Show();
        }



       // [System.Runtime.InteropServices.ComVisible(true)]
        public void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            // Set initial directory    
            //openFileDlg.InitialDirectory = @"/";
            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                // Print Filename 
                FileNameTextBox.Text = openFileDlg.FileName;
                // Print content of the file 
                string fileContent = System.IO.File.ReadAllText(openFileDlg.FileName);
                SendMessage s = new SendMessage();
                s.ManageMessage(fileContent);
            }
        }
    }
}
