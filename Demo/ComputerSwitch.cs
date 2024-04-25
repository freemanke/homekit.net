using System.Net;
using HomeKit.Net;
using HomeKit.Net.Enums;
using System.Net;
using System.Net.NetworkInformation;

namespace Demo;

/// <summary>
/// 计算机远程控制开关
/// 通过继承类 Accessory，我们就可以自定义一个自己的配件，在下面的示例中，我们定义一个开关，
/// 在构造函数中，我们加载一个名为 Switch 的服务，并且定义配件类型为开关，从 switch 服务中获取 on 这个特性，
/// 通过操作 on 这个特性，我们就可以通过代码模拟开关状态变化了，并且可以在苹果手机的家庭 App 上看到开关状态的变化
/// </summary>
public class ComputerSwitch : Accessory
{
    public bool IsOn { get; private set; }
    
    /// <summary>
    /// 苹果手机家庭应用控制操作后的回调方法
    /// </summary>
    public event Action<bool>? OnChange; 
    

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