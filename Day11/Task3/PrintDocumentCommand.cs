using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    public class PrintDocumentCommand : ICommand
    {
        private Printer _printer;

        public PrintDocumentCommand(Printer printer)
        {
            _printer = printer;
        }

        public void Execute()
        {
            _printer.Print();
        }
    }
}
