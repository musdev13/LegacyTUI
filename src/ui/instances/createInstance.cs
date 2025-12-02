using System.Net.NetworkInformation;
using System.Text.Json;

namespace LegacyTUIComp.Instances
{
    public partial class UI
    {
        private static string InputInstanceName()
        {
            string? input;
            while (true)
            {
                Console.Clear();
                Console.Write("New instance name\n>_: ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) break;
                Console.WriteLine("Empty Name!\npress any key");
                LegacyTUIComp.UI.getChar();
            }
            input = input.Trim();
            while (input.Contains("  "))
            {
                input = input.Replace("  ", " ");
            }
            input = input.Replace(" ", "_");
            return input;
        }
        public struct McVersion
        {
            public string Id { get; set; }
            public string Type { get; set; }
        }
        private static string getVanilaVersion()
        {
            Console.Clear();
            Console.WriteLine("Fetching mojang version list");
            string json = LegacyTUIComp.Methods.getJson("https://launchermeta.mojang.com/mc/game/version_manifest.json");
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            JsonElement versions = root.GetProperty("versions");
            List<McVersion> versionList = new List<McVersion>();
            foreach (JsonElement version in versions.EnumerateArray())
            {
                versionList.Add(new McVersion
                {
                    Id = version.GetProperty("id").GetString()!,
                    Type = version.GetProperty("type").GetString()!
                });
            }

            int pageSize = 6;
            List<List<McVersion>> pages = new List<List<McVersion>>();
            for (int i = 0; i < versionList.Count; i += pageSize)
            {
                pages.Add(versionList.Skip(i).Take(pageSize).ToList());
            }
            List<List<string>> pagesPrint = new List<List<string>>();
            for (int i = 0; i < versionList.Count; i += pageSize)
            {
                var page = versionList.Skip(i)
                                      .Take(pageSize)
                                      .Select(v => $"{v.Id} - {v.Type}")
                                      .ToList();
                pagesPrint.Add(page);
            }

            // foreach (JsonElement version in versions.EnumerateArray())
            // {
            //     string? id = version.GetProperty("id").GetString();
            //     string? type = version.GetProperty("type").GetString();
            //     Console.WriteLine($"{id} ({type})");
            // }

            bool running = true;
            string versionstr="";
            int curPage = 0;

            while (running){
                Console.Clear();
                LegacyTUIComp.UI.showOptions($"Vanila versions {curPage}/{pages.Count-1}",pagesPrint[curPage].ToArray(),"",new string[] {"Next","Prev"});
                char choice = LegacyTUIComp.UI.getChar();

                if (choice >= '1' && choice <= '6')
                {
                    int index = choice - '1';
                    versionstr = pages[curPage][index].Id;
                    running = false;
                    continue;
                }
                switch (choice)
                {
                    case 'a':
                        if (curPage+1 <= pagesPrint.Count-1) curPage++;
                        break;
                    case 'b':
                        if (curPage-1 >= 0) curPage--;
                        break;
                }
            }
            return versionstr;
        }
        public static string getInstanceVersion()
        {
            bool running = true;
            string version="";
            while (running){
                Console.Clear();
                LegacyTUIComp.UI.showOptions("Select Loader", new string[] {"Vanila","Fabric","Forge"}, "");
                char choice = LegacyTUIComp.UI.getChar();
                switch (choice)
                {
                    case '1': // vanila
                        version = getVanilaVersion();
                        running = false;
                        break;
                    case '2': // Fabric
                        version = "";
                        running = false;
                        break;
                    case '3': // Forge
                        version = "";
                        running = false;
                        break;
                }
            }

            return version;
        }
        public static void createInstance()
        {
            string input = InputInstanceName();
            string version = getInstanceVersion();
            string instancesPath = Path.Combine(LegacyTUIComp.Methods.WorkspaceDir(),"instances");
            string instancePath = Path.Combine(instancesPath,input);

            Directory.CreateDirectory(instancePath);
            LegacyTUIComp.Methods.SetFromFile(Path.Combine(instancePath,"LegacyTUI_data"),new string[] {version});

            // LegacyTUIComp.UI.getChar();
        }
    }
}