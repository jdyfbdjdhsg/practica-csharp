using System;

public delegate void BatteryLowHandler(int batteryLevel);

public class BatteryMonitor
{
    public event BatteryLowHandler BatteryLow;

    private int _batteryLevel;

    public void CheckBattery(int level)
    {
        _batteryLevel = level;
        Console.WriteLine($"Уровень заряда: {_batteryLevel}%");

        if (_batteryLevel < 20)
        {
            BatteryLow?.Invoke(_batteryLevel);
        }
    }
}

public class PowerSaver
{
    public void EnablePowerSaving(int level)
    {
        Console.WriteLine($"Включен режим энергосбережения! Уровень: {level}%");
    }
}

public class UserNotifier
{
    public void ShowWarning(int level)
    {
        Console.WriteLine($"Низкий заряд батареи: {level}%");
    }
}

class Program
{
    static void Main()
    {
        BatteryMonitor monitor = new BatteryMonitor();
        PowerSaver powerSaver = new PowerSaver();
        UserNotifier notifier = new UserNotifier();

        monitor.BatteryLow += powerSaver.EnablePowerSaving;
        monitor.BatteryLow += notifier.ShowWarning;

        Console.WriteLine("Мониторинг батареи\n");

        monitor.CheckBattery(50);
        monitor.CheckBattery(30);
        monitor.CheckBattery(15);  
        monitor.CheckBattery(10); 
    }
}