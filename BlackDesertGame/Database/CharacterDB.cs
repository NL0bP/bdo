using System;
using System.Collections.Generic;
using System.Linq;
using BDCommon.Database;
using BDCommon.Structures.Player;
using BDCommon.Utils;

namespace BlackDesertGame.Database
{
    class CharacterDataBase : BaseDBReader
    {
        const string FileDb = "Players.cache";

        public static List<Player> LoadAll()
        {
            List<Player> players = DeserializeFile<List<Player>>(FileDb);
            if (players != null)
            {
                GUIDGenerator.SetGUID(players.Max(p => p.PlayerId));
            }
            return players ?? new List<Player>();
        }

        public static void SaveAll(List<Player> players)
        {
            SerializeFile(FileDb, players);
        }
    }
}
