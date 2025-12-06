using System.CommandLine;

namespace LegacyTUI
{
    class CLI
    {
        public static void main(string[] args)
        {
            var updateOption = new Option<bool>("--update", "Install/Update Legacy Launcher");
            updateOption.AddAlias("-u");

            var root = new RootCommand("LegacyTUI"){
                updateOption
            };

            root.SetHandler((bool update) =>
            {
                if (update)
                {
                    LegacyTUIComp.Methods.installBootstrap();
                }
            }, updateOption);

            root.Invoke(args);
        }
    }
}