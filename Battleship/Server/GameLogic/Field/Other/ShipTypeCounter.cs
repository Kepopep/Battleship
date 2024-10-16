using Server.GameLogic.Ship;

namespace Server.GameLogic.Field;

public class ShipTypeCounter
{
    private readonly Dictionary<ShipType, int> _existingTypes = new();

    private readonly ShipConfig? _config;

    public ShipTypeCounter(ShipConfig? config)
    {
        if (config == null)
        {
            Console.WriteLine("Ship config is null");
            return;
        }
        
        _config = config;
    }
    
    public void Add(ShipType ship)
    {
        if (!_existingTypes.TryAdd(ship, 1))
        {
            _existingTypes[ship]++;
        }
    }

    public bool CanAdd(ShipType type)
    {
        if (_config == null || !_existingTypes.ContainsKey(type))
        {
            return true;
        }

        var maxCount = _config
            .Configs
            .First(x => x.ShipType == type)
            .MaxCount;
        
        return _existingTypes[type] < maxCount;
    }
}