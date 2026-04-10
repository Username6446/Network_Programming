using System.Collections.ObjectModel;
using System.IO;
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
        TcpClient tcpClient;
        IPEndPoint serverEndPoint;
        const string serverAddress = "127.0.0.1";
        const int server_port = 4040;

        NetworkStream ns = null;
        StreamReader sr = null;
        StreamWriter sw = null;

        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = messages;
            tcpClient = new TcpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), server_port);
            this.nicknameTextBox.Text = "q";
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (msgTextBox.Text.Length > 0 && nicknameTextBox.Text.Length > 0)
            {
                string message = msgTextBox.Text;
                string nickname = "";
                SendMessage(message, nickname);
            }
        }

        private void Connection_Button_Click(object sender, RoutedEventArgs e)
        {
            string message = "$<join>";
            try
            {
                tcpClient.Connect(serverEndPoint);
                ns = tcpClient.GetStream();
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);

                if (nicknameTextBox.Text.Length > 0)
                {
                    string nickname = nicknameTextBox.Text;
                    SendMessage(message, nickname);
                }
                Listen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }
        private void SendMessage(string message, string nickname)
        {
            //string allMessage = message+nickname;
            sw.WriteLine(message);
            sw.Flush();
        }
        private async void Listen()
        {
            while (true)
            {
                string? result = await sr.ReadLineAsync();
                //string[] res = result!.Split('!');
                //string nickname = res[0];
                //string message = res[1];
                messages.Add(new MessageInfo(result, "", DateTime.Now));
                
            }
        }

        private void Disconnect_Button_Click(object sender, RoutedEventArgs e)
        {
            ns.Close();
            tcpClient.Close();
            
            //string message = "$<leave>";
            //if (nicknameTextBox.Text.Length > 0)
            //{
            //    string nickname = nicknameTextBox.Text;
            //    SendMessage(message, nickname);
            //}
            //Listen();

        }
    }
    class MessageInfo
    {
        public string NickName { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public MessageInfo(string? NickName, string? Message, DateTime Time)
        {
            this.NickName = NickName ?? "";
            this.Message = Message ?? "";
            this.Time = Time;
        }
        public override string ToString()
        {
            return $"NickName : {NickName} Message : {Message}. Time : {Time}";
        }
    }
}