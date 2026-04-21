using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Project1_DZ_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Dictionary<string, string> botMemory = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "привіт", "Привіт! Чим можу допомогти?" },
                { "як справи", "Все чудово, працюю на всі 100%!" },
                { "що робиш", "Чекаю на твої запитання." },
                { "бувай", "До зустрічі! Заходь ще." }
            };

            int port = 4040;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener server = new TcpListener(ip, port);

            try
            {
                server.Start();
                Console.WriteLine($"Сервер запущено на {ip}:{port}. Очікування клієнта...");

                while (true)
                {
                    using TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Клієнт підключився!");
                    using NetworkStream stream = client.GetStream();

                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        string clientMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                        Console.WriteLine($"Отримано: {clientMessage}");

                        string responseMessage;

                        if (botMemory.TryGetValue(clientMessage, out string? answer))
                        {
                            responseMessage = answer;
                        }
                        else
                        {
                            responseMessage = "Я поки не знаю відповіді на цю фразу.";
                        }

                        byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);
                        stream.Write(responseData, 0, responseData.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка сервера: {ex.Message}");
            }
            finally
            {
                server.Stop();
            }
        }
    }
}