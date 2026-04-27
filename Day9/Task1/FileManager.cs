using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class FileManager
    {
        public void CreateFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void CopyFile(string source, string destination)
        {
            File.Copy(source, destination, true);
        }

        public void MoveFile(string source, string destination)
        {
            File.Move(source, destination);
        }

        public void RenameFile(string oldPath, string newPath)
        {
            File.Move(oldPath, newPath);
        }

        public void DeleteFileSafe(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                else
                    Console.WriteLine($"Файл {path} не существует");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public bool CompareSize(string file1, string file2)
        {
            return new FileInfo(file1).Length == new FileInfo(file2).Length;
        }

        public void DeleteFilesByPattern(string directory, string pattern)
        {
            var files = Directory.GetFiles(directory, pattern);
            foreach (var file in files)
            {
                File.Delete(file);
                Console.WriteLine($"Удален: {file}");
            }
        }

        public List<string> ListFiles(string directory)
        {
            return Directory.GetFiles(directory).ToList();
        }

        public void SetReadOnly(string path, bool readOnly)
        {
            new FileInfo(path).IsReadOnly = readOnly;
        }

        public void TryWriteToReadOnly(string path, string content)
        {
            try
            {
                File.WriteAllText(path, content);
                Console.WriteLine("Запись успешна");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Не удалось записать: файл защищен от записи");
            }
        }
    }
}
