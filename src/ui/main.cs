using static TermLComp.UI;

namespace TermL
{
    public partial class UI
    {
        public static void MainMenu()
        {
            while(true){
                Console.Clear();
                showOptions("TermLegacy",new string[] {"Instances","Install/Update Legacy Launcher"},"Exit");
                char choice = getChar();

                switch (choice)
                {
                    case '2':
                        TermLComp.Methods.installBootstrap();
                        break;
                    case '1':
                        Instances();
                        break;
                    case '0':
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}