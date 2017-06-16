using BDCommon.Structures.LoginServer;
using BDCommon.Structures.Player;
using Hik.Communication.ScsServices.Service;

namespace BDCommon.Network.Contracts
{
    [ScsService]
    public interface IGameServerContract
    {
        bool Auth(string authKey);
        void RegisterGs(GsInfo gsInfo);
        AccountData AccountData(int gameHash);
    }
}
