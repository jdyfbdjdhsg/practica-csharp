using System;
using System.Collections.Generic;
using System.Text;

namespace Task2
{
    public class HealingDecorator : CharacterDecorator
    {
        public HealingDecorator(IGameCharacter character) : base(character) { }

        public override string GetAbilities()
        {
            return base.GetAbilities() + ", Лечение";
        }
    }
}
