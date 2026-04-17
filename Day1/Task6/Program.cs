int dayNum;
Console.Write("Введите порядковый номер дня месяца: ");
dayNum = int.Parse(Console.ReadLine());

int daysInMonth = 30; // или 31, для примера взято 30
int remainingDays = daysInMonth - dayNum;

Console.WriteLine($"До конца месяца осталось дней: {remainingDays}");