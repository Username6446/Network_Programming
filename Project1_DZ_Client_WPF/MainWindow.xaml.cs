using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Project1_DZ_Client_WPF
{
    public partial class MainWindow : Window
    {

        public ObservableCollection<string> messages { get; set; }

        private TcpClient? _client;
        private NetworkStream? _stream;

        public MainWindow()
        {
            InitializeComponent();
            messages = new ObservableCollection<string>();
            DataContext = this;
            ipAddressTb.Text = "127.0.0.1";
            portTb.Text = "4040";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string message = messageTb.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            string ip = ipAddressTb.Text;
            if (!int.TryParse(portTb.Text, out int port))
            {
                MessageBox.Show("Неправильний формат порту!");
                return;
            }

            try
            {
                if (_client == null || !_client.Connected)
                {
                    _client = new TcpClient(ip, port);
                    _stream = _client.GetStream();
                }

                messages.Add($"Ви: {message}");

                byte[] data = Encoding.UTF8.GetBytes(message);
                _stream!.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytes = _stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytes);

                messages.Add($"Говорун: {response}");

                messageTb.Clear();
            }
            catch (Exception ex)
            {
                messages.Add($"[Помилка з'єднання]: {ex.Message}");

                _client?.Close();
                _client = null;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _client?.Close();
            base.OnClosed(e);
        }
    }
}