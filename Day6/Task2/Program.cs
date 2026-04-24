using System;
using System.IO;

public delegate void ImageProcessor(string imagePath);

class Program
{
    static void ProcessImage(string imagePath, ImageProcessor processor)
    {
        Console.WriteLine($"Обработка изображения: {imagePath}");
        processor(imagePath);
        Console.WriteLine("Обработка завершена\n");
    }

    static void ResizeImage(string imagePath)
    {
        Console.WriteLine($"  - Изменяем размер изображения {imagePath}");
        Thread.Sleep(500);
    }

    static void ConvertToGrayscale(string imagePath)
    {
        Console.WriteLine($"  - Преобразуем в оттенки серого {imagePath}");
        Thread.Sleep(500);
    }

    static void Main()
    {
        string imageFile = "photo.jpg";

        Console.WriteLine("Демонстрация callback");

        ProcessImage(imageFile, ResizeImage);
        ProcessImage(imageFile, ConvertToGrayscale);

        Console.WriteLine("Комбинированная обработка");
        ImageProcessor combined = ResizeImage;
        combined += ConvertToGrayscale;
        ProcessImage(imageFile, combined);
    }
}