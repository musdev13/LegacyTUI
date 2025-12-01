namespace TermLComp
{
    public partial class Methods{
        public static void installBootstrap()
        {
            Console.Clear();
            DownloadFile("https://dl.llaun.ch/legacy/bootstrap",Path.Combine(WorkspaceDir(),"bootstrap.jar"));
        }
    }
}