using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    public class PrintManager
    {
        private ICommand _command;

        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        public void ExecuteCommand()
        {
            _command?.Execute();
        }
    }
}
