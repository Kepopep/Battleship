using Server.GameLogic.Field;

namespace Server.Web.Lobby.Game;

public class GameFieldView
{
    public Cell[] Self {get; set;}
    
    public Cell[] Opponent {get; set;}
}