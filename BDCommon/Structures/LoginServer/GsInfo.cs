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

namespace BDCommon.Structures.LoginServer
{
    /*
    * @author Karyzir
    * 
    */

    [Serializable]
    public class GsInfo
    {
        public short ServerId { get; set; }

        public string ServerName { get; private set; }

        public string Ip { get; private set; }

        public ushort Port { get; private set; }

        public GsInfo(short serverId, string serverName, string ip, ushort port)
        {
            ServerId = serverId;
            ServerName = serverName;
            Ip = ip;
            Port = port;
        }
    }
}
