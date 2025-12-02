namespace LegacyTUIComp.Instances
{
    public partial class Methods
    {
        public static void changeName(string instancePath)
        {
            string newName = LegacyTUIComp.Instances.UI.InputInstanceName();
            string instancesPath = Directory.GetParent(instancePath)!.FullName;

            // Console.WriteLine($"Old Path: {instancePath}\nNew Path: {Path.Combine(instancesPath,newName)}");
            // LegacyTUIComp.UI.getChar();
            Directory.Move(instancePath,Path.Combine(instancesPath,newName));
        }
    }
}