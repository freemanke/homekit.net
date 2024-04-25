﻿using System.Net;
using System.Net.NetworkInformation;
using HomeKit.Net;

namespace Demo;

public class Program
{
    public static async Task Main(string[] args)
    {
        await SingleAccessory();
    }
    
    public static async Task SingleAccessory()
    {
        var driver = new AccessoryDriver(port: 6555);
        var accessory = new ComputerSwitch(driver, "计算机开关");
        accessory.OnChange += async _ =>
        {
            var macAddress = "7C:10:C9:8C:87:43";
            await PhysicalAddress.Parse(macAddress.Replace(":", "-")).SendWolAsync();
            Console.WriteLine($"唤醒计算机 {macAddress}");
        };

        driver.AddAccessory(accessory);
        await driver.StartAsync(new CancellationTokenSource().Token);
    }

    public static async Task MultipleAccessories()
    {
        var tokenSource = new CancellationTokenSource();
        var driver = new AccessoryDriver(port: 6554);
        var bridge = new Bridge(driver, "网关");
        var computerSwitch = new ComputerSwitch(driver, "计算机开关");
        bridge.AddAccessory(computerSwitch);
        computerSwitch.OnChange += async _ =>
        {
            var macAddress = "7C:10:C9:8C:87:43";
            await PhysicalAddress.Parse(macAddress.Replace(":", "-")).SendWolAsync();
            Console.WriteLine($"唤醒计算机 {macAddress}");
        };

        var sensor = new TemperatureSensor(driver, "温度传感器");
        bridge.AddAccessory(sensor);
        driver.AddAccessory(bridge);
        await driver.StartAsync(tokenSource.Token);
    }
}