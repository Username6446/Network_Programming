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
using System.Windows.Shapes;

namespace Project3_SMTP_Client
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        
        public LoginWindow()
        {
            InitializeComponent();
            loginTb.Text = "likovvana777@gmail.com";
            passwordTb.Text = "lqbiudxzyjccymgo";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Login = loginTb.Text;
            //Password = passwordTb.Text;
            MainWindow mainWindow = new MainWindow(loginTb.Text, passwordTb.Text);
            mainWindow.Show();
            this.Close();
        }
    }
}
