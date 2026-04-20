using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project3_SMTP_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        //public MainWindow()
        //{
        //    InitializeComponent();
        //    fromTb.Text = "likovvana777@gmail.com";
        //    toTb.Text = "wesicif817@pmdeal.com";
        //}
        ObservableCollection<string> attachments;
        public MainWindow(string login, string password)
        {
            InitializeComponent();
            Login = fromTb.Text = login;
            Password = password;
            toTb.Text = "wesicif817@pmdeal.com";
            Server = "smtp.gmail.com";
            Port = 587;
            attachments = new ObservableCollection<string>();
            this.DataContext = attachments;
            priorityCb.ItemsSource = Enum.GetValues(typeof(MailPriority));
            priorityCb.SelectedIndex = 1;
        }

        private void Button_Send(object sender, RoutedEventArgs e)
        {
            MailMessage message = new MailMessage(fromTb.Text, toTb.Text, subjectTb.Text, messageTb.Text+ $"\nFrom : \n{Login}");

            //using (StreamReader sr = new StreamReader(@"Files/mail.html"))
            //{
            //    message.Body = sr.ReadToEnd();
            //    message.IsBodyHtml = true;
            //}
            if (priorityCb.SelectedItem is MailPriority selectedPriority)
            {
                message.Priority = selectedPriority;
            }
            else
            {
                message.Priority = MailPriority.Normal; 
            }
            foreach (string item in attachments)
            {
                message.Attachments.Add(new Attachment(@$"{item}"));
            }

            SmtpClient smtpClient = new SmtpClient(Server, Port);
            smtpClient.EnableSsl = true;

            smtpClient.Credentials = new NetworkCredential(Login, Password);

            smtpClient.SendCompleted += SmtpClient_SendCompleted;
            smtpClient.SendAsync(message, message);

        }

        private void SmtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show($"Error: {e.Error.Message}");
            else
                MessageBox.Show("Message was sent!");

            if (e.UserState is MailMessage msg) msg.Dispose();
            (sender as SmtpClient)?.Dispose();
        }

        private void Button_Pin(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFolderDialog = new OpenFileDialog();
            
            if(openFolderDialog.ShowDialog() == true)
            {
                attachments.Add(openFolderDialog.FileName);
            }
        }
    }
}