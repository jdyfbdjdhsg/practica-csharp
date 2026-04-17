Console.WriteLine("Двузначные числа, равные утроенному произведению своих цифр:");

for (int num = 10; num <= 99; num++)
{
    int tens = num / 10;
    int units = num % 10;

    if (num == 3 * (tens * units))
    {
        Console.WriteLine(num);
    }
}