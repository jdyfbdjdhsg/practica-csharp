using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class Bird : IAnimal
    {
        public string MakeSound()
        {
            return "Чик-чирик!";
        }
    }
}
