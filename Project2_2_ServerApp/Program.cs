using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Project2_2_ServerApp
{
    class ChatServer
    {
        const short port = 4040;
        const string serverAddress = "127.0.0.1";

        TcpListener server;

        
        public ChatServer()
        {
            server = new TcpListener(new IPEndPoint(IPAddress.Parse(serverAddress), port));
        }

        private Dictionary<string, string> cityCodes = new Dictionary<string, string>()
        {
            { "AA", "Києва" },
            { "KA", "Києва" },
            { "AX", "Харкова" },
            { "KX", "Харкова" },
            { "BO", "Львова" },
            { "KO", "Львова" },
            { "BH", "Одеси" },
            { "KH", "Одеси" },  
            { "AE", "Дніпра" },
            { "KE", "Дніпра" },
            { "AH", "Донецька" },
            { "AM", "Луганська" },
            { "AP", "Запоріжжя" },
            { "AB", "Вінниці" },
            { "AC", "Луцька" },
            { "AT", "Івано-Франківська" },
            { "BA", "Кропивницького" },
            { "BE", "Миколаєва" },
            { "BI", "Полтави" },
            { "BK", "Рівного" },
            { "BM", "Сум" },
            { "BT", "Херсона" },
            { "BX", "Хмельницького" },
            { "CA", "Черкас" },
            { "CB", "Чернігова" },
            { "CE", "Чернівців" },
            { "CH", "Севастополя" }
        };
        public void Start()
        {
            server.Start();
            Console.WriteLine("Waiting ");
            TcpClient client = server.AcceptTcpClient();

            Console.WriteLine("Connected");
            NetworkStream ns = client.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            while (true)
            {
                try
                {
                    string? message = sr.ReadLine();
                    if (message == null)
                    {
                        break;
                    }

                    string city;
                    if (cityCodes.TryGetValue(message.ToUpper(), out city))
                    {
                        Console.WriteLine($"Машина з {city}");
                        sw.WriteLine($"Підтверджено: машина з {city}");
                    }
                    else
                    {
                        Console.WriteLine($"Невідомий код: {message}");
                        sw.WriteLine($"Помилка: код '{message}' не розпізнано");
                    }
                    sw.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                    break;
                }
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
