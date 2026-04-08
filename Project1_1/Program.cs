using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Project1_1
{
    internal class Program
    {
        static int port = 4040;
        static string address = "127.0.0.1";
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint remotePoint = new IPEndPoint(IPAddress.Any, 0);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(address), port);
                //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                //    ProtocolType.Udp);
                UdpClient client = new UdpClient();

                string message = "";
                while (message != "end")
                {
                    Console.WriteLine("Enter message : ");
                    message = Console.ReadLine()!;
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    // socket.SendTo(data, endPoint);
                    client.Send(data, data.Length, endPoint);

                    //string responce = " ";
                    //data = new byte[1024];
                    //int bytes = 0;
                    //do
                    //{
                    //    bytes = socket.ReceiveFrom(data, ref remotePoint);
                    //    responce = Encoding.UTF8.GetString(data);
                    //    Console.WriteLine($"Server responce : {responce}");

                    //} while (socket.Available > 0);
                    string responce = " ";
                    data = client.Receive(ref remotePoint);
                    responce = Encoding.UTF8.GetString(data);
                    Console.WriteLine($"Server responce : {responce}");
                }
                client.Close();
                //socket.Shutdown(SocketShutdown.Both);
                //socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}