/*
 * This file is part of black desert-emu <http://necroz-team.net>.
 *  
 * black desert-emu is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * black desert-emu is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *  
 * You should have received a copy of the GNU General Public License
 * along with black desert-emu. If not, see <http://www.gnu.org/licenses/>.
 */
#region Authors
//Karyzir aka Tanatos
#endregion

using System;
using BDCommon.Structures.Creature.Player;

namespace BDCommon.Structures.Player
{
    //TODO
    [Serializable, ProtoBuf.ProtoContract]
    public class CharacterData
    {
         [ProtoBuf.ProtoMember(1)]
        public Race Race;
         [ProtoBuf.ProtoMember(2)]
        public string Name;
         [ProtoBuf.ProtoMember(3)]
        public Zodiac Zodiac;
         [ProtoBuf.ProtoMember(4)]
        public byte Face;
         [ProtoBuf.ProtoMember(5)]
        public byte Hair;
         [ProtoBuf.ProtoMember(6)]
        public byte[] CharacterDatas;
         [ProtoBuf.ProtoMember(7)]
        public byte Unk;
    }

    public class Tatoo
    {
        public byte Type;
        public Geometry Location;
        public Tanned ColorRgb;
        public byte Transparency;
        public byte Concetration;
    }

    public class Tanned
    {
        public byte R;
        public byte G;
        public byte B;
    }

    public class Eyes
    {
        public byte Eyelashes;
        public Tanned EyelashesColorRgb;
        public byte EyelashesDensity;
        public byte UpperEyelid;
        public Tanned UpperColorRgb;
        public byte UpperEyelidDensity;
        public byte LowerEyelid;
        public byte LowerEyelidDensity;
    }

    public class Geometry
    {
        public byte PosX;
        public byte PosY;
        public byte SizeX;
        public byte SizeY;
        public byte Rotation;
    }
}
