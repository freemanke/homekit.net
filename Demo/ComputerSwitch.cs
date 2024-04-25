using System.Net;
using HomeKit.Net;
using HomeKit.Net.Enums;
using System.Net;
using System.Net.NetworkInformation;

namespace Demo;

/// <summary>
/// 计算机远程控制开关
/// </summary>
public class ComputerSwitch : Accessory
{
    public bool IsOn { get; private set; }
    
    public event Action<bool>? OnChange; 
    
    /// <summary>
    /// 苹果手机家庭应用控制操作后的回调方法
    /// </summary>
    public Characteristics CurrentOnCharacteristics { get; }

    /// <summary>
    /// 计算机开关
    /// </summary>
    public ComputerSwitch(AccessoryDriver driver, string name, int? aid = null)
        : base(driver, name, aid)
    {
        var service = AddPreloadService("Switch");
        Category = Category.CATEGORY_SWITCH;
        CurrentOnCharacteristics = service.GetCharacteristics("On");
        CurrentOnCharacteristics.SetValueCallback = Callback;
    }

    private void Callback(object value)
    {
        IsOn = (bool)value;
        OnChange?.Invoke(IsOn);
    }
}