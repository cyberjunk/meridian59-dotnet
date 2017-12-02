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

namespace Meridian59.Data.Models
{
    /// <summary>
    /// ObjectFlagsUpdate for updating ObjectFlags on an object
    /// </summary>
    [Serializable]
    public class ObjectFlagsUpdate : ObjectID
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_FLAGS = "Flags";
        #endregion

        #region IByteSerializable
        public override int ByteLength
        { 
            get
            {
                return base.ByteLength + flags.ByteLength; // ObjectID base (4) + ObjectFlags
            } 
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);                            // ObjectID (4 bytes)

            flags.ReadFrom(Buffer, cursor);                                    // Flags (n bytes)
            cursor += flags.ByteLength;

            return cursor - StartIndex;   
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);                                             // ID (4 bytes)

            cursor += flags.WriteTo(Buffer, cursor);                                            // Flags (n bytes)

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            flags.ReadFrom(ref Buffer);
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            flags.WriteTo(ref Buffer);
        }

        #endregion

        #region Fields
        protected readonly ObjectFlags flags = new ObjectFlags();
        #endregion

        #region Properties
        public ObjectFlags Flags
        {
            get { return flags; }
        }
        #endregion

        #region Constructors
        public ObjectFlagsUpdate() : base()
        {
        }

        public ObjectFlagsUpdate(
            uint ID,
            uint Flags = 0)
            : base(ID)
        {
            this.flags.Value = Flags;
        }

        public ObjectFlagsUpdate(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        public unsafe ObjectFlagsUpdate(ref byte* Buffer)
            : base(ref Buffer) { }

        #endregion

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            flags.Clear(RaiseChangedEvent);
        }
        #endregion

        #region Methods
        #endregion

    }
}
