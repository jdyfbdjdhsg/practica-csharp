using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class BirdFactory : AnimalFactory
    {
        public override IAnimal CreateAnimal()
        {
            return new Bird();
        }
    }
}
