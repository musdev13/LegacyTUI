namespace TermLComp
{
    public partial class Methods
    {
        public static string getJson(string url)
        {
            using HttpClient client = new HttpClient();
            return client.GetStringAsync(url).Result; // <-- СИНХРОННО
        }
    }
}