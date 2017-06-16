using BlackDesertGame.Services.PlayerService;

namespace BlackDesertGame.Network.Packets.Recv
{
    class RpExit : ARecvPacket
    {
        public override void Read()
        {
            
        }

        public override void Process()
        {
           PlayerWorldService.LeaveWorld(Connection);
        }
    }
}
