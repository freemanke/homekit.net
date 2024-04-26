using HomeKit.Net.Enums;

namespace HomeKit.Net;

/// <summary>
/// A representation of a HAP bridge.
/// A `Bridge` can have multiple `Accessories`
/// </summary>
public class Bridge : Accessory
{
    public Dictionary<int, Accessory> Accessories { get; } = new();

    public Bridge(AccessoryDriver accessoryDriver, string name) 
        : base(accessoryDriver, name)
    {
        Category = Category.CATEGORY_BRIDGE;
    }

    public void AddAccessory(Accessory accessory)
    {
        if (accessory.Category == Category.CATEGORY_BRIDGE)
        {
            throw new Exception("Bridges cannot be bridged");
        }

        if (accessory.Aid == Aid)
        {
            // For some reason AID=7 gets unsupported. See issue #61
            for (var i = 2; i < 100; i++)
            {
                if (i != Aid && i != 7 && Accessories.Keys.All(it => it != i))
                {
                    accessory.Aid = i;
                    break;
                }
            }
        }

        //if (accessory.Aid == Aid || Accessories.Keys.Any(it => it == accessory.Aid))
        //{
        //    throw new Exception("Duplicate AID found when attempting to add accessory");
        //}

        Accessories[accessory.Aid] = accessory;
    }

    public Characteristics GetCharacteristic(int aid, int iid)
    {
        if (Aid == aid)
        {
            return (Characteristics)IidManager.GetObject(iid);
        }

        var accessory = Accessories[aid];
        return accessory.GetAccessoryCharacteristic(aid, iid);
    }

    public List<AccessoryHapJson> ToHaps()
    {
        var list = Accessories.Select(it => it.Value.ToHap()).ToList();
        list.Add(ToHap());
        return list;
    }
}