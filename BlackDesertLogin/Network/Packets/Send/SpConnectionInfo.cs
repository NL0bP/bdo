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
using System.IO;
using System.Linq;
using BlackDesertLogin.Services;

namespace BlackDesertLogin.Network.Packets.Send
{
    class SpConnectionInfo : ASendPacket
    {
        public override void Write(BinaryWriter writer)
        {
            WriteH(writer, (short)GsService.ConnectedServers.Count);
            foreach (var info in GsService.ConnectedServers.Select(kv => kv.Value))
            {
                WriteH(writer, 11); //Unk
                WriteH(writer, info.ServerId);
                WriteS(writer, info.ServerName.Length > 30 ? info.ServerName.Substring(0, 30) : info.ServerName);
                WriteB(writer, new byte[60 - info.ServerName.Length * 2]);
                WriteIP(writer, info.Ip);
                WriteC(writer, 0);
                WriteH(writer, (short)info.Port);
                WriteC(writer, 90);
            }
        }
    }
}
