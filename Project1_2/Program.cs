using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Project1_2
{
    internal class Program
    {
        static int port = 4040;
        static string address = "127.0.0.1";
        static void Main(string[] args)
        {
            IPAddress iPAddress = IPAddress.Parse(address);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            //Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            //ProtocolType.Udp);
            UdpClient listener = new UdpClient(iPEndPoint);

            try
            {
                //listenSocket.Bind(iPEndPoint);
                Console.WriteLine("Server started! Waiting for connection.....");
                while (true)
                {
                    //int bytes = 0;
                    //byte[] data = new byte[1024];
                    //listenSocket.ReceiveFrom(data,ref endPoint);
                    byte[] data = listener.Receive(ref endPoint);
                    string msg = Encoding.UTF8.GetString(data);
                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()} : {msg}. From:" +
                        $"{endPoint}");

                    string message = "Message was send!";
                    data = Encoding.UTF8.GetBytes(message);
                    listener.Send(data, data.Length, endPoint);
                    //listenSocket.SendTo(data, endPoint);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            //listenSocket.Shutdown(SocketShutdown.Both);
            //listenSocket.Close();
            listener.Close();
        }
    }
}
