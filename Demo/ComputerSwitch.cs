using System.Net;
using HomeKit.Net;
using HomeKit.Net.Enums;
using static System.Net.Mime.MediaTypeNames;
using ZXing.Aztec.Internal;
using System.Net;
using System.Net.NetworkInformation;

namespace Demo;

public class ComputerSwitch : Accessory
{
    public bool IsOn { get; private set; }
    
    public event Action<bool>? OnChange; 
    
    /// <summary>
    /// 苹果手机的家庭应用改变后回调方法
    /// </summary>
    public Characteristics CurrentOnCharacteristics { get; }

    /// <summary>
    /// 计算机开关
    /// </summary>
    public ComputerSwitch(AccessoryDriver accessoryDriver, string name, int? aid = null)
        : base(accessoryDriver, name, aid)
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