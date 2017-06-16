using BDCommon.Structures.Player;

namespace BDCommon.EngineInterfaces
{
    public interface IAdminEngine
    {
        void ProcessAction(Player connection, string message);
    }
}
