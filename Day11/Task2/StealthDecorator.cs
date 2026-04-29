using System;
using System.Collections.Generic;
using System.Text;

namespace Task2
{
    public class StealthDecorator : CharacterDecorator
    {
        public StealthDecorator(IGameCharacter character) : base(character) { }

        public override string GetAbilities()
        {
            return base.GetAbilities() + ", Скрытность";
        }
    }
}
