using System;
using System.Collections.Generic;
using System.Text;

namespace Task2
{
    public class BaseCharacter : IGameCharacter
    {
        public string GetAbilities()
        {
            return "Базовые атаки";
        }
    }
}
