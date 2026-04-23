using System;

namespace LoggingSystem
{
    interface IErrorLogger
    {
        void Log(string message);
    }

    interface IEventLogger
    {
        void Log(string message);
    }

    class Logger : IErrorLogger, IEventLogger
    {
        void IErrorLogger.Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ОШИБКА] {DateTime.Now}: {message}");
            Console.ResetColor();
        }

        void IEventLogger.Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[СОБЫТИЕ] {DateTime.Now}: {message}");
            Console.ResetColor();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger();

            IErrorLogger errorLogger = logger;
            IEventLogger eventLogger = logger;

            Console.WriteLine("Демонстрация логирования\n");

            errorLogger.Log("Не удалось подключиться к базе данных");
            eventLogger.Log("Пользователь успешно авторизовался");

            errorLogger.Log("Файл не найден: config.ini");
            eventLogger.Log("Данные сохранены в файл output.txt");

        }
    }
}