namespace BlackDesertGame.Network.Packets.Recv
{
    class RpAuthorize : ARecvPacket
    {
        private string _token;
        public override void Read()
        {
            _token = ReadS();
            //Unk datas
        }

        public override void Process()
        {
           GameServer.LService.GetAccountData(Connection, _token);
        }
    }
}
