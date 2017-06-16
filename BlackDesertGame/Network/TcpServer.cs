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
using BlackDesertGame.Network.Protocol;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Server;
using NLog;

namespace BlackDesertGame.Network
{
    /*
     * @autor Karyzir, Maxes727
     * */
    public class TcpServer
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected string BindAddress;
        protected int BindPort;
        protected int MaxConnections;
        public Dictionary<IScsServerClient, Connection> Connections;

        public IScsServer Server;

        public TcpServer(string bindAddress, int bindPort, int maxConnections)
        {
            BindAddress = bindAddress;
            BindPort = bindPort;
            MaxConnections = maxConnections;
            Connections = new Dictionary<IScsServerClient, Connection>();
        }

        public void BeginListening()
        {
            Log.Info("Start TcpServer at {0}:{1}...", BindAddress, BindPort);
            Server = ScsServerFactory.CreateServer(new ScsTcpEndPoint(BindAddress, BindPort));
            Server.Start();

            Server.ClientConnected += OnConnected;
            Server.ClientDisconnected += OnDisconnected;
        }

        public void ShutdownServer()
        {
            Log.Info("Shutdown TcpServer...");
            Server.Stop();
        }

        protected void OnConnected(object sender, ServerClientEventArgs e)
        {
            if (Connections.Count >= MaxConnections)
            {
                e.Client.Disconnect();
                return;
            }

            Log.Info("Client connected!");
            Connections.Add(e.Client, new Connection(e.Client, true));
        }

        protected void OnDisconnected(object sender, ServerClientEventArgs e)
        {
            Connections.Remove(e.Client);
            Log.Info("Client disconnected!");
        }
    }
}
