using HomeKit.Net;
using HomeKit.Net.Enums;

namespace Demo;

/// <summary>
/// 温度传感器
/// </summary>
public class TemperatureSensor : Accessory
{
    private readonly Timer timer;

    public Characteristics CurrentTemperatureCharacteristics { get; }
    
    public TemperatureSensor(AccessoryDriver driver, string name, CancellationToken token = default)
        : base(driver, name)
    {
        var service = AddPreloadService("Temperature Sensor");
        Category = Category.CATEGORY_SENSOR;
        CurrentTemperatureCharacteristics = service.GetCharacteristics("Current Temperature");
        CurrentTemperatureCharacteristics.SetValue(1);
        timer = new Timer(Test, token, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
    }

    public void Test(object? state)
    {
        if (state is CancellationToken { IsCancellationRequested: true })
        {
            return;
        }
        var random = new Random();
        var temperature = random.Next(1, 50);
        CurrentTemperatureCharacteristics.SetValue(temperature);
        timer.Change(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
    }
}