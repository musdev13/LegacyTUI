using static TermLComp.UI;

namespace TermL
{
    public partial class UI
    {
        public static void Instances()
        {
            var pages = TermLComp.Instances.Methods.init();
            int curPage = 0;
            while(true){
                Console.Clear();
                showOptions($"Instances {curPage}/{pages.Count-1}",pages[curPage].ToArray(), "Back", new string[] {"Next","Prev","Create Instance"});
                char choice = getChar();

                if (choice >= '1' && choice <= '4')
                {
                    int index = choice - '1';
                    if (index < pages[curPage].Count)
                    {
                        TermLComp.Instances.UI.Instance(pages[curPage][index]);
                    }
                }

                switch (choice)
                {
                    case '0':
                        return;
                    case 'a':
                        if (curPage+1 <= pages.Count-1) curPage++;
                        break;
                    case 'b':
                        if (curPage-1 >= 0) curPage--;
                        break;
                    case 'c':
                        TermLComp.Instances.UI.createInstance();
                        pages = TermLComp.Instances.Methods.init();
                        TermLComp.Instances.UI.Instance(pages[0][0]);
                        break;
                }
            }
        }
    }
}