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
using BlackDesertLogin.Network.Packets.Recv;
using BlackDesertLogin.Network.Packets.Send;

namespace BlackDesertLogin.Network.Protocol
{
    class OpCodes
    {
        public static Dictionary<ushort, Type> Recv = new Dictionary<ushort, Type>
        {
            {0x0bb9, typeof(RpLogIn)},
            {0x0bbb, typeof(RpEnterPin)},
        

        };
        public static Dictionary<Type, ushort> Send = new Dictionary<Type, ushort>
        {
              {typeof (SpLogIn), 0x0bba},
              {typeof (SpPinOk), 0x0bbc},
              {typeof (SpConnectionInfo), 0x0bbf},


        };
    }
}
