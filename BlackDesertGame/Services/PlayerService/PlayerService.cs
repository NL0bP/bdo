using System.Collections.Generic;
using System.Linq;
using BDCommon.Structures.Player;
using BDCommon.Utils;
using BlackDesertGame.Database;
using BlackDesertGame.Managers.Tasks;
using BlackDesertGame.Network.Packets.Send;
using BlackDesertGame.Network.Protocol;
using NLog;

namespace BlackDesertGame.Services.PlayerService
{
    /* *
     * @author Karyzir, Maxes727
     */
    public class PlayerService
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static List<Player> Players = new List<Player>();
        private static readonly object PlayersLock = new object();

        public static int SaveInterval = 600;

        /// <summary>
        /// Initilize service
        /// </summary>
        public static void Init()
        {
            Players = CharacterDataBase.LoadAll();
            Log.Info("Loaded characters: {0}", Players.Count);
           
            TaskManager.AddTask(SaveCharacters, SaveInterval);
        }

        /// <summary>
        /// Save all accounts from server cache
        /// </summary>
        private static void SaveCharacters()
        {
            Log.Info("Periodic save. Characters count: {0}", Players.Count);
            lock (PlayersLock)
                CharacterDataBase.SaveAll(Players);
        }

        public static void CreateNewCharacter(Connection connection, CharacterData playerData)
        {
            var cached = new Player
            {
                AccountId = connection.AccountInfo.Id,
                CharacterData = playerData
            };
            lock (PlayersLock)
            {
                if (Players.Exists(s => s.CharacterData.Name == playerData.Name))
                {
                    new SpFailCreateCharacter().Send(connection);
                    return;
                }
                cached.PlayerId = GUIDGenerator.NextGUID();
                cached.Level = 1;
                Players.Add(cached);
            }
            new SpCreateCharacter(cached,connection).Send(connection, 1);
            Log.Info("New character created!");
        }

        public static Connection GetConnectionFromCharacterName(string name)
        {
            Connection con = GameServer.Server.Connections.FirstOrDefault(
                p =>
                    p.Value.CurrentPlayer != null &&
                    p.Value.CurrentPlayer.CharacterData.Name.Equals(name)).Value;
            return con;
        }
    }
}
