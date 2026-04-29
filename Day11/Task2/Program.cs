namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Декоратор\n");

            IGameCharacter character = new BaseCharacter();
            Console.WriteLine($"Базовый персонаж: {character.GetAbilities()}");

            character = new StealthDecorator(character);
            Console.WriteLine($"С навыком скрытности: {character.GetAbilities()}");

            character = new PowerAttackDecorator(character);
            Console.WriteLine($"+ мощная атака: {character.GetAbilities()}");

            character = new HealingDecorator(character);
            Console.WriteLine($"+ лечение: {character.GetAbilities()}");
        }
    }
}