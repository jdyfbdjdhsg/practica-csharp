using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class DogFactory : AnimalFactory
    {
        public override IAnimal CreateAnimal()
        {
            return new Dog();
        }
    }
}
