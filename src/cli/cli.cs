using System.CommandLine;
using System.CommandLine.Invocation;
using LegacyTUIComp.Logic;
using LegacyTUIComp.Shared;

namespace LegacyTUI
{
    class CLI
    {
        public static void main(string[] args)
        {
            var updateOption = new Option<bool>(new[] { "--update", "-u" }, "Install/Update Legacy Launcher");

            var instanceOption = new Option<string>(new[] { "--instance", "-i" }, "Instance Name");
            var newInstanceOption = new Option<bool>(new[] { "--new", "-n" }, "Create new instance");
            var versionOption = new Option<string>(new[] { "--game-version", "-v" }, "Instance Version");
            var launcherOption = new Option<bool>(new[] { "--launcher", "-l" }, "Launch Legacy Launcher with instance");
            var typeOption = new Option<string>(new[] { "--type", "-t" }, "Version Type (fabric, forge, vanilla)");
            var pageOption = new Option<int>(new[] { "--page", "-p" }, "Page Number (0-based)");
            var deleteOption = new Option<string>(new[] { "--delete", "-D" }, "Delete Instance or Mod") { Arity = ArgumentArity.ZeroOrOne };
            var renameOption = new Option<string>(new[] { "--rename", "-r" }, "Rename Instance to");
            var updateProfilesOption = new Option<bool>("--update-profiles", "Update Profiles");

            var folderOption = new Option<bool>(new[] { "--folder", "-f" }, "Open Instance or Mods Folder");

            var modsOption = new Option<bool>(new[] { "--mods", "-m" }, "Mods Management");
            var globalOption = new Option<bool>(new[] { "--global", "-g" }, "Global Search (Modrinth)");
            var searchOption = new Option<string>(new[] { "--search", "-s" }, "Search Query");
            var installOption = new Option<string>("--install", "Install Mod Slug");
            var disableOption = new Option<string>(new[] { "--disable", "-d" }, "Disable Mod");
            var enableOption = new Option<string>(new[] { "--enable", "-e" }, "Enable Mod");

            var root = new RootCommand("legacytui");
            root.Name = "legacytui";
            root.AddOption(updateOption);
            root.AddOption(instanceOption);
            root.AddOption(newInstanceOption);
            root.AddOption(versionOption);
            root.AddOption(launcherOption);
            root.AddOption(typeOption);
            root.AddOption(pageOption);
            root.AddOption(deleteOption);
            root.AddOption(renameOption);
            root.AddOption(updateProfilesOption);
            root.AddOption(modsOption);
            root.AddOption(globalOption);
            root.AddOption(searchOption);
            root.AddOption(installOption);
            root.AddOption(disableOption);
            root.AddOption(enableOption);
            root.AddOption(folderOption);

            root.SetHandler((InvocationContext context) =>
            {
                var update = context.ParseResult.GetValueForOption(updateOption);
                var instanceName = context.ParseResult.GetValueForOption(instanceOption);
                var isNew = context.ParseResult.GetValueForOption(newInstanceOption);
                var version = context.ParseResult.GetValueForOption(versionOption);
                var launchLauncher = context.ParseResult.GetValueForOption(launcherOption);
                var type = context.ParseResult.GetValueForOption(typeOption);
                var page = context.ParseResult.GetValueForOption(pageOption);

                var deleteArg = context.ParseResult.GetValueForOption(deleteOption);
                // Check if flag was present regardless of value (for boolean-like usage)
                var deleteFlagPresent = context.ParseResult.FindResultFor(deleteOption) != null;

                var renameTo = context.ParseResult.GetValueForOption(renameOption);
                var updateProfiles = context.ParseResult.GetValueForOption(updateProfilesOption);
                var isMods = context.ParseResult.GetValueForOption(modsOption);
                var isGlobal = context.ParseResult.GetValueForOption(globalOption);
                var searchQuery = context.ParseResult.GetValueForOption(searchOption);
                var installSlug = context.ParseResult.GetValueForOption(installOption);
                var disableMod = context.ParseResult.GetValueForOption(disableOption);
                var enableMod = context.ParseResult.GetValueForOption(enableOption);
                var isFolder = context.ParseResult.GetValueForOption(folderOption);

                try
                {
                    if (update)
                    {
                        LegacyTUIComp.Methods.installBootstrap();
                        return;
                    }

                    // Version Listing Logic: -v "" -t "type"
                    if (version != null && version == "" && !string.IsNullOrEmpty(type))
                    {
                        List<McVersion> versions = new List<McVersion>();
                        if (type.Contains("fabric", StringComparison.OrdinalIgnoreCase)) versions = VersionFetcher.GetFabricVersions();
                        else if (type.Contains("forge", StringComparison.OrdinalIgnoreCase)) versions = VersionFetcher.GetForgeVersions();
                        else if (type.Contains("vanilla", StringComparison.OrdinalIgnoreCase)) versions = VersionFetcher.GetVanillaVersions();
                        else
                        {
                            Console.WriteLine("Invalid type. Use fabric, forge, or vanilla.");
                            return;
                        }

                        int pageSize = 10; // Or whatever default
                        int totalPages = (int)Math.Ceiling(versions.Count / (double)pageSize);
                        if (page < 0) page = 0;
                        if (page >= totalPages) page = totalPages - 1;

                        var pageContent = versions.Skip(page * pageSize).Take(pageSize).ToList();
                        Console.WriteLine($"Versions ({type}) Page {page}/{totalPages - 1}:");
                        foreach (var v in pageContent)
                        {
                            Console.WriteLine($"{v.Id} - {v.Type}");
                        }
                        return;
                    }

                    // Mods Management Logic
                    if (isMods)
                    {
                        if (string.IsNullOrEmpty(instanceName))
                        {
                            Console.WriteLine("Error: -i (instance) is required for mods management.");
                            return;
                        }

                        if (isFolder)
                        {
                            string workspaceDir = LegacyTUIComp.Methods.WorkspaceDir();
                            string modsPath = Path.Combine(workspaceDir, "instances", instanceName, "mods");
                            if (Directory.Exists(modsPath))
                            {
                                Console.WriteLine($"Opening mod folder: {modsPath}");
                                LegacyTUIComp.Methods.OpenPath(modsPath);
                            }
                            else
                            {
                                Console.WriteLine($"Mod folder not found at: {modsPath}");
                            }
                            return;
                        }

                        if (isGlobal || !string.IsNullOrEmpty(searchQuery) || (isGlobal && searchQuery == null))
                        {
                            string query = searchQuery ?? "";
                            if (!isGlobal && !string.IsNullOrEmpty(searchQuery)) isGlobal = true;

                            if (isGlobal)
                            {
                                Console.WriteLine($"Searching Modrinth for '{query}'...");
                                try
                                {
                                    var (foundMods, printLines) = ModLogic.SearchMods(instanceName, query);
                                    foreach (var line in printLines)
                                    {
                                        Console.WriteLine(line);
                                    }
                                    if (!string.IsNullOrEmpty(installSlug))
                                    {
                                        ModLogic.InstallMod(instanceName, installSlug);
                                        Console.WriteLine($"Installed {installSlug}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error searching/installing: {ex.Message}");
                                }
                                return;
                            }
                        }

                        if (!string.IsNullOrEmpty(installSlug))
                        {
                            ModLogic.InstallMod(instanceName, installSlug);
                            Console.WriteLine($"Installed {installSlug}");
                            return;
                        }

                        if (!string.IsNullOrEmpty(disableMod))
                        {
                            ModLogic.DisableMod(instanceName, disableMod);
                            Console.WriteLine($"Disabled {disableMod}");
                            return;
                        }

                        if (!string.IsNullOrEmpty(enableMod))
                        {
                            ModLogic.EnableMod(instanceName, enableMod);
                            Console.WriteLine($"Enabled {enableMod}");
                            return;
                        }

                        if (deleteFlagPresent)
                        {
                            if (!string.IsNullOrEmpty(deleteArg))
                            {
                                ModLogic.DeleteMod(instanceName, deleteArg);
                                Console.WriteLine($"Deleted mod {deleteArg}");
                            }
                            else
                            {
                                Console.WriteLine("Error: To delete a mod, please specify the mod filename: -D \"filename\"");
                            }
                            return;
                        }

                        // List Mods
                        var mods = ModLogic.ListMods(instanceName);
                        Console.WriteLine($"Mods in {instanceName}:");
                        foreach (var m in mods) Console.WriteLine(m);
                        return;
                    }

                    // Instance Logic
                    if (!string.IsNullOrEmpty(instanceName))
                    {
                        if (isNew)
                        {
                            if (string.IsNullOrEmpty(version))
                            {
                                Console.WriteLine("Error: -v (version) is required for creation.");
                                return;
                            }
                            string tag = "vanilla";
                            if (version.Contains("Fabric", StringComparison.OrdinalIgnoreCase)) tag = "fabric";
                            else if (version.Contains("Forge", StringComparison.OrdinalIgnoreCase)) tag = "forge";
                            else if (version.Contains("Quilt", StringComparison.OrdinalIgnoreCase)) tag = "quilt";

                            InstanceLogic.CreateInstance(instanceName, version, tag);
                            Console.WriteLine($"Instance {instanceName} created.");
                            return;
                        }

                        if (isFolder)
                        {
                            string workspaceDir = LegacyTUIComp.Methods.WorkspaceDir();
                            string instancePath = Path.Combine(workspaceDir, "instances", instanceName);
                            if (Directory.Exists(instancePath))
                            {
                                Console.WriteLine($"Opening instance folder: {instancePath}");
                                LegacyTUIComp.Methods.OpenPath(instancePath);
                            }
                            else
                            {
                                Console.WriteLine($"Instance folder not found at: {instancePath}");
                            }
                            return;
                        }

                        if (!string.IsNullOrEmpty(renameTo))
                        {
                            InstanceLogic.RenameInstance(instanceName, renameTo);
                            Console.WriteLine($"Renamed {instanceName} to {renameTo}.");
                            return;
                        }

                        if (updateProfiles)
                        {
                            InstanceLogic.UpdateProfiles(instanceName);
                            Console.WriteLine("Profiles updated.");
                            return;
                        }

                        if (deleteFlagPresent)
                        {
                            InstanceLogic.DeleteInstance(instanceName);
                            Console.WriteLine($"Instance {instanceName} deleted.");
                            return;
                        }

                        // Launch
                        Console.WriteLine($"Launching {instanceName}...");
                        InstanceLogic.LaunchInstance(instanceName, !launchLauncher);
                        return;
                    }

                    Console.WriteLine("No valid command provided. interactive mode?");
                    UI.MainMenu();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            });

            root.Invoke(args);
        }
    }
}