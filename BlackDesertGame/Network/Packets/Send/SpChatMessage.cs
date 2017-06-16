using System.IO;
using BDCommon.Structures.Player;
using BlackDesertGame.Handlers;

namespace BlackDesertGame.Network.Packets.Send
{
    /* *
     * @author Maxes727
     */

    public class SpChatMessage : ASendPacket
    {
        protected ChatMessage Msg;
        public SpChatMessage(ChatMessage msg)
        {
            Msg = msg;
        }

        public override void Write(BinaryWriter writer)
        {
            /*
            string data = "0101001C9200780054007A"
            + "00000000000404005C1C004D00650072"
            + "006300790000000CD580B70000000000"
            + "0000000000DD0200000000A7557CC1FD"
            + "7F0000100000000000";
            */

            Player player = Msg.Sender;

            WriteC(writer, (byte)Msg.MsgType); //0x03
            WriteH(writer, 0x04); //0x04

            WriteC(writer, 0x1C);
            WriteH(writer, 0x92);

            WriteS(writer, player.CharacterData.Name);
            WriteB(writer, new byte[62 - (player.CharacterData.Name.Length + 1) * 2]);
            
            WriteSs(writer, Msg.Message);
        }
    }
}
