using Microsoft.Win32;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
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
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Project6_Gmail_FInal
{
    public partial class ComposeWindow : Window
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Server { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 465;

        ObservableCollection<string> attachments;

        public ComposeWindow(string login, string password)
        {
            InitializeComponent();
            Login = fromTb.Text = login;
            Password = password;

            toTb.Text = "wesicif817@pmdeal.com";
            attachments = new ObservableCollection<string>();
            this.DataContext = attachments;

        }

        private async void Button_Send(object sender, RoutedEventArgs e)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Ivan Lykov", fromTb.Text));
            message.To.Add(new MailboxAddress("", toTb.Text));
            message.Subject = subjectTb.Text;

            var builder = new BodyBuilder();
            builder.TextBody = messageTb.Text + $"\n\nSent from my WPF App\nUser: {Login}";

            foreach (string filePath in attachments)
            {
                builder.Attachments.Add(filePath);
            }

            message.Body = builder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(Server, Port, true);

                    await client.AuthenticateAsync(Login, Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                MessageBox.Show("Повідомлення успішно відправлено!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                messageTb.Text = "";
                attachments.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відправки: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Pin(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    attachments.Add(file);
                }
            }
        }
    }
}
