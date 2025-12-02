namespace LegacyTUIComp
{
    public partial class Methods
    {
        public static string? GetFromFile(string path, int lineNumber)
        {
            if (!File.Exists(path))
                return null;

            var lines = File.ReadAllLines(path);

            if (lineNumber < 0 || lineNumber >= lines.Length)
                return null;

            return lines[lineNumber];
        }

        public static void SetFromFile(string path, string[] lines)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllLines(path, lines);
        }
    }
}