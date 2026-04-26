using System;
using System.IO;
using System.Text.Json;

namespace Task3_Serializer
{
    class HotelRoom
    {
        public int RoomNumber { get; set; }
        public string GuestName { get; set; }
    }

    interface ISerializer<T>
    {
        string Serialize(T item);
        T Deserialize(string data);
    }

    class JsonSerializer<T> : ISerializer<T>
    {
        public string Serialize(T item)
        {
            return JsonSerializer.Serialize(item);
        }

        public T Deserialize(string data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }
    }

    class SerializerManager<T>
    {
        private ISerializer<T> serializer;

        public SerializerManager(ISerializer<T> serializer)
        {
            this.serializer = serializer;
        }

        public void SaveToFile(T item, string filename)
        {
            string json = serializer.Serialize(item);
            File.WriteAllText(filename, json);
            Console.WriteLine($"Сохранено в файл: {filename}");
        }

        public T LoadFromFile(string filename)
        {
            string json = File.ReadAllText(filename);
            return serializer.Deserialize(json);
        }
    }
    
    class Program
    {
        static void Main()
        {
            var room = new HotelRoom { RoomNumber = 201, GuestName = "Анна Иванова" };

            var manager = new SerializerManager<HotelRoom>(new JsonSerializer<HotelRoom>());

            manager.SaveToFile(room, "room.json");

            var loaded = manager.LoadFromFile("room.json");
            Console.WriteLine($"Загружено: Номер {loaded.RoomNumber}, Гость {loaded.GuestName}");
        }
    }
}