using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    public class CancelPrintCommand : ICommand
    {
        private Printer _printer;

        public CancelPrintCommand(Printer printer)
        {
            _printer = printer;
        }

        public void Execute()
        {
            _printer.Cancel();
        }
    }
}
