/*
 * This file is part of black desert-emu <http://necroz-project.net/>.
 *  
 * black desert-emu is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * black desert-emu is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *  
 * You should have received a copy of the GNU General Public License
 * along with black desert-emu. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using BDCommon.Database;
using BDCommon.Utils;
using BDCommon.Structures.Player;
using MySql.Data.MySqlClient;
using NLog;

namespace BlackDesertLogin.Database
{
    /*
     * @autor Maxes727
     * */
    class AccountDB : BaseDBReader
    {
        /*
        const string FileDB = "Accounts.cache";
        public static Dictionary<int, AccountData> LoadAll()
        {
            Dictionary<int, AccountData> accounts = null;
            accounts = DeserializeFile<Dictionary<int, AccountData>>(FileDB) ?? new Dictionary<int, AccountData>();
            return accounts;
        }

        public static void SaveAll(Dictionary<int, AccountData> accounts)
        {
            SerializeFile(FileDB, accounts);
        }
        */

        private const string INSERT_QUERY = "INSERT INTO `t_users` (`guid`,`username`,`password`) VALUES (@GUID,@USERNAME,@PASSWORD);";
        private const string SELECT_QUERY = "SELECT * FROM `t_users`;";
        private const string UPDATE_QUERY = "UPDATE `t_users` SET `access_level` = @ACCESSLEVEL WHERE `guid` = @GUID;";
        public static Dictionary<int, AccountData> LoadAll()
        {
            Dictionary<int, AccountData> accounts = new Dictionary<int, AccountData>();
            foreach (var a in MysqlDB.SelectAll(new MysqlQuery(SELECT_QUERY)))
            {
                AccountData data = new AccountData()
                {
                    Id = Convert.ToInt32(a["guid"]),
                    Login = Convert.ToString(a["username"]),
                    Password = Convert.ToString(a["password"]),
                    AccessLevel = Convert.ToInt32(a["access_level"])
                };
                GUIDGenerator.SetGUID(data.Id);
                accounts.Add(data.Id, data);
            }
            return accounts;
        }

        public static void Insert(AccountData data)
        {
            MysqlQuery query = new MysqlQuery(INSERT_QUERY);
            query.AddValue("GUID", data.Id, MySqlDbType.Int32);
            query.AddValue("USERNAME", data.Login.Length > 16 ? data.Login.Substring(0, 16) : data.Login, MySqlDbType.VarChar);
            query.AddValue("PASSWORD", data.Password.Length > 32 ? data.Password.Substring(0, 32) : data.Password, MySqlDbType.VarChar);

            MysqlDB.Query(query);
        }

        public static void UpdateAll(Dictionary<int, AccountData> accounts)
        {
            foreach (var a in accounts)
            {
                MysqlQuery query = new MysqlQuery(UPDATE_QUERY);
                query.AddValue("GUID", a.Value.Id, MySqlDbType.Int32);
                query.AddValue("ACCESSLEVEL", a.Value.AccessLevel, MySqlDbType.Int32);

                MysqlDB.Query(query);
            }
        }
    }
}
