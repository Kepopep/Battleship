using Server.GameLogic.Player;

namespace Server.Web;

public static class GlobalLobby
{
    private static readonly Dictionary<string, Player> _players = new Dictionary<string, Player>();
    
    public static void ConnectPlayer(string connectionId)
    {
        _players.Add(connectionId, new Player());
    }
    
    public static void DisconnectPlayer(string connectionId)
    {
        _players.Remove(connectionId);
    }

    public static bool TryAssignOpponent(string connectionId, out string opponentId)
    {
        var possibleOpponents = _players
            .Where(x => x.Key != connectionId && !x.Value.HasEnemy);

        if (!possibleOpponents.Any() || !_players.ContainsKey(connectionId))
        {
            opponentId = string.Empty;
            return false;
        }
        
        
        var opponentInfo = possibleOpponents.First();
        
        _players[connectionId].AssignEnemy(opponentInfo.Value);
        
        opponentInfo.Value.AssignEnemy(_players[connectionId]);

        opponentId = opponentInfo.Key;

        Console.WriteLine($"Game started: {connectionId} vs {opponentInfo.Key}");
        return true;
    }
    
    public static Player GetPlayer(string connectionId) => _players[connectionId];
}