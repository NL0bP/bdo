using BlackDesertGame.Handlers;

namespace BlackDesertGame.Network.Packets.Recv
{
    /* *
     * @author Maxes727
     */

    class RpChatMessage : ARecvPacket
    {
        protected string Msg;
        protected MessageType MsgType;
        public override void Read()
        {
            MsgType = (MessageType)ReadC(); //unk
            ReadC(); //unk
            ReadH(); //Length
            Msg = ReadS(); //Message
        }

        public override void Process()
        {
            ChatHandler.SendMessage(Connection.CurrentPlayer, Msg, MsgType);
        }
    }
}