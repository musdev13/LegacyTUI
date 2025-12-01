namespace TermLComp.Instances
{
    public partial class UI
    {
        private static string InputInstanceName()
        {
            string? input;
            while (true)
            {
                Console.Clear();
                Console.Write("New instance name\n>_: ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) break;
                Console.WriteLine("Empty Name!\npress any key");
                TermLComp.UI.getChar();
            }
            input = input.Trim();
            while (input.Contains("  "))
            {
                input = input.Replace("  ", " ");
            }
            input = input.Replace(" ", "_");
            return input;
        }
        public static void createInstance()
        {
            string input = InputInstanceName();
            Directory.CreateDirectory(Path.Combine(TermLComp.Methods.WorkspaceDir(),input));

            

            TermLComp.UI.getChar();
        }
    }
}