using System;
using System.Collections.Generic;
using System.Text;

namespace Task2
{
    public class PowerAttackDecorator : CharacterDecorator
    {
        public PowerAttackDecorator(IGameCharacter character) : base(character) { }

        public override string GetAbilities()
        {
            return base.GetAbilities() + ", Мощная атака";
        }
    }
}
