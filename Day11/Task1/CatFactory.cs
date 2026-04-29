using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class CatFactory : AnimalFactory
    {
        public override IAnimal CreateAnimal()
        {
            return new Cat();
        }
    }
}
