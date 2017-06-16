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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BDCommon.Structures.Player;
using BDCommon.Utils;
using BlackDesertLogin.Network.Protocol;
using BlackDesertLogin.Network.Packets.Send;
using BlackDesertLogin.Database;
using NLog;


namespace BlackDesertLogin.Services
{
    /*
     * @autor Karyzir, Maxes727
     * */
    class AuthService
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static Dictionary<int, AccountData> _accounts = new Dictionary<int, AccountData>();
        private static Dictionary<string, AccountData> _tokens = new Dictionary<string, AccountData>();
        private static Dictionary<int,int> _gameHashes = new Dictionary<int, int>(); 

        private static readonly object AccountsLock = new object();

        public static Thread DbSaveThread;
        public static int SaveInterval = 600;
        private static int _lastSaveUtc = Funcs.GetRoundedUtc();

        static AuthService()
        {
            DbSaveThread = new Thread(() =>
            {
                while (LoginServer.IsWorked)
                {
                    if (Funcs.GetRoundedUtc() - _lastSaveUtc > SaveInterval)
                    {
                        SaveAccounts();
                        _lastSaveUtc = Funcs.GetRoundedUtc();
                    }
                    Thread.Sleep(1000);
                }
            });
            DbSaveThread.Start();
        }
        /// <summary>
        /// Initilize service
        /// </summary>
        public static void Init()
        {
            _accounts = AccountDB.LoadAll();

            Log.Info("Loaded {0} accounts.", _accounts.Count);
        }
        /// <summary>
        /// Save accounts
        /// </summary>
        private static void SaveAccounts()
        {
            Log.Info("Periodic save. Accounts count: {0}", _accounts.Count);
            lock (AccountsLock)
                AccountDB.UpdateAll(_accounts);
        }
        /// <summary>
        /// Foreach AccountData from Token storage and return AccountData
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static AccountData GetAccountDataByToken(string token)
        {
            if (_tokens.ContainsKey(token))
                return _tokens[token];
            return null;
        }

        public static AccountData GetAccountDataByGameHash(int gameHash)
        {
            foreach (int accId in _gameHashes.Where(s => s.Value == gameHash).Select(infos => infos.Key))
            {
                return _accounts[accId];
            }
            _gameHashes.Remove(gameHash);//Remove gamehash
            return null;
        }

        public static string SetTokenForAccount(string account, string passwd)
        {
            var acc = _accounts.FirstOrDefault(ai => ai.Value.Login == account && ai.Value.Password == Funcs.CalculateMd5Hash(passwd)).Value;
            if (acc == null)
            {
                if (_accounts.Values.Any(inf => inf.Login.ToLower() == account.ToLower()))
                {
                    return null;
                }
                lock (AccountsLock)
                {
                    acc = new AccountData { Id = GUIDGenerator.NextGUID(), Login = account, Password = Funcs.CalculateMd5Hash(passwd) };
                    _accounts.Add(acc.Id, acc);
                    AccountDB.Insert(acc);
                    Log.Info("Account '{0}' created", account);
                }
            }
            string token = TokenGenerator.GenerateByAccount(acc);
            int gameHash = TokenGenerator.GenerateSessionHash();

            if (_gameHashes.ContainsKey(acc.Id))
            {
                _gameHashes.Remove(acc.Id);
            }

            _gameHashes.Add(acc.Id, gameHash);
            _tokens.Add(token, acc);

            Log.Info("Set token '{0}' from account: {1}", token, account);
            Log.Info("Set GameHash '{0}' from account: {1}", gameHash, account);

            return token;
        }

        public static void ValidateAuthToken(Connection con, string token)
        {
            if (con.Account != null)
            {
                con.CloseConnection(true);
                return;
            }
            AccountData account = GetAccountDataByToken(token);
            if (account == null)
            {
                Log.Info("Invalid token from client: {0}", con.Client.RemoteEndPoint.ToString());
                con.CloseConnection(true);
                return;
            }
            int time = Funcs.GetRoundedUtc();
            if ((time - account.LoginToken.TokenCreationTime) > 10 * 60)
            {
                Log.Info("Token '{0}' time expired from client: {1}", token, con.Client.RemoteEndPoint.ToString());
                con.CloseConnection(true);
                return;
            }
            new SpLogIn(_gameHashes[account.Id]).Send(con);//Sending individual hashes

            _tokens.Remove(account.LoginToken.Key);//Remove Auth Token

            account.LoginToken = null;
            con.Account = account;

 
        }
    }
}
