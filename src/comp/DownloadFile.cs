using System.Net;

namespace TermLComp
{
    public partial class Methods
    {
        public static void DownloadFile(string url, string filePath)
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] data = client.GetByteArrayAsync(url).Result;
                File.WriteAllBytes(filePath, data);
            }

            Console.WriteLine($"File downloaded: {filePath} : {url}");
        }
    }
}