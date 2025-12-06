using System.Text.Json;
using LegacyTUIComp;
using LegacyTUIComp.Instances;
using LegacyTUIComp.Shared;

namespace LegacyTUIComp.Logic
{
    public class VersionFetcher
    {
        public static List<McVersion> GetVanillaVersions()
        {
            string json = Methods.getJson("https://launchermeta.mojang.com/mc/game/version_manifest.json");
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
            return versionList;
        }

        public static List<McVersion> GetFabricVersions()
        {
            return GetLegacyLauncherVersions("Fabric");
        }

        public static List<McVersion> GetForgeVersions()
        {
            return GetLegacyLauncherVersions("Forge");
        }

        private static List<McVersion> GetLegacyLauncherVersions(string filterType)
        {
            string json = Methods.getJson("https://repo.llaun.ch/versions/versions.json");
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

                    if (!string.IsNullOrEmpty(id) && id.Contains(filterType, StringComparison.OrdinalIgnoreCase))
                    {
                        versionList.Add(new McVersion
                        {
                            Id = id,
                            Type = type
                        });
                    }
                }
            }
            return versionList;
        }
    }
}

