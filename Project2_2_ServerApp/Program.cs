using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Project2_2_ServerApp
{
    class ChatServer
    {
        const short port = 4040;
        const string JOIN_CMD = "$<join>";
        const string LEAVE_CMD = "$<leave>";
        UdpClient udpClient;
        IPEndPoint remoteEP = null;
        List<IPEndPoint> members;
        
        public ChatServer()
        {
            udpClient = new UdpClient(port);
            members = new List<IPEndPoint>();
        }
        private void AddMember(IPEndPoint member)
        {
            members.Add(remoteEP);
            Console.WriteLine("Member was added!");
        }
        private void DeleteMember(IPEndPoint member)
        {
            members.Remove(remoteEP);
            Console.WriteLine("Member was leave!");
        }
        public void Start()
        {
            while (true)
            {
                byte[] data = udpClient.Receive(ref remoteEP);

                string[] res = Encoding.UTF8.GetString(data).Split('!');
                string nickname = res[0]; 
                string message = res[1]; 
                Console.WriteLine($"NickName : {nickname} Got message : {message}" +
            $"From : {remoteEP} Time : {DateTime.Now.ToShortTimeString()}");
                switch (message)
                {
                    case JOIN_CMD:
                        AddMember(remoteEP);
                        break;
                    case LEAVE_CMD:
                        DeleteMember(remoteEP);
                        break;
                    default:
                        SendToAllMembers(data);
                        break;
                }

            }
        }
        private void SendToAllMembers(byte[] data)
        {
            foreach (IPEndPoint member in members)
            {
                udpClient.SendAsync(data, data.Length, member);
            }
        }
    }
    internal class Program
    {
        
        
        static void Main(string[] args)
        {
            
            ChatServer server = new ChatServer();
            server.Start();
            
        }
    }
}
