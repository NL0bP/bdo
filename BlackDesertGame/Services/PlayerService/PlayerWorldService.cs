using System.Linq;
using BlackDesertGame.Network.Packets.Send;
using BlackDesertGame.Network.Protocol;

namespace BlackDesertGame.Services.PlayerService
{
    /* *
     * @author Karyzir, Maxes727
     */
    public static class PlayerWorldService
    {
        public static void SendCharacterList(Connection connection)
        {
            new SpCharacterList(
                PlayerService.Players.Where(p => p.AccountId == connection.AccountInfo.Id).ToList()).Send(connection, 1);
        }

        public static void EnterInWorld(Connection connection, int characterId)
        {
            //Jesus... TODO
            var player = PlayerService.Players.LastOrDefault(s => s.PlayerId == characterId);

            if (player == null)
            {
                connection.Client.Disconnect();
                return;
            }

            new SpEnterWorldResponse().Send(connection);
            new SpUnk0E90().Send(connection);
            new SpCharacterInfo(player).Send(connection, 1);
            new SpUnk0Bf0().Send(connection, 1);
            new SpUnk0BFD_1().Send(connection);
            new SpUnk0BFD_2().Send(connection);

            connection.CurrentPlayer = player;
            player.Connection = connection;
        }

        public static void LeaveWorld(Connection connection)
        {
            if (connection.CurrentPlayer != null)
                connection.CurrentPlayer.Connection = null;
            connection.CurrentPlayer = null;

            connection.Client.Disconnect();
        }
    }
}
