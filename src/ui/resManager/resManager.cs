namespace LegacyTUI.resManager
{
    public partial class UI
    {
        public static void main(string instancePath, string instanceVersion, string instanceTag)
        {
            while (true)
            {
                Console.Clear();
                LegacyTUIComp.UI.showOptions
                (
                    "Resource Manager",
                    new string[]
                    {
                        "Mods",
                        "Resource Packs",
                    },
                    "Back"
                );
                char choice = LegacyTUIComp.UI.getChar();

                switch (choice)
                {
                    case '0':
                        return;
                    case '1':
                        resManager.mods.UI.main(instancePath, instanceVersion, instanceTag);
                        break;
                    case '2':
                        global::LegacyTUI.UI.itHasntBeenDoneYet();
                        break;
                }
            }
        }
    }
}