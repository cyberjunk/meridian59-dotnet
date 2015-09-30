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
#if !VANILLA

using Meridian59.Protocol.Enums;
using Meridian59.Data.Models;

namespace Meridian59.Protocol.GameMessages
{
	public class StopWaveMessage : GameModeMessage
	{		
		#region IByteSerializable
		public override int ByteLength
		{
			get
			{
				return base.ByteLength + PlayInfo.ByteLength;
			}
		}

		public override int WriteTo(byte[] Buffer, int StartIndex = 0)
		{
			int cursor = StartIndex;

			cursor += base.WriteTo(Buffer, cursor);
			cursor += PlayInfo.WriteTo(Buffer, cursor);

			return cursor - StartIndex;
		}

		public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
		{
			int cursor = StartIndex;

			cursor += base.ReadFrom(Buffer, cursor);

			PlayInfo = new StopSound(Buffer, cursor);
			cursor += PlayInfo.ByteLength;

			return cursor - StartIndex;
		}

		public override unsafe void WriteTo(ref byte* Buffer)
		{
			base.WriteTo(ref Buffer);

			PlayInfo.WriteTo(ref Buffer);
		}

		public override unsafe void ReadFrom(ref byte* Buffer)
		{
			base.ReadFrom(ref Buffer);

			PlayInfo = new StopSound(ref Buffer);
		}
		#endregion

		public StopSound PlayInfo { get; set; }

		public StopWaveMessage(StopSound PlayInfo) 
			: base(MessageTypeGameMode.StopWave)
		{
			this.PlayInfo = PlayInfo;
		}

		public StopWaveMessage(byte[] Buffer, int StartIndex = 0) 
			: base (Buffer, StartIndex = 0) { }

		public unsafe StopWaveMessage(ref byte* Buffer)
			: base(ref Buffer) { }
	}
}
#endif
