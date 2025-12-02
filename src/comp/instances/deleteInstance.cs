namespace LegacyTUIComp.Instances
{
    public partial class Methods
    {
        public static bool deleteInstance(string instanceName, string instancePath)
        {
            Console.Clear();
            Console.Write($"Are you sure about delete \"{instanceName}\" instance?\n>_ (y/n): ");
            char choice = LegacyTUIComp.UI.getChar();

            if (choice == 'y')
            {
                Directory.Delete(instancePath, true);
                return true;
            }
            return false;
        }
    }
}