using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System.Collections.ObjectModel;
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

namespace Project6_Gmail_FInal
{
    public class EmailMessage
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
    public partial class MainWindow : Window
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public ObservableCollection<EmailMessage> Emails { get; set; } = new ObservableCollection<EmailMessage>();
        public MainWindow(string login, string password)
        {
            InitializeComponent();
            Login = login;
            Password = password;
            emailsListBox.ItemsSource = Emails;

            LoadEmailsAsync(login, password);
        }
        private async void LoadEmailsAsync(string login, string password)
        {
            try
            {
                using (var client = new ImapClient())
                {
                    await client.ConnectAsync("imap.gmail.com", 993, true);
                    await client.AuthenticateAsync(login, password);

                    var inbox = client.Inbox;
                    await inbox.OpenAsync(FolderAccess.ReadOnly);

                    int count = inbox.Count;
                    int start = Math.Max(0, count - 20);

                    for (int i = count - 1; i >= start; i--)
                    {
                        var message = await inbox.GetMessageAsync(i);

                        Emails.Add(new EmailMessage
                        {
                            Subject = message.Subject ?? "(No Subject)",
                            From = message.From.ToString(),
                            Body = message.TextBody ?? message.HtmlBody ?? "No content",
                            Date = message.Date.DateTime
                        });
                    }

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void emailsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (emailsListBox.SelectedItem is EmailMessage selectedEmail)
            {
                EmailDetailWindow detailWindow = new EmailDetailWindow(selectedEmail);
                detailWindow.Owner = this;
                detailWindow.Show();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ComposeWindow composeWindow = new ComposeWindow(Login, Password);
            composeWindow.Owner = this;
            composeWindow.Show();

        }

    }
}