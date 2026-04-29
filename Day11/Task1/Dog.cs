using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class Dog : IAnimal
    {
        public string MakeSound()
        {
            return "Гав-гав!";
        }
    }
}
