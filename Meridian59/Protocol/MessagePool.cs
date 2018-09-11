/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

using System.Collections.Concurrent;
using Meridian59.Common.Enums;
using Meridian59.Protocol.Enums;
using Meridian59.Protocol.GameMessages;

namespace Meridian59.Protocol
{
   /// <summary>
   /// A pool holding instances for certain message types.
   /// Most useful for fixed sized messages that are used a lot.
   /// </summary>
   public static class MessagePool
   {
      private const int INITREQMOVE = 20;
      private const int INITMOVE = 100;
      private const int INITTURN = 20;

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////

      private static readonly ConcurrentStack<ReqMoveMessage> poolReqMove = new ConcurrentStack<ReqMoveMessage>();
      private static readonly ConcurrentStack<MoveMessage>    poolMove    = new ConcurrentStack<MoveMessage>();
      private static readonly ConcurrentStack<TurnMessage>    poolTurn    = new ConcurrentStack<TurnMessage>();

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////

      /// <summary>
      /// Static Constructor
      /// </summary>
      static MessagePool()
      {
         for (int i = 0; i < INITREQMOVE; i++) poolReqMove.Push(new ReqMoveMessage(0, 0, 0, 0, 0));
         for (int i = 0; i < INITMOVE; i++)    poolMove.Push(new MoveMessage(0, 0, 0, 0, 0));
         for (int i = 0; i < INITTURN; i++)    poolTurn.Push(new TurnMessage(0, 0));
      }

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////

      /// <summary>
      /// Adds a free BP_REQ_MOVE message back to the pool.
      /// </summary>
      /// <param name="Message"></param>
      public static void PushFree(GameMessage Message)
      {
         if (Message is GameModeMessage)
         {
            switch ((MessageTypeGameMode)Message.PI)
            {
               case MessageTypeGameMode.ReqMove: poolReqMove.Push((ReqMoveMessage)Message); break;
               case MessageTypeGameMode.Move: poolMove.Push((MoveMessage)Message); break;
               case MessageTypeGameMode.Turn: poolTurn.Push((TurnMessage)Message); break;
            }
         }
      }

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////
      // OUTGOING MESSAGES (VALUE CONSTRUCTORS)

      /// <summary>
      /// Returns a free BP_REQ_MOVE message from the pool or allocates one.
      /// Replacement for calling the according message constructor.
      /// </summary>
      /// <returns></returns>
      public static ReqMoveMessage PopReqMove(ushort X, ushort Y, byte MoveMode, uint CurrentMapID, ushort Angle)
      {
         ReqMoveMessage msg;
         if (poolReqMove.TryPop(out msg))
         {
            msg.X = X;
            msg.Y = Y;
            msg.MoveMode = MoveMode;
            msg.CurrentMapID = CurrentMapID;
            msg.Angle = Angle;
            return msg;
         }
         else
            return new ReqMoveMessage(X, Y, MoveMode, CurrentMapID, Angle);
      }

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////
      // INCOMING MESSAGES (PARSER CONSTRUCTORS)

      /// <summary>
      /// Returns a free BP_MOVE message from the pool or allocates one.
      /// Replacement for calling the according message constructor by parser.
      /// </summary>
      /// <returns></returns>
      public unsafe static MoveMessage PopMove(ref byte* Buffer)
      {
         MoveMessage msg;
         if (poolMove.TryPop(out msg))
         {
            msg.ReadFrom(ref Buffer);
            return msg;
         }
         else
            return new MoveMessage(ref Buffer);
      }

      /// <summary>
      /// Returns a free BP_MOVE message from the pool or allocates one.
      /// Replacement for calling the according message constructor by parser.
      /// </summary>
      /// <returns></returns>
      public unsafe static TurnMessage PopTurn(ref byte* Buffer)
      {
         TurnMessage msg;
         if (poolTurn.TryPop(out msg))
         {
            msg.ReadFrom(ref Buffer);
            return msg;
         }
         else
            return new TurnMessage(ref Buffer);
      }
   }
}
