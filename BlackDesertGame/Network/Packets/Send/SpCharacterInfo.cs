using System.IO;
using BDCommon.Structures.Creature.Player;
using BDCommon.Structures.Player;

namespace BlackDesertGame.Network.Packets.Send
{
    public class SpCharacterInfo : ASendPacket
    {
        private static string _data, _data1, _data11, _data12, _data13, _data2;
        protected Player PlayerData;
        public SpCharacterInfo(Player playerData)
        {
            PlayerData = playerData;
            _data = "010000" +
                     "000100003C2D0000189EC600207EC500" +
                     "CC9847046788BE0000000080BF763F";
            _data1 = "0001000000FA00000058FAFFFFFFFFFF" +
                    "FF58FAFFFFFFFFFFFF58FAFFFFFFFFFF" +
                    "FF02946DE49500000000000000000000" +
                    "00000000000003000000000000000000" +
                    "00000000000000000089004300000000" +
                    "00000001000000";
            _data11 = "02" +
                     "00000001000000030000000000000000" +
                     "006929000000FCFFFF1C000000000000" +
                     "00000000000005B40100000000000000" +
                     "00000000000000000000000000000000" +
                     "00000000000000000000000000000000" +
                     "00000000FCFFFF014B0C000001000000" +
                     "4200440045004D005500000077006E00" +
                     "55007300650072002400000000000000" +
                     "00000000000000000000000000000000" +
                     "00000000000000000000000000000003" +
                     "02000000";
            _data12 = "01C16F";
            _data13 = "010000001000000090000080040012" +
                     "000000B4000009000000000009000000" +
                     "0000";
           _data2 = "00000300000069290000" +
                    "3000CD29000032000000000000000000" +
                    "00000000000000000000000000000000" +
                    "00000000000000000000000000000000" +
                    "00000000000000000000000000000000" +
                    "00000000000000000000000000000000" +
                    "050000000080EA120000000000780256" +
                    "0500000000B026570500000000FD7D94" +
                    "400100000000000000000047543DEDF9" +
                    "7F00000000000000000000000080EA12" +
                    "00000000003054067700000000000000" +
                    "00000058F1E4D0000000001F20010055" +
                    "05000058F16C170B0000001800000000" +
                    "000000000000000000000000000000FA" +
                    "76000000000000000000000000000000" +
                    "000000000000000000000090F1E4D000" +
                    "00000001000000020000000000000095" +
                    "400100000000000000000058F1E4D000" +
                    "000000607FD307000800FCFFFF";


        }
        public override void Write(BinaryWriter writer)
        {
           WriteD(writer, PlayerData.PlayerId);
	       WriteD(writer, PlayerData.AccountId);
           WriteB(writer, _data);
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
           WriteB(writer, _data1);
           WriteD(writer, PlayerData.PlayerId);
           WriteD(writer, PlayerData.AccountId);
           WriteB(writer, _data11);
           WriteS(writer, PlayerData.CharacterData.Name);
           WriteB(writer, new byte[22 - ((PlayerData.CharacterData.Name.Length * 2) + 2)]);
           WriteB(writer, _data12);
           WriteD(writer, PlayerData.AccountId);
           WriteB(writer, _data13);
           WriteD(writer, PlayerData.Level);
           WriteB(writer, _data2);
        }
    }
}
