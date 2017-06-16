using BlackDesertGame.Handlers;

namespace BlackDesertGame.Network.Packets.Recv
{
    /* *
     * @author Maxes727
     */

    class RpWhisperMessage : ARecvPacket
    {
        private string target;
        private string msg;
        public override void Read()
        {
            target = ReadS();
            ReadB(62 - (target.Length * 2 + 2));
            ReadH(); // message Length
            msg = ReadS();
        }

        public override void Process()
        {
            ChatHandler.SendWhisperMessage(Connection.CurrentPlayer, msg, target);
        }
    }
}