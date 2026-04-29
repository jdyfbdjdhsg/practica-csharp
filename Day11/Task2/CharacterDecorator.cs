using System;
using System.Collections.Generic;
using System.Text;

namespace Task2
{
    public abstract class CharacterDecorator : IGameCharacter
    {
        protected IGameCharacter _character;

        public CharacterDecorator(IGameCharacter character)
        {
            _character = character;
        }

        public virtual string GetAbilities()
        {
            return _character.GetAbilities();
        }
    }
}
