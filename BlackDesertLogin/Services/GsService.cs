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
using BDCommon.Network.Contracts;
using BDCommon.Structures.LoginServer;
using BDCommon.Structures.Player;
using BlackDesertLogin.Configurations;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using NLog;

namespace BlackDesertLogin.Services
{
    class GsService : ScsService, IGameServerContract
    {
        /*
        * @author Karyzir
        * 
        */
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static IScsServiceApplication _gsservice;
        private string _authKey;
        private IScsServiceClient _authedClient;

        public static readonly Dictionary<IScsServiceClient, GsInfo> ConnectedServers = new Dictionary<IScsServiceClient, GsInfo>();

        /// <summary>
        /// Initilize game service
        /// </summary>
        public void Init(string key)
        {
            _authKey = key;
            _gsservice = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(ServicesConfigs.Default.GameServiceIp, ServicesConfigs.Default.GameServicePort));
            _gsservice.AddService<IGameServerContract, GsService>(this);

            _gsservice.ClientConnected += GsserviceOnClientConnected;
 
            _gsservice.ClientDisconnected += (sender, args) =>
            {
                if (ConnectedServers.ContainsKey(args.Client))
                    ConnectedServers.Remove(args.Client);

                Log.Info("GameServer disconnected.");
            };

            _gsservice.Start();
            Log.Info("GameService started at {0}:{1}", ServicesConfigs.Default.GameServiceIp,
                                                                                  ServicesConfigs.Default.GameServicePort);
        }

        private void GsserviceOnClientConnected(object sender, ServiceClientEventArgs serviceClientEventArgs)
        {
            _authedClient = serviceClientEventArgs.Client;
        }

        public bool Auth(string authKey)
        {
            bool isAuthed = authKey == _authKey;

            if (!isAuthed)
                _authedClient.Disconnect();

            return authKey == _authKey;
        }

        public void RegisterGs(GsInfo gsInfo)
        {
            ConnectedServers.Add(_authedClient, gsInfo);
            Log.Info("Server [{0}] has been available", gsInfo.ServerName);
            _authedClient = null;
        }

        public AccountData AccountData(int gamehash)
        {
            return AuthService.GetAccountDataByGameHash(gamehash);
        }
    }
}
