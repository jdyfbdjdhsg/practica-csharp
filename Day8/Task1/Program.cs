using System;
using System.Collections;

namespace Task1_Hashtable
{
    

    class HotelRoom
    {
        public int RoomNumber { get; set; }
        public string GuestName { get; set; }
    }

    class HotelReservationSystem
    {
        private Hashtable rooms = new Hashtable();

        public void AddRoom(int roomNumber, string guestName)
        {
            rooms[roomNumber] = new HotelRoom { RoomNumber = roomNumber, GuestName = guestName };
        }

        public void RemoveRoom(int roomNumber)
        {
            rooms.Remove(roomNumber);
        }

        public HotelRoom FindRoom(int roomNumber)
        {
            return (HotelRoom)rooms[roomNumber];
        }

        public void DisplayAllRooms()
        {
            foreach (DictionaryEntry entry in rooms)
            {
                HotelRoom room = (HotelRoom)entry.Value;
                Console.WriteLine($"Номер: {room.RoomNumber}, Гость: {room.GuestName}");
            }
        }
    }
    
    class Program
    {
        static void Main()
        {
            var hotel = new HotelReservationSystem();

            hotel.AddRoom(101, "Иван Петров");
            hotel.AddRoom(102, "Мария Сидорова");
            hotel.AddRoom(103, "Алексей Иванов");

            hotel.DisplayAllRooms();

            var room = hotel.FindRoom(102);
            if (room != null)
                Console.WriteLine($"Найден номер {room.RoomNumber}: {room.GuestName}");

            hotel.RemoveRoom(103);
            hotel.DisplayAllRooms();
        }
    }
}