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
using BDCommon.Network.Contracts;
using BDCommon.Structures.LoginServer;
using BDCommon.Structures.Player;
using BlackDesertGame.Network.Protocol;
using BlackDesertGame.Services.PlayerService;
using Hik.Communication.Scs.Communication;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using NLog;

namespace BlackDesertGame.Services
{
    class LoginService
    {
        private readonly string _loginIp;
        private readonly short _loginPort;
        private readonly string _authKey;
        private readonly GsInfo _gsInfo;

        private bool _autoConnect;

        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public IScsServiceClient<IGameServerContract> LoginBridge;

        public LoginService(string loginIp, short loginPort, string authKey, GsInfo gsInfo)
        {
            _loginIp = loginIp;
            _loginPort = loginPort;
            _authKey = authKey;
            _gsInfo = gsInfo;
        }

        public void Start(bool autoreconnect = true)
        {
            _autoConnect = autoreconnect;

            ConnectToLogin();
        }

        private void ConnectToLogin()
        {
            LoginBridge =
                ScsServiceClientBuilder.CreateClient<IGameServerContract>(
                    new ScsTcpEndPoint(_loginIp, _loginPort));
            LoginBridge.ConnectTimeout = 3000; //3 second for reconnection
            LoginBridge.Timeout = 1000;
            LoginBridge.Connected += (sender, args) =>
            {
                try
                {
                    LoginBridge.ServiceProxy.Auth(_authKey);
                    LoginBridge.ServiceProxy.RegisterGs(_gsInfo);
                }
                catch (Exception)
                {
                    Log.Error("Login server auth failed!");
                    _autoConnect = false;
                }
            };

            LoginBridge.Disconnected += (sender, args) =>
            {
                
                Log.Error("Login server disconnected");
                TryToConnect();
            };

            TryToConnect();
        }

        private void TryToConnect()
        {
            while (LoginBridge.CommunicationState != CommunicationStates.Connected && _autoConnect)
            {
                Log.Info("Try to connect to Login Server at {0}:{1}", _loginIp, _loginPort);
                try
                {
                    LoginBridge.Connect();
                    Log.Info("Connected to Login Server at {0}:{1}", _loginIp, _loginPort);
                }
                catch
                {
                    if (LoginBridge == null)
                        return;

                    Log.Info("Connection failed, reconnect...");
                    ConnectToLogin();
                }
            }
        }

        public void GetAccountData(Connection conn, string token)
        {
            //TODO
            int hash = int.Parse(token);

            AccountData accData = LoginBridge.ServiceProxy.AccountData(hash);

            if (accData == null)
            {
                conn.Client.Disconnect();
                return;
            }

            conn.AccountInfo = accData;
            PlayerWorldService.SendCharacterList(conn);
        }
    }
}
