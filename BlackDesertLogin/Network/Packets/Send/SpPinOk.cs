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

namespace BlackDesertLogin.Network.Packets.Send
{
    class SpPinOk : ASendPacket
    {
        //01 11 00 BC 0B 00 14 C3 00 81 DA 05 78 9B 0E 00 ...&#188;...&#195;.&#218;.x›..
        //00 .

        public SpPinOk()
        {
            
        }
        public override void Write(BinaryWriter writer)
        {
            WriteB(writer, "0014C30081DA05789B0E0000");
        }
    }
}
