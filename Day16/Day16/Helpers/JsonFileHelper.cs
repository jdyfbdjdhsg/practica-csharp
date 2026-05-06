using Newtonsoft.Json;
using System.IO;

namespace Day16.Helpers
{
    public static class JsonHelper
    {
        private static readonly object _lock = new object();

        public static T ReadFromFile<T>(string filePath)
        {
            lock (_lock)
            {
                if (!File.Exists(filePath))
                {
                    return Activator.CreateInstance<T>();
                }

                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json) ?? Activator.CreateInstance<T>();
            }
        }

        public static void WriteToFile<T>(string filePath, T data)
        {
            lock (_lock)
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(filePath, json);
            }
        }
    }
}