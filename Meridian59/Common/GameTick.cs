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

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Meridian59.Common
{
    /// <summary>
    /// A game ticking system with 64-Bit ticks and millisecond resolution.
    /// </summary>
    public class GameTick
    {
        #region Adjustable tick intervals
        public double INTERVALTPSMEASURE     = 250.0;
        public double INTERVALINTERACT       = 500.0;
        public double INTERVALREQUSERCOMMAND = 500.0;
        public double INTERVALREQACTION      = 500.0;
        public double INTERVALSAY            = 0.0;
        public double INTERVALBROADCAST      = 0.0;

#if VANILLA
        public double INTERVALREQMOVE   = 1000.0;
        public double INTERVALREQTURN   = 1000.0;
        public double INTERVALREQATTACK = 1000.0;
        public double INTERVALREQCAST   = 1000.0;
#else
        public double INTERVALREQMOVE   = 250.0;
        public double INTERVALREQTURN   = 250.0;
        public double INTERVALREQATTACK = 250.0;
        public double INTERVALREQCAST   = 250.0;
#endif
        #endregion

        /// <summary>
        /// Milliseconds in a second
        /// </summary>
        public const uint MSINSECOND = 1000;

        /// <summary>
        /// Divide 'ElapsedTicks' of StopWatch by this to get milliseconds.
        /// </summary>
        protected static readonly double MSTICKDIVISOR = (double)Stopwatch.Frequency / 1000.0;

        /// <summary>
        /// Stopwatch object, usually high resolution on most CLR
        /// </summary>
        protected readonly Stopwatch watch = new Stopwatch();

        #region Ticks
        /// <summary>
        /// Current tick
        /// </summary>
        public double Current { get; protected set; }
       
        /// <summary>
        /// Last tick
        /// </summary>
        public double Last { get; protected set; }

        /// <summary>
        /// Tick we last calculated (T)icks(P)er(S)econd
        /// </summary>
        public double TPSMeasure { get; protected set; }
        
        /// <summary>
        /// Tick we last sent BP_REQ_MOVE to the server
        /// </summary>
        public double ReqMove { get; protected set; }

        /// <summary>
        /// Tick we last sent BP_REQ_TURN to the server
        /// </summary>
        public double ReqTurn { get; protected set; }

        /// <summary>
        /// Tick we last sent BP_REQ_ATTACK to the server
        /// </summary>
        public double ReqAttack { get; protected set; }

        /// <summary>
        /// Tick we last sent BP_REQ_CAST to the server
        /// </summary>
        public double ReqCast { get; protected set; }

        /// <summary>
        /// Tick we last sent BP_USERCOMMAND
        /// </summary>
        public double ReqUserCommand { get; protected set; }

        /// <summary>
        /// Tick we last sent BP_ACTION (dance, wave, loot, moods)
        /// </summary>
        public double ReqAction { get; protected set; }

        /// <summary>
        /// Tick we last sent a chatmessage in NORMAL mode
        /// </summary>
        public double Say { get; protected set; }

        /// <summary>
        /// Tick we last sent a chatmessage in ALL mode
        /// </summary>
        public double Broadcast { get; protected set; }

        /// <summary>
        /// Ticks we last sent an interaction with an ID, like
        /// BP_REQ_USE, BP_REQ_APPLY, BP_REQ_UNUSE, BP_REQ_LOOK and others.
        /// </summary>
        public Dictionary<uint, double> Interactions { get; protected set; }
        #endregion

        #region Spans
        /// <summary>
        /// Milliseconds elapsed since last tick.
        /// Calculated once.
        /// </summary>
        public double Span { get; protected set; }

        /// <summary>
        /// Milliseconds elapsed since last TPS measuring.
        /// Calculated on-the-fly.
        /// </summary>
        public double SpanTPSMeasure { get { return Current - TPSMeasure; } }

        /// <summary>
        /// Milliseconds elapsed since last BP_REQ_MOVE.
        /// Calculated on-the-fly.
        /// </summary>
        public double SpanReqMove { get { return Current - ReqMove; } }

        /// <summary>
        /// Milliseconds elapsed since last BP_REQ_TURN.
        /// Calculated on-the-fly.
        /// </summary>
        public double SpanReqTurn { get { return Current - ReqTurn; } }

        /// <summary>
        /// Milliseconds elapsed since last BP_REQ_ATTACK.
        /// Calculated on-the-fly.
        /// </summary>
        public double SpanReqAttack { get { return Current - ReqAttack; } }

        /// <summary>
        /// Milliseconds elapsed since last BP_REQ_CAST.
        /// Calculated on-the-fly.
        /// </summary>
        public double SpanReqCast { get { return Current - ReqCast; } }

        /// <summary>
        /// Milliseconds elapsed since last BP_USERCOMMAND.
        /// Calculated on-the-fly.
        /// </summary>
        public double SpanReqUserCommand { get { return Current - ReqUserCommand; } }

        /// <summary>
        /// Milliseconds elapsed since last BP_ACTION.
        /// Calculated on-the-fly.
        /// </summary>
        public double SpanReqAction { get { return Current - ReqAction; } }
      
        /// <summary>
        /// Milliseconds elapsed since last said something.
        /// Calculated on-the-fly.
        /// </summary>
        public double SpanSay { get { return Current - Say; } }

        /// <summary>
        /// Milliseconds elapsed since last broadcasted something.
        /// Calculated on-the-fly.
        /// </summary>
        public double SpanBroadcast { get { return Current - Broadcast; } }

        #endregion

        /// <summary>
        /// Whether the underlying timersystem supports high resolution.
        /// </summary>
        public bool IsHighResolution
        {
            get
            {
                return Stopwatch.IsHighResolution;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameTick()
        {
            Interactions = new Dictionary<uint, double>();

            // start watch
            watch.Start();
        }

        /// <summary>
        /// Proceed to next game tick.
        /// </summary>
        public virtual void Tick()
        {
            // save the tick as last one
            Last = Current;
 
            // get tick for this threadloop in milliseconds
            Current = (double)watch.ElapsedTicks / MSTICKDIVISOR;

            // update general time span once (others on getter)
            Span = Current - Last;           
        }

        /// <summary>
        /// Returns a dedicated ms tick for this moment from the clock.
        /// This might be later than Current, depending on thread cycle duration.
        /// </summary>
        /// <returns></returns>
        public double GetUpdatedTick()
        {
            return (double)watch.ElapsedTicks / MSTICKDIVISOR;
        }

        #region Can
        /// <summary>
        /// Call this to know if you can measure TPS
        /// </summary>
        /// <returns></returns>
        public bool CanTPSMeasure()
        {
            return SpanTPSMeasure >= INTERVALTPSMEASURE;
        }

        /// <summary>
        /// Call this to know if you can send a BP_REQ_MOVE
        /// </summary>
        /// <returns></returns>
        public bool CanReqMove()
        {
            return SpanReqMove >= INTERVALREQMOVE;
        }

        /// <summary>
        /// Call this to know if you can send a BP_REQ_TURN
        /// </summary>
        /// <returns></returns>
        public bool CanReqTurn()
        {
            return SpanReqTurn >= INTERVALREQTURN;
        }

        /// <summary>
        /// Call this to know if you can send a BP_REQ_ATTACK
        /// </summary>
        /// <returns></returns>
        public bool CanReqAttack()
        {
            return SpanReqAttack >= INTERVALREQATTACK;
        }

        /// <summary>
        /// Call this to know if you can send a BP_REQ_CAST
        /// </summary>
        /// <returns></returns>
        public bool CanReqCast()
        {
            return SpanReqCast >= INTERVALREQCAST;
        }

        /// <summary>
        /// Call this to know if you can send a BP_USERCOMMAND
        /// </summary>
        /// <returns></returns>
        public bool CanReqUserCommand()
        {
            return SpanReqUserCommand >= INTERVALREQUSERCOMMAND;
        }

        /// <summary>
        /// Call this to know if you can send a BP_ACTION
        /// </summary>
        /// <returns></returns>
        public bool CanReqAction()
        {
            return SpanReqAction >= INTERVALREQACTION;
        }

        /// <summary>
        /// Call this to know if you can interact with an ID,
        /// like BP_REQ_USE, BP_REQ_APPLY, BP_REQ_LOOK and others.
        /// </summary>
        /// <returns></returns>
        public bool CanInteract(uint ID)
        {
            // default value
            double tick = 0;

            // try to get from tracked ones
            Interactions.TryGetValue(ID, out tick);

            return (Current - tick) >= INTERVALINTERACT;
        }

        /// <summary>
        /// Call this to know if you can say something.
        /// </summary>
        /// <returns></returns>
        public bool CanSay()
        {
            return SpanSay >= INTERVALSAY;
        }

        /// <summary>
        /// Call this to know if you can broadcast something.
        /// </summary>
        /// <returns></returns>
        public bool CanBroadcast()
        {
            return SpanBroadcast >= INTERVALBROADCAST;
        }
        #endregion

        #region Did
        /// <summary>
        /// Call this when you did a TPS measuring
        /// </summary>
        public void DidTPSMeasure()
        {
            TPSMeasure = Current;
        }

        /// <summary>
        /// Call this when you sent a BP_REQ_MOVE
        /// </summary>
        public void DidReqMove()
        {
            ReqMove = Current;
        }

        /// <summary>
        /// Call this when you sent a BP_REQ_TURN
        /// </summary>
        public void DidReqTurn()
        {
            ReqTurn = Current;
        }

        /// <summary>
        /// Call this when you sent a BP_REQ_ATTACK
        /// </summary>
        public void DidReqAttack()
        {
            ReqAttack = Current;
        }

        /// <summary>
        /// Call this when you sent a BP_REQ_CAST
        /// </summary>
        public void DidReqCast()
        {
            ReqCast = Current;
        }

        /// <summary>
        /// Call this when you sent a BP_USERCOMMAND
        /// </summary>
        public void DidReqUserCommand()
        {
            ReqUserCommand = Current;
        }

        /// <summary>
        /// Call this when you sent a BP_ACTION
        /// </summary>
        public void DidReqAction()
        {
            ReqAction = Current;
        }

        /// <summary>
        /// Call this when you said something
        /// </summary>
        public void DidSay()
        {
            Say = Current;
        }

        /// <summary>
        /// Call this when you broadcasted something
        /// </summary>
        public void DidBroadcast()
        {
            Broadcast = Current;
        }

        /// <summary>
        /// Call this when you interacted with an ID, like
        /// sent a BP_REQ_USE or BP_REQ_APPLY for an item.
        /// </summary>
        public void DidInteract(uint ID)
        {
            Interactions[ID] = Current;
        }
        #endregion
    }
}
