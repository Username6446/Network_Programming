using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
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

namespace Project2_Multichat_UDP_Protocol
{
    public partial class MainWindow : Window
    {
        UdpClient udpClient;
        IPEndPoint serverEndPoint;
        const string serverAddress = "127.0.0.1";
        const int server_port = 4040;
        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = messages;
            udpClient = new UdpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), server_port);
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (msgTextBox.Text.Length > 0 && nicknameTextBox.Text.Length > 0)
            {
                string message = msgTextBox.Text;
                string nickname = nicknameTextBox.Text;
                SendMessage(message, nickname);
            }
        }

        private void Join_Button_Click(object sender, RoutedEventArgs e)
        {
            string message = "$<join>";
            if (nicknameTextBox.Text.Length > 0)
            {
                string nickname = nicknameTextBox.Text;
                SendMessage(message, nickname);
            }
            Listen();

        }
        private async void SendMessage(string message, string nickname)
        {
            string result = nickname + "!" + message;
            byte[] data = Encoding.UTF8.GetBytes(result);
            await udpClient.SendAsync(data, data.Length, serverEndPoint);
        }
        private async void Listen()
        {
            while (true)
            {
                var result = await udpClient.ReceiveAsync();
                string[] res = Encoding.UTF8.GetString(result.Buffer).Split('!');
                string nickname = res[0];
                string message = res[1];
                messages.Add(new MessageInfo(nickname, message, DateTime.Now));
                
            }
        }

        private void Leave_Button_Click(object sender, RoutedEventArgs e)
        {
            string message = "$<leave>";
            if (nicknameTextBox.Text.Length > 0)
            {
                string nickname = nicknameTextBox.Text;
                SendMessage(message, nickname);
            }
            Listen();

        }
    }
    class MessageInfo
    {
        public string NickName { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public MessageInfo(string NickName, string Message, DateTime Time)
        {
            this.NickName = NickName;
            this.Message = Message;
            this.Time = Time;
        }
        public override string ToString()
        {
            return $"NickName : {NickName} Message : {Message}. Time : {Time}";
        }
    }
}