namespace TermLComp.Instances
{
    public partial class Methods
    {
        public static List<List<string>> init()
        {
            string workspace = TermLComp.Methods.WorkspaceDir();
            string instancesPath = Path.Combine(workspace, "instances");

            // Создаём папку instances, если её нет
            Directory.CreateDirectory(instancesPath);

            // Получаем все подпапки и сортируем по дате изменения
            string[] directories = Directory.GetDirectories(instancesPath)
                                            .OrderByDescending(d => Directory.GetLastWriteTime(d))
                                            .Select(d => Path.GetFileName(d))
                                            .ToArray();

            // Разбиваем на страницы по 4 папки
            List<List<string>> pages = new List<List<string>>();
            int pageSize = 4;
            for (int i = 0; i < directories.Length; i += pageSize)
            {
                List<string> page = directories.Skip(i).Take(pageSize).ToList();
                pages.Add(page);
            }

            return pages;
        }
    }
}