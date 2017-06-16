
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
    class SpLogIn : ASendPacket
    {
        private readonly string _token;

        public SpLogIn(int token)
        {
            _token = token.ToString("D7");
        }
        public override void Write(BinaryWriter writer)
        {
            WriteS(writer, _token);
            WriteB(writer, "E30100000000FEFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFA00CDB27FF070000BD0B000000000000A402E30100000000000005C3B171F6010000001FD84C5E00000000D68CBC080200000007B2472C00000000ED44BF1F01000000EE9D6A9801000000AB93B0FA00000000FE364C53010000009D05D58D000000007D2C7A2A000000008DC7219200000000D2F1AA81000000005A8B4963010000009DD96D6B0100000099744D7A000000007B333FF501000000F35CAFA701000000F4E79719000000003C69844002000000DC84792B00000000");
        }
    }
}
