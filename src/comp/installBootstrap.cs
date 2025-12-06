namespace LegacyTUIComp
{
    public partial class Methods
    {
        public static void installBootstrap()
        {
            DownloadFile("https://dl.llaun.ch/legacy/bootstrap", Path.Combine(WorkspaceDir(), "bootstrap.jar"));
        }
    }
}