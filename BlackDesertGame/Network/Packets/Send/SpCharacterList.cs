using System.Collections.Generic;
using System.IO;
using BDCommon.Structures.Creature.Player;
using BDCommon.Structures.Player;
using BDCommon.Utils;

namespace BlackDesertGame.Network.Packets.Send
{
    public class SpCharacterList : ASendPacket
    {
        protected List<Player> Players;
        protected static byte[] Data0, Data1, Data2;

        public SpCharacterList(List<Player> players)
        {
            Players = players;
            Data0 = ("9E0E00000034610043000000000000000000FFFFFFFFFFFFFFFF0000000000000000").ToBytes();
            Data1 = ("00000000009EDB45FEF6" +
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
    "0000").ToBytes();
            Data2 = ("FFFFFFFF" +
    "FFFFFFFF0032420A0000000100000000" +
    "000000E0000000000000000000000000" +
    "00FFFFFFFFFFFFFFFF00000000000000" +
    "0000000000000000000000F0F58C170B" +
    "00").ToBytes();

        }
        public override void Write(BinaryWriter writer)
        {
            WriteB(writer, Data0);
            WriteC(writer, (byte) Players.Count);
            int unk = 0;
            foreach (Player t in Players)
            {
                switch (t.CharacterData.Race)
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
                WriteD(writer, t.PlayerId);
                WriteD(writer, t.AccountId);
                WriteC(writer, (byte) unk++);
                WriteS(writer, t.CharacterData.Name);
                WriteB(writer, new byte[22 - ((t.CharacterData.Name.Length * 2) + 2)]);
                WriteB(writer, Data1);
                WriteC(writer, t.CharacterData.Face);
                WriteC(writer, t.CharacterData.Hair);
                WriteC(writer, t.CharacterData.Unk);
                WriteC(writer, 0);
                WriteC(writer, 0);
                WriteC(writer, 0);
                WriteC(writer, (byte) t.CharacterData.Zodiac);
                WriteB(writer, new byte[38]);
                WriteB(writer, t.CharacterData.CharacterDatas);
                WriteB(writer, Data2);
            }
        }
    }
}
