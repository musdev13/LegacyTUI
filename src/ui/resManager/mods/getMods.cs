using System.Text.Json;
using System.Linq;

namespace LegacyTUI.resManager.mods.global
{
    public struct Mod
    {
        public string slug;
        public string title;
        public string description;
        public string author;
        public string modType;
        public string pageUrl;
    }
    public partial class Methods
    {
        public static (List<List<Mod>>, List<List<string>>) search(string instanceVersion, string instanceTag, string searchQuery)
        {
            string clearVersion = resManager.mods.Methods.clearVersion(instanceVersion);
            List<Mod> mods = new List<Mod>();
            string json = LegacyTUIComp.Methods.getJson($"https://api.modrinth.com/v2/search?query={searchQuery}&game_versions=[\"{clearVersion}\"]&loaders=[\"{instanceTag}\"]&limit=50");
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            JsonElement hits = root.GetProperty("hits");
            foreach (JsonElement hit in hits.EnumerateArray())
            {
                string slug = hit.GetProperty("slug").GetString()!;
                string title = hit.GetProperty("title").GetString()!;
                string description = hit.GetProperty("description").GetString()!;
                string author = hit.GetProperty("author").GetString()!;
                string modType = hit.GetProperty("project_type").GetString()!;
                if (modType != "mod") continue;
                mods.Add(new Mod
                {
                    slug = slug,
                    title = title,
                    description = description,
                    author = author,
                    modType = modType,
                    pageUrl = $"https://modrinth.com/mod/{slug}"
                });
            }
            int pageSize = 6;
            List<List<Mod>> pages = new List<List<Mod>>();
            for (int i = 0; i < mods.Count; i += pageSize)
            {
                pages.Add(mods.Skip(i).Take(pageSize).ToList()!);
            }
            List<List<string>> pagesPrint = new List<List<string>>();
            for (int i = 0; i < mods.Count; i += pageSize)
            {
                var page = mods.Skip(i)
                               .Take(pageSize)
                               .Select(v => $"{v.title} - {v.author}")
                               .ToList();
                pagesPrint.Add(page);
            }

            return (pages, pagesPrint);
        }
        public static void installMod(string slug, string modsFolder, string instanceVersion, string instanceTag)
        {
            string clearVersion = resManager.mods.Methods.clearVersion(instanceVersion);
            string json = LegacyTUIComp.Methods.getJson($"https://api.modrinth.com/v2/project/{slug}/version");
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            foreach (JsonElement element in root.EnumerateArray())
            {
                if (!element.GetProperty("game_versions").EnumerateArray().Any(v => v.GetString() == clearVersion)) continue;
                if (!element.GetProperty("loaders").EnumerateArray().Any(l => l.GetString() == instanceTag)) continue;

                JsonElement files = element.GetProperty("files");
                foreach (JsonElement file in files.EnumerateArray())
                {
                    if (file.GetProperty("primary").GetBoolean())
                    {
                        string url = file.GetProperty("url").GetString()!;
                        string filename = file.GetProperty("filename").GetString()!;
                        global::LegacyTUIComp.Methods.DownloadFile(url, Path.Combine(modsFolder, filename));

                        // Console.WriteLine("Installing dependencies...");
                        JsonElement dependencies = element.GetProperty("dependencies");
                        foreach (JsonElement dependency in dependencies.EnumerateArray())
                        {
                            string dependencyID = dependency.GetProperty("project_id").GetString()!;
                            installMod(dependencyID, modsFolder, instanceVersion, instanceTag);
                        }

                        return;
                    }
                }
            }
        }
    }
    public partial class UI
    {
        public static void ShowMod(Mod mod, string modsFolder, string instanceVersion, string instanceTag)
        {
            Console.Clear();
            LegacyTUIComp.UI.showOptions($"{mod.title}\n", new string[] { $"author: {mod.author}", $"description: {mod.description}", $"mod page: {mod.pageUrl}" }, "Back", new string[] { "Install" });
            char choice = LegacyTUIComp.UI.getChar();
            switch (choice)
            {
                case '0':
                    return;
                case 'a':
                    Console.Clear();
                    Console.WriteLine("Installing...");
                    Methods.installMod(mod.slug, modsFolder, instanceVersion, instanceTag);
                    break;
            }
        }
        public static void main(string instanceVersion, string instanceTag, string modsFolder)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                {
                    string title = "Modrinth Search";
                    Console.WriteLine($"{title}\n{new string('-', title.Length)}");
                }
                Console.Write("Search query: ");
                string searchQuery = Console.ReadLine()!;
                Console.WriteLine("Searching...");
                (List<List<Mod>> pages, List<List<string>> pagesPrint) = Methods.search(instanceVersion, instanceTag, searchQuery);
                int curPage = 0;
                bool innerRunning = true;
                while (innerRunning)
                {
                    Console.Clear();
                    LegacyTUIComp.UI.showOptions($"Modrinth Search {curPage}/{pages.Count - 1}", pagesPrint[curPage].ToArray(), "Back", new string[] { "Next", "Prev", "Search" });
                    char choice = LegacyTUIComp.UI.getChar();
                    if (choice >= '1' && choice <= '6')
                    {
                        int index = choice - '1';
                        ShowMod(pages[curPage][index], modsFolder, instanceVersion, instanceTag);
                    }
                    switch (choice)
                    {
                        case '0':
                            innerRunning = false;
                            running = false;
                            return;
                        case 'a':
                            if (curPage + 1 <= pagesPrint.Count - 1) curPage++;
                            break;
                        case 'b':
                            if (curPage - 1 >= 0) curPage--;
                            break;
                        case 'c':
                            innerRunning = false;
                            break;
                    }
                }
            }
        }
    }
}