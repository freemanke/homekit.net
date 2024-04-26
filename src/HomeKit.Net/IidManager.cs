namespace HomeKit.Net;

/// <summary>
/// Maintains a mapping between Service/Characteristic objects and IIDs
/// </summary>
public class IidManager
{
    private int counter;
    private Dictionary<IAssignIid, int> mapping = new();

    private int GetNewIid()
    {
        counter++;
        return counter;
    }

    /// <summary>
    /// Assign an IID to given object
    /// </summary>
    public void Assign<T>(T o) where T : IAssignIid
    {
        if (!mapping.ContainsKey(o))
        {
            var iid = GetNewIid();
            mapping[o] = iid;
        }
    }

    /// <summary>
    /// Get the object that is assigned the given IID
    /// </summary>
    /// <param name="iid"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IAssignIid GetObject(int iid)
    {
        foreach (var pair in mapping)
        {
            if (pair.Value == iid)
            {
                return pair.Key;
            }
        }

        throw new Exception($"can not find object with iid{iid}");
    }

    /// <summary>
    /// Get the IID assigned to the given object
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int GetIid(IAssignIid o)
    {
        foreach (var pair in mapping)
        {
            if (pair.Key == o)
            {
                return pair.Value;
            }
        }

        throw new Exception($"can not find iid with object {o}");
    }

    /// <summary>
    /// Remove an object from the mapping
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int RemoveObject(IAssignIid o)
    {
        if (mapping.ContainsKey(o))
        {
            var iid = mapping[o];
            mapping.Remove(o);
            return iid;
        }
        throw new Exception($"can not find iid with object  {o}");
    }

    /// <summary>
    /// Remove an object with an IID from the mapping
    /// </summary>
    /// <param name="iid"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IAssignIid RemoveIid(int iid)
    {
        var pairs = mapping.Where(a => a.Value == iid).ToList();
        foreach(var pair in pairs)
        {
            mapping.Remove(pair.Key);
            return pair.Key;
        }

        throw new Exception($"can not find iid with iid {iid}");
    }
}