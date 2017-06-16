using BDCommon.Structures.Creature.Player;
using BDCommon.Structures.Player;
using BlackDesertGame.Services.PlayerService;

namespace BlackDesertGame.Network.Packets.Recv
{
    class RpCreateCharacter : ARecvPacket
    {
        protected CharacterData CharacterData;
        public override void Read()
        {
            CharacterData = new CharacterData();
            CharacterData.Race = (Race) ReadC();
            ReadC();
            CharacterData.Name = ReadS();
            int size = 60 - (CharacterData.Name.Length * 2);
            ReadB(size);
            CharacterData.Zodiac = (Zodiac) ReadC();
            CharacterData.Face = (byte) ReadC();
            CharacterData.Hair = (byte) ReadC();
            CharacterData.Unk = (byte)ReadC();
            ReadB(41);
            CharacterData.CharacterDatas = ReadB(989);
        }

        public override void Process()
        {
           PlayerService.CreateNewCharacter(Connection, CharacterData);
        }
    }
}
