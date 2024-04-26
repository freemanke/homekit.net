using System.Text;
using HomeKit.Net.Enums;

namespace HomeKit.Net;

/// <summary>
/// Accessory base class
/// </summary>
public abstract class Accessory
{
    /// <summary>
    /// Accessory constructor
    /// </summary>
    /// <param name="accessoryDriver">Accessory Driver</param>
    /// <param name="name">Accessory name</param>
    /// <param name="aid">Accessory ID</param>
    protected Accessory(AccessoryDriver accessoryDriver, string name, int aid = Const.STANDALONE_AID)
    {
        AccessoryDriver = accessoryDriver;
        Name = name;
        Aid = aid;
        AddInfoService();
    }

    /// <summary>
    /// Accessory category
    /// </summary>
    public Category Category { get; protected set; } = Category.CATEGORY_OTHER;

    /// <summary>
    /// Accessory ID
    /// </summary>
    public int Aid { get; set; }

    public AccessoryDriver AccessoryDriver { get; }

    /// <summary>
    /// Accessory Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Accessory ID manager
    /// </summary>
    public IidManager IidManager = new();

    /// <summary>
    /// Pair Verify One Encryption Context
    /// </summary>
    public PairVerifyOneEncryptionContext PairVerifyOneEncryptionContext { get; set; } = new();

    /// <summary>
    /// Accessory Services
    /// </summary>
    public List<Service> Services { get; } = new();

    /// <summary>
    /// add the required `AccessoryInformation` service
    /// </summary>
    public void AddInfoService()
    {
        var accessoryInformationService = Loader.LoadService("AccessoryInformation");
        AddService(accessoryInformationService);
        accessoryInformationService.ConfigureCharacteristics("Name", Name);
        accessoryInformationService.ConfigureCharacteristics("SerialNumber", "default");
    }

    /// <summary>
    /// Print setup message to console
    /// </summary>
    public void SetupMessage()
    {
        var xhmUri = XhmUri();
        var pinCode = Encoding.UTF8.GetString(AccessoryDriver.State.PinCode);
        Console.WriteLine($"Setup payload: {xhmUri}");
        Console.WriteLine($"Scan this code with your Home app on your iOS device: {xhmUri}");
        Console.WriteLine($"Or enter this code in your Home app on your iOS device: {pinCode}");
        QRConsole.Output(xhmUri);
    }

    /// <summary>
    /// A HAP representation of this Accessory
    /// </summary>
    public AccessoryHapJson ToHap()
    {
        var result = new AccessoryHapJson
        {
            Aid = Aid,
            Services = new List<ServiceHapJson>()
        };
        foreach (var service in Services)
        {
            result.Services.Add(service.ToHap());
        }

        return result;
    }

    /// <summary>
    /// Add Service
    /// </summary>
    /// <param name="service">Accessory service</param>
    public void AddService(Service service)
    {
        service.Accessory = this;
        Services.Add(service);
        IidManager.Assign(service);
        foreach (var characteristics in service.CharacteristicsList)
        {
            characteristics.Accessory = this;
            IidManager.Assign(characteristics);
        }
    }

    /// <summary>
    /// Get service
    /// </summary>
    /// <param name="serviceName">Service name</param>
    /// <returns>Service</returns>
    /// <exception cref="Exception"></exception>
    public Service GetService(string serviceName)
    {
        var service = Services.FirstOrDefault(it => it.Name == serviceName);
        if (service == null)
        {
            throw new Exception($"can not find service with name {serviceName}");
        }

        return service;
    }

    /// <summary>
    /// Generates the X-HM:// uri
    /// </summary>
    /// <returns>Setup Code URI</returns>
    public string XhmUri()
    {
        long payload = 0;
        payload |= 0 & 0x7;
        payload <<= 4;
        payload |= 0 & 0xF; // reserved bits

        payload <<= 8;
        payload |= (uint)((int)Category & 0xFF); // category

        payload <<= 4;
        payload |= 2 & 0xF; // flags
        payload <<= 27;

        var pinCode = Encoding.UTF8.GetString(AccessoryDriver.State.PinCode);
        payload |= (uint)(int.Parse(pinCode.Replace("-", "")) & 0x7FFFFFFF);

        var encodedPayload = Base36Converter.ConvertTo(payload);
        encodedPayload = encodedPayload.PadLeft(9, '0');
        return "X-HM://" + encodedPayload + AccessoryDriver.State.SetupId;
    }

    /// <summary>
    /// Create a service with the given name and add it to this accessory
    /// </summary>
    /// <param name="serviceName"></param>
    protected Service AddPreloadService(string serviceName)
    {
        var service = Loader.LoadService(serviceName);
        if (service == null)
        {
            throw new Exception($"can not find service with name {serviceName}");
        }

        AddService(service);
        return service;
    }

    /// <summary>
    /// Get accessory's characteristics
    /// </summary>
    /// <param name="aid">Accessory ID</param>
    /// <param name="iid"></param>
    /// <returns></returns>
    public Characteristics GetAccessoryCharacteristic(int aid, int iid)
    {
        if (Aid != aid)
        {
            return null!;
        }

        return (Characteristics)IidManager.GetObject(iid);
    }

    /// <summary>
    /// Set Primary Service
    /// </summary>
    public void SetPrimaryService(Service service)
    {
        foreach (var service1 in Services)
        {
            if (service.Iid == service1.Iid)
            {
                service1.IsPrimaryService = true;
                break;
            }
        }
    }

    public void Publish(object value, IAssignIid sender, string connectionString = "", bool immediate = false)
    {
        var sendData = new SendEventDataItem
        {
            Aid = Aid,
            Iid = IidManager.GetIid(sender),
            Value = value
        };
        AccessoryDriver.Publish(sendData, connectionString, immediate);
    }
}