using WebSocketSharp;
using WebSocketSharp.Server;

using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

namespace Server.Behaviors;

public class BaseBehavior : WebSocketBehavior
{
   /// <summary>
   /// Получение сообщения об открытие сокета
   /// </summary>
   protected override void OnOpen()
   {
      base.OnOpen();
   }

   /// <summary>
   /// Получение сообщения от сервиса
   /// </summary>
   protected override void OnMessage(MessageEventArgs e)
   {
      base.OnMessage(e);
      
      Send(e.Data);
   }
   
   /// <summary>
   /// Получение сообщения об ошибке сокета
   /// </summary>
   protected override void OnError(ErrorEventArgs e)
   {
      base.OnError(e);
   }
   
   /// <summary>
   /// Получение сообщения об закрытие сокета
   /// </summary>
   protected override void OnClose(CloseEventArgs e)
   {
      base.OnClose(e);
   }
}