using System;

namespace LegacyTUI
{
    class LegacyTUI
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                UI.MainMenu();
            }
            else
            {
                CLI.main(args);
            }
        }
    }
}
