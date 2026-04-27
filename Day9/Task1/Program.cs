
namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = @"C:\Temp\FileManagerTest";
            Directory.CreateDirectory(basePath);

            string fileName = "ivanov.ii";
            string filePath = Path.Combine(basePath, fileName);

            var fileManager = new FileManager();
            var infoProvider = new FileInfoProvider();

            string content = "Hello, this is a test file!";
            fileManager.CreateFile(filePath, content);
            string readContent = fileManager.ReadFile(filePath);
            Console.WriteLine($"Прочитано: {readContent}");

            if (fileManager.FileExists(filePath))
                Console.WriteLine("Файл существует");

            var info = infoProvider.GetFileInfo(filePath);
            Console.WriteLine($"Размер: {info.Size} байт");
            Console.WriteLine($"Создан: {info.CreationTime}");
            Console.WriteLine($"Изменен: {info.LastModified}");

            string copyPath = Path.Combine(basePath, "ivanov_copy.ii");
            fileManager.CopyFile(filePath, copyPath);
            Console.WriteLine($"Копия существует: {fileManager.FileExists(copyPath)}");

            string newDir = Path.Combine(basePath, "NewFolder");
            Directory.CreateDirectory(newDir);
            string movedPath = Path.Combine(newDir, fileName);
            fileManager.MoveFile(copyPath, movedPath);
            Console.WriteLine($"Файл перемещен в {movedPath}");

            string renamedPath = Path.Combine(newDir, "familiya.io");
            fileManager.RenameFile(movedPath, renamedPath);

            fileManager.DeleteFileSafe(Path.Combine(basePath, "nonexistent.txt"));

            bool areEqual = fileManager.CompareSize(filePath, renamedPath);
            Console.WriteLine($"Файлы одинакового размера: {areEqual}");

            fileManager.DeleteFilesByPattern(basePath, "*.ii");

            var files = fileManager.ListFiles(basePath);
            foreach (var f in files) Console.WriteLine(f);

            string testFile = Path.Combine(basePath, "test.txt");
            File.WriteAllText(testFile, "test");
            fileManager.SetReadOnly(testFile, true);
            fileManager.TryWriteToReadOnly(testFile, "new content");
            fileManager.SetReadOnly(testFile, false);

            var permissions = infoProvider.CheckPermissions(testFile);
            Console.WriteLine($"Чтение: {permissions.CanRead}, Запись: {permissions.CanWrite}, Выполнение: {permissions.CanExecute}");

            Console.WriteLine("\nГотово!");
        }
    }
}