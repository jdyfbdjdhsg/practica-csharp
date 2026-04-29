namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Команда\n");

            Printer printer = new Printer();
            PrintManager manager = new PrintManager();

            ICommand printCommand = new PrintDocumentCommand(printer);
            ICommand cancelCommand = new CancelPrintCommand(printer);

            manager.SetCommand(printCommand);
            manager.ExecuteCommand();

            manager.SetCommand(cancelCommand);
            manager.ExecuteCommand();
        }
    }
}