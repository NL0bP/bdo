using System.IO;

namespace BlackDesertGame.Network.Packets.Send
{
    class SpFailCreateCharacter : ASendPacket
    {
        public override void Write(BinaryWriter writer)
        {
            WriteB(writer, "2284ADB1");
        }
    }
}
