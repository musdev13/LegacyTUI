using System.Net.NetworkInformation;
using System.Text.Json;

namespace LegacyTUIComp.Instances
{
    public partial class UI
    {
        public static string InputInstanceName()
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
            string versionstr = "";
            int curPage = 0;

            while (running)
            {
                Console.Clear();
                LegacyTUIComp.UI.showOptions($"Vanila versions {curPage}/{pages.Count - 1}", pagesPrint[curPage].ToArray(), "", new string[] { "Next", "Prev" });
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
                        if (curPage + 1 <= pagesPrint.Count - 1) curPage++;
                        break;
                    case 'b':
                        if (curPage - 1 >= 0) curPage--;
                        break;
                }
            }
            return versionstr;
        }

        private static string getFabricVersion()
        {
            Console.Clear();
            Console.WriteLine("Fetching LegacyLauncher version list");
            string json = LegacyTUIComp.Methods.getJson("https://repo.llaun.ch/versions/versions.json");
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            JsonElement versions = root.GetProperty("versions");
            List<McVersion> versionList = new List<McVersion>();
            foreach (JsonElement version in versions.EnumerateArray())
            {
                if (version.TryGetProperty("id", out JsonElement idElem) &&
                    version.TryGetProperty("type", out JsonElement typeElem))
                {
                    string id = idElem.GetString()!;
                    string type = typeElem.GetString()!;

                    if (!string.IsNullOrEmpty(id) && id.Contains("Fabric", StringComparison.OrdinalIgnoreCase))
                    {
                        versionList.Add(new McVersion
                        {
                            Id = id,
                            Type = type
                        });
                    }
                }
                else
                {
                    Console.WriteLine("Skipped version: missing id or type");
                }
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

            bool running = true;
            string versionstr = "";
            int curPage = 0;

            while (running)
            {
                Console.Clear();
                LegacyTUIComp.UI.showOptions($"Fabric versions {curPage}/{pages.Count - 1}", pagesPrint[curPage].ToArray(), "", new string[] { "Next", "Prev" });
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
                        if (curPage + 1 <= pagesPrint.Count - 1) curPage++;
                        break;
                    case 'b':
                        if (curPage - 1 >= 0) curPage--;
                        break;
                }
            }
            return versionstr;
        }

        private static string getForgeVersion()
        {
            Console.Clear();
            Console.WriteLine("Fetching LegacyLauncher version list");
            string json = LegacyTUIComp.Methods.getJson("https://repo.llaun.ch/versions/versions.json");
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            JsonElement versions = root.GetProperty("versions");
            List<McVersion> versionList = new List<McVersion>();
            foreach (JsonElement version in versions.EnumerateArray())
            {
                if (version.TryGetProperty("id", out JsonElement idElem) &&
                    version.TryGetProperty("type", out JsonElement typeElem))
                {
                    string id = idElem.GetString()!;
                    string type = typeElem.GetString()!;

                    if (!string.IsNullOrEmpty(id) && id.Contains("Forge", StringComparison.OrdinalIgnoreCase))
                    {
                        versionList.Add(new McVersion
                        {
                            Id = id,
                            Type = type
                        });
                    }
                }
                else
                {
                    Console.WriteLine("Skipped version: missing id or type");
                }
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

            bool running = true;
            string versionstr = "";
            int curPage = 0;

            while (running)
            {
                Console.Clear();
                LegacyTUIComp.UI.showOptions($"Forge versions {curPage}/{pages.Count - 1}", pagesPrint[curPage].ToArray(), "", new string[] { "Next", "Prev" });
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
                        if (curPage + 1 <= pagesPrint.Count - 1) curPage++;
                        break;
                    case 'b':
                        if (curPage - 1 >= 0) curPage--;
                        break;
                }
            }
            return versionstr;
        }
        public static (string version, string tag) getInstanceVersion()
        {
            bool running = true;
            string version = "";
            string tag = "";
            while (running)
            {
                Console.Clear();
                LegacyTUIComp.UI.showOptions("Select Loader", new string[] { "Vanila", "Fabric", "Forge" }, "");
                char choice = LegacyTUIComp.UI.getChar();
                switch (choice)
                {
                    case '1': // vanila
                        version = getVanilaVersion();
                        tag = "vanila";
                        running = false;
                        break;
                    case '2': // Fabric
                        version = getFabricVersion();
                        tag = "fabric";
                        running = false;
                        break;
                    case '3': // Forge
                        version = getForgeVersion();
                        tag = "forge";
                        running = false;
                        break;
                }
            }

            return (version, tag);
        }
        public static void createInstance()
        {
            string input = InputInstanceName();
            var version = getInstanceVersion();
            string workspaceDir = LegacyTUIComp.Methods.WorkspaceDir();
            string instancesPath = Path.Combine(workspaceDir, "instances");
            string instancePath = Path.Combine(instancesPath, input);

            Directory.CreateDirectory(instancePath);
            LegacyTUIComp.Methods.SetFromFile(Path.Combine(instancePath, "LegacyTUI_data"), new string[] { version.version, version.tag });
            // LegacyTUIComp.UI.getChar();
            LegacyTUIComp.Methods.updateProfiles(workspaceDir, instancePath);
        }
    }
}