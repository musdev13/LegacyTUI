using System.Text.RegularExpressions;

namespace LegacyTUI.resManager.mods
{
    public partial class Methods
    {
        public static (List<List<string>>, string) initModsFolder(string instancePath)
        {
            string modsFolder = Path.Combine(instancePath, "mods");
            Directory.CreateDirectory(modsFolder);

            string?[] jarFiles = Directory.GetFiles(modsFolder, "*.jar*")
                             .Select(Path.GetFileName)
                             .ToArray();

            List<List<string>> pages = new List<List<string>>();
            int pageSize = 6;

            for (int i = 0; i < jarFiles.Length; i += pageSize)
            {
                pages.Add(jarFiles.Skip(i).Take(pageSize).ToList()!);
            }

            if (pages.Count == 0)
                pages.Add(new List<string>());

            return (pages, modsFolder);
        }

        public static string clearVersion(string instanceVersion)
        {
            string version = "";
            Match m = Regex.Match(instanceVersion, @"\d+\.\d+\.\d+");
            if (m.Success)
            {
                version = m.Value;
            }
            return version;
        }
    }
    public partial class UI
    {
        public static void main(string instancePath, string instanceVersion, string instanceTag)
        {
            Console.Clear();
            Console.WriteLine("Getting Mods...");
            (var pages, string modsFolder) = Methods.initModsFolder(instancePath);
            int curPage = 0;

            while (true)
            {
                Console.Clear();
                LegacyTUIComp.UI.showOptions(
                    $"Mods {curPage}/{pages.Count - 1}",
                    pages[curPage].ToArray(),
                    "Back",
                    new string[] { "Next", "Prev", "Get Mods", "Mods folder" }
                );
                char choice = LegacyTUIComp.UI.getChar();

                if (choice >= '1' && choice <= '6')
                {
                    int index = choice - '1';
                    if (index < pages[curPage].Count)
                    {
                        mods.UI.mod(pages[curPage][index], modsFolder);
                        (pages, modsFolder) = Methods.initModsFolder(instancePath);
                    }
                }

                switch (choice)
                {
                    case '0':
                        return;
                    case 'a':
                        if (curPage + 1 <= pages.Count - 1) curPage++;
                        break;
                    case 'b':
                        if (curPage - 1 >= 0) curPage--;
                        break;
                    case 'c':
                        mods.global.UI.main(instanceVersion, instanceTag, modsFolder);
                        (pages, modsFolder) = Methods.initModsFolder(instancePath);
                        break;
                }
            }
        }
    }
}