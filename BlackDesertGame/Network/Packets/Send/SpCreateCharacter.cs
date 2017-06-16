/*
 * This file is part of black desert-emu <http://necroz-project.net/>.
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
/*
 * Authors:karyzir
 */

using System.IO;
using BDCommon.Structures.Creature.Player;
using BDCommon.Structures.Player;
using BDCommon.Utils;
using BlackDesertGame.Network.Protocol;

namespace BlackDesertGame.Network.Packets.Send
{
    public class SpCreateCharacter : ASendPacket
    {
        protected static string Data1, Data2;
        protected Player PlayerData;
        protected Connection Connection;
        public SpCreateCharacter(Player playerData, Connection conn)
        {
            PlayerData = playerData;
            Connection = conn;
            Data1 = 
 ("00000000009EDB45FEF6" +
  "7F000000F08C170B0000000000F4010B" +
  "00340000000000230D0000F028B70100" +
  "00000000000000000000000000000000" +
  "00000000000000000000000000000000" +
  "00000000000000000000000000000000" +
  "00000000000000000000000000000000" +
  "00000000000000000000000000000000" +
  "00000000000000000000000000000000" +
  "00000000000000000000000000000000" +
  "00000000000000000000000000000000" +
  "00000000000000000000000000000000" +
  "00000000000000000000000000000000" +
  "0000");
            Data2 = 
             ("FFFFFFFF" +
              "FFFFFFFF0032420A0000000100000000" +
              "000000E0000000000000000000000000" +
              "00FFFFFFFFFFFFFFFF00000000000000" +
              "0000000000000000000000F0F58C170B" +
              "00");
        }

        public override void Write(BinaryWriter writer)
        {          
            switch (PlayerData.CharacterData.Race)
            {
                    case Race.Warrior:
                    WriteC(writer, 1);
                    break;
                    case Race.Ranger:
                    WriteC(writer, 2);
                    break;
                    case Race.Sorcerer:
                    WriteC(writer, 3);
                    break;
                    case Race.Giant:
                    WriteC(writer, 4);
                    break;
            }
            WriteC(writer, 0);
            WriteD(writer, PlayerData.PlayerId);// player id
            WriteD(writer, Connection.AccountInfo.Id);// account id
            WriteC(writer, 0);
            WriteS(writer, PlayerData.CharacterData.Name);
            WriteB(writer, new byte[22 - ((PlayerData.CharacterData.Name.Length * 2) + 2)]);
            WriteB(writer, Data1.ToBytes());
            WriteC(writer, PlayerData.CharacterData.Face);
            WriteC(writer, PlayerData.CharacterData.Hair);
            WriteC(writer, PlayerData.CharacterData.Unk);
            WriteC(writer, 0);
            WriteC(writer, 0);
            WriteC(writer, 0);
            WriteC(writer, (byte)PlayerData.CharacterData.Zodiac);
            WriteB(writer, new byte[38]);
            WriteB(writer, PlayerData.CharacterData.CharacterDatas);
            WriteB(writer, Data2.ToBytes());
        }
    }
}
