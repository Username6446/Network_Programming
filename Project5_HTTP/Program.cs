using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace Project5_HTTP
{
    class Post
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
    }
    internal class Program
    {
        static string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static string file1GB = @"https://ash-speed.hetzner.com/1GB.bin";

        static async Task Main(string[] args)
        {
            // Get 
            #region Get
            //string url = "https://jsonplaceholder.typicode.com/users";
            //HttpClient httpclient = new HttpClient();

            //var response = await httpclient.GetAsync(url);

            //Console.WriteLine($"Status : {response.StatusCode}");
            //var result = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(result);

            #endregion

            #region POST
            //Post post = new Post()
            //{
            //    Title = "About SIMI",
            //    Body = "I like simi. Cool store",
            //    UserId = 523
            //};

            //string json = JsonConvert.SerializeObject(post);

            //var data = new StringContent(json);
            //HttpClient httpClient = new HttpClient();
            //var url = "https://jsonplaceholder.typicode.com/posts";

            //var response = await httpClient.PostAsync(url, data);
            //Console.WriteLine($"Status : {response.StatusCode}");

            //var res = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(res);
            #endregion

            #region Downloading
            //var url = @"https://cdn.pixabay.com/photo/2015/04/19/08/32/flower-729510_1280.jpg";
            //HttpClient httpClient = new HttpClient();
            //HttpRequestMessage request = new HttpRequestMessage()
            //{
            //    Method = HttpMethod.Get,
            //    RequestUri = new Uri(url)
            //};
            //HttpResponseMessage message =  await httpClient.SendAsync(request);
            //Console.WriteLine($"Status code : {message.StatusCode}");

            //using (FileStream fs = new FileStream(desktop + "/image.jpg", FileMode.Create))
            //{
            //    await message.Content.CopyToAsync(fs);
            //}

            DownloadFileAsync(file1GB);

            Console.ReadKey();
            #endregion
        }
        private static async void DownloadFileAsync(string address)
        {
            WebClient webClient = new WebClient();

            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            string filename = Path.GetFileName(address);
            string fullpath = Path.Combine(desktop, filename);
            await webClient.DownloadFileTaskAsync(address, fullpath);
            //Console.WriteLine($"{filename} was loaded!");
        }

        private static void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine($"Downloading .... {(float)e.BytesReceived / 1024 / 1024} Mb from" +
                $"Percentage : {e.ProgressPercentage} %");
        }


        private static void WebClient_DownloadFileCompleted(object? sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(e.Cancelled)
            {
                Console.WriteLine("File downloading canceled!");
            }
            else
            {
                Console.WriteLine("File downloaded!");
            }
        }
    }
}
