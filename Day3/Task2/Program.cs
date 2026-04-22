using System;
using System.Collections.Generic;
using System.Linq;

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
}

class Employee : Person
{
    public double Salary { get; set; }
    public string Department { get; set; }
}

static class ArrayUtils
{
    public static int GetMaxValue(Person[] people)
    {
        return people.Max(p => p.Age);
    }

    public static void SortByName(Person[] people)
    {
        Array.Sort(people, (x, y) => x.Name.CompareTo(y.Name));
    }

    public static Person[] FilterByAge(Person[] people)
    {
        return people.Where(p => p.Age > 30).ToArray();
    }

    public static double CalculateAverageSalary(Employee[] employees)
    {
        return employees.Average(e => e.Salary);
    }

    public static Person[] GenerateRandomPersons(int size)
    {
        Random rnd = new Random();
        string[] names = { "Анна", "Иван", "Мария", "Петр", "Ольга" };
        string[] cities = { "Москва", "СПб", "Казань" };
        Person[] result = new Person[size];
        for (int i = 0; i < size; i++)
        {
            result[i] = new Person
            {
                Name = names[rnd.Next(names.Length)],
                Age = rnd.Next(18, 70),
                City = cities[rnd.Next(cities.Length)]
            };
        }
        return result;
    }
}

static class MathOperations
{
    public static double Sum(double[] numbers)
    {
        return numbers.Sum();
    }

    public static double Product(double[] numbers)
    {
        double res = 1;
        foreach (var n in numbers) res *= n;
        return res;
    }
}

static class StringProcessor
{
    public static string ConcatenateNames(Person[] people)
    {
        return string.Join(", ", people.Select(p => p.Name));
    }

    public static Dictionary<string, int> CountPeopleInCity(Person[] people)
    {
        return people.GroupBy(p => p.City).ToDictionary(g => g.Key, g => g.Count());
    }

    public static void ReverseArray(Person[] people)
    {
        Array.Reverse(people);
    }

    public static Employee[] FindEmployeesByDepartment(Employee[] employees, string department)
    {
        return employees.Where(e => e.Department == department).ToArray();
    }

    public static Dictionary<int, List<Person>> GroupByAge(Person[] people)
    {
        return people.GroupBy(p => p.Age).ToDictionary(g => g.Key, g => g.ToList());
    }
}