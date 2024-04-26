using Newtonsoft.Json;

namespace HomeKit.Net;

/// <summary>
/// Characteristics
/// </summary>
public class Characteristics : IAssignIid
{
    /// <summary>
    /// Characteristics ID
    /// </summary>
    public Guid Iid { get; set; }

    /// <summary>
    /// Characteristics Name
    /// </summary>
    public string Name { get; set; }
    public string Format { get; set; }
    public List<string> Permissions { get; set; }
    public string HapType { get; set; }
    public Accessory Accessory { get; set; }
    public int? MaxValue { get; set; }
    public int? MinValue { get; set; }
    public int? MaxLength { get; set; }
    public int? MinStep { get; set; }
    public string Unit { get; set; }

    private object Value;

    [JsonIgnore] 
    public Service Service { get; set; }

    /// <summary>
    /// Valid Values
    /// </summary>
    [JsonIgnore]
    public List<KeyValuePair<string, string>> ValidValues { get; set; } = new();

    /// <summary>
    /// Callback when value is set
    /// </summary>
    public Action<object>? SetValueCallback { get; set; }
    
    /// <summary>
    /// Callback when getting value
    /// </summary>
    public Func<object>? GetValueCallback { get; set; }

    public Characteristics(Guid iid)
    {
        Iid = iid;
        GuidToHapType();
    }

    public void GuidToHapType()
    {
        HapType = Utils.GuidToHapType(Iid);
    }

    public void InitValue()
    {
        Value = GetDefaultValue();
    }

    public void SetHapType()
    {
        HapType = Utils.GuidToHapType(Iid);
    }

    private bool IsNumberFormat => Constants.HAP_FORMAT_NUMERICS.Contains(Format);

    /// <summary>
    /// Get Value
    /// </summary>
    public object GetValue()
    {
        if (GetValueCallback != null)
        {
            var value = GetValueCallback();
            value = ValidValue(value);
            return value;
        }

        return Value;
    }

    /// <summary>
    /// Set Value
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(object value)
    {
        var newValue = ValidValue(value);
        var isChange = newValue != Value;
        Value = newValue;
        if (isChange)
        {
            Notify();
        }

        if (IsInAlwaysNull)
        {
            Value = null;
        }
    }


    /// <summary>
    /// Notify clients about a value change. Sends the value
    /// </summary>
    private void Notify(string connectionString = "")
    {
        var immediate = Constants.IMMEDIATE_NOTIFY.Contains(Iid);
        Accessory.Publish(GetValue(), this, connectionString, immediate);
    }

    /// <summary>
    /// Valid Value In ValidValues
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    private void ValidValueInValidValues<T>(T value)
    {
        if (ValidValues.Count > 0)
        {
            var validValues = ValidValues.Select(it => (T)Convert.ChangeType(it, typeof(T))).ToList();
            if (!validValues.Contains(value))
            {
                throw new Exception(
                    $"{Name}: value={value} is an invalid value, is not in {string.Join(";", validValues)}");
            }
        }
    }

    /// <summary>
    /// Valid Value
    /// 校验值是否合法
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public object ValidValue(object value)
    {
        if (Format == Constants.HAP_FORMAT_STRING)
        {
            if (value is not string str)
            {
                throw new Exception($"{Name}: value={value} is not string");
            }

            ValidValueInValidValues(str);

            return str;
        }

        if (Format == Constants.HAP_FORMAT_BOOL)
        {
            if (value is bool boolValue)
            {
                return boolValue;
            }

            return Convert.ToBoolean(value);
        }

        if (Constants.HAP_FORMAT_NUMERICS.Contains(Format))
        {
            if (!(value is int || value is float))
            {
                throw new Exception($"{Name}: value={value} is not a numeric value.");
            }

            if (value is double doubleValue && MinStep.HasValue)
            {
                doubleValue = (int)Math.Round(MinStep.Value * Math.Round((double)(doubleValue / MinStep.Value)), 14);
                if (MaxValue.HasValue)
                {
                    doubleValue = Math.Min(doubleValue, MaxValue.Value);
                }

                if (MinValue.HasValue)
                {
                    doubleValue = Math.Max(doubleValue, MinValue.Value);
                }

                ValidValueInValidValues(doubleValue);
                return doubleValue;
            }

            if (value is float floatValue && MinStep.HasValue)
            {
                floatValue = (float)Math.Round(MinStep.Value * Math.Round(floatValue / MinStep.Value), 14);
                if (MaxValue.HasValue)
                {
                    floatValue = Math.Min(floatValue, MaxValue.Value);
                }

                if (MinValue.HasValue)
                {
                    floatValue = Math.Max(floatValue, MinValue.Value);
                }

                ValidValueInValidValues(floatValue);
                return floatValue;
            }
        }

        return value;
    }

    public CharacteristicsHapJson ToHap()
    {
        var result = new CharacteristicsHapJson()
        {
            Iid = Accessory.IidManager.GetIid(this),
            Type = HapType,
            Format = Format,
            Perms = Permissions
        };

        var tempValue = GetValue();

        if (IsNumberFormat)
        {
            result.MinValue = MinValue;
            result.MaxValue = MaxValue;
            result.MinStep = MinStep;
            result.Unit = Unit;
            if (ValidValues.Count > 0)
            {
                result.ValidValues = ValidValues.Select(it => decimal.Parse(it.Value)).ToList();
            }
        }

        if (Format == Constants.HAP_FORMAT_STRING)
        {
            if (MaxLength.HasValue && MaxLength.Value != Constants.DEFAULT_MAX_LENGTH)
            {
                result.MaxLen = MaxLength.Value;
            }
        }

        if (Permissions.Contains(Constants.HAP_PERMISSION_READ))
        {
            result.Value = tempValue;
        }

        return result;
    }

    public object GetDefaultValue()
    {
        var guid = new Guid("00000073-0000-1000-8000-0026BB765291");
        if (Iid == guid)
        {
            return null;
        }

        if (ValidValues.Count > 0)
        {
            return ValidValues.Select(it => it.Value).FirstOrDefault();
        }

        return Constants.HAP_FORMAT_DEFAULTS[Format];
    }

    /// <summary>
    /// Called from broker for value change in Home app, Change self.
    /// value to value and call callback
    /// </summary>
    /// <param name="value"></param>
    /// <param name="connectionString"></param>
    public void ClientUpdateValue(object value, string connectionString)
    {
        var originalValue = value;
        if (!IsInAlwaysNull || value != null)
        {
            value = ValidValue(value);
        }

        var previousValue = Value;
        Value = value;
        if (SetValueCallback != null)
        {
            SetValueCallback(value);
        }

        var change = previousValue != Value;
        if (change)
        {
            Notify(connectionString);
        }

        if (IsInAlwaysNull)
        {
            Value = null;
        }
    }

    public bool IsInAlwaysNull => Iid == new Guid("00000073-0000-1000-8000-0026BB765291");
   
    public override string ToString()
    {
        return $"name:{Name},format:{Format},value:{Value}";
    }
}