using System.Net.NetworkInformation;
using System.Text.Json;
using LegacyTUIComp.Logic;
using LegacyTUIComp.Shared;

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

        public static (string version, string tag) getInstanceVersion()
        {
            bool running = true;
            string version = "";
            string tag = "";
            while (running)
            {
                Console.Clear();
                LegacyTUIComp.UI.showOptions("Select Loader", new string[] { "Vanilla", "Fabric", "Forge" }, "");
                char choice = LegacyTUIComp.UI.getChar();
                switch (choice)
                {
                    case '1': // vanilla
                        version = SelectVersion(VersionFetcher.GetVanillaVersions(), "Vanilla");
                        tag = "vanilla";
                        running = false;
                        break;
                    case '2': // Fabric
                        version = SelectVersion(VersionFetcher.GetFabricVersions(), "Fabric");
                        tag = "fabric";
                        running = false;
                        break;
                    case '3': // Forge
                        version = SelectVersion(VersionFetcher.GetForgeVersions(), "Forge");
                        tag = "forge";
                        running = false;
                        break;
                }
            }

            return (version, tag);
        }

        private static string SelectVersion(List<McVersion> versionList, string typeName)
        {
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
                LegacyTUIComp.UI.showOptions($"{typeName} versions {curPage}/{pages.Count - 1}", pagesPrint[curPage].ToArray(), "", new string[] { "Next", "Prev" });
                char choice = LegacyTUIComp.UI.getChar();

                if (choice >= '1' && choice <= '6')
                {
                    int index = choice - '1';
                    if (index < pages[curPage].Count)
                    {
                        versionstr = pages[curPage][index].Id;
                        running = false;
                    }
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

        public static void createInstance()
        {
            string input = InputInstanceName();
            var version = getInstanceVersion();
            string workspaceDir = LegacyTUIComp.Methods.WorkspaceDir();
            string instancesPath = Path.Combine(workspaceDir, "instances");
            string instancePath = Path.Combine(instancesPath, input);

            Directory.CreateDirectory(instancePath);
            LegacyTUIComp.Methods.SetFromFile(Path.Combine(instancePath, "LegacyTUI_data"), new string[] { version.version, version.tag });
            LegacyTUIComp.Methods.updateProfiles(workspaceDir, instancePath);
        }
    }
}