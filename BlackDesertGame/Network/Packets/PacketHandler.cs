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
using System.Linq;
using BDCommon.Utils;
using BlackDesertGame.Network.Packets.Recv;
using BlackDesertGame.Network.Packets.Send;
using BlackDesertGame.Network.Protocol;
using BlackDesertGame.Network.Protocol.Message;
using NLog;

namespace BlackDesertGame.Network.Packets
{
    /*
     * @autor Maxes727, Karyzir
     * */
    class PacketHandler
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static Dictionary<ushort, Type> Recv = new Dictionary<ushort, Type>
        {
            {0x0bc0, typeof(RpAuthorize)},
            {0x0BE0, typeof(RpEnterWorld)},
            {0x0BF4, typeof(RpCreateCharacter)},
            {0x0D56, typeof(RpChatMessage)},
            {0x0D57, typeof(RpWhisperMessage)},
            {0x0BC9, typeof(RpExit)},      
        };
        public static Dictionary<Type, ushort> Send = new Dictionary<Type, ushort>
        {
            {typeof (SpCharacterList), 0x0BC1},
            {typeof (SpEnterWorldResponse), 0x0BEF},
            {typeof (SpUnk0E90), 0x0E93},
            {typeof (SpCharacterInfo),   0x0BE1},
            {typeof (SpUnk0Bf0), 0x0BF1},
            {typeof (SpUnk0BFD_1), 0x0BFD},
            {typeof (SpUnk0BFD_2), 0x0BFD},
            {typeof (SpChatMessage), 0x0D5A},
            {typeof (SpCreateCharacter), 0x0BF5},
            {typeof (SpFailCreateCharacter), 0x0BF6},
        };


        public static void SendPacket(Connection con, ASendPacket packet)
        {
            if (con.Client.CommunicationState == Hik.Communication.Scs.Communication.CommunicationStates.Connected && con.IsValid)
                packet.Send(con);
        }

        public static void BroadcastPacket(ASendPacket packet)
        {
            foreach (Connection connection in GameServer.Server.Connections.Select(p => p.Value))
            {
                SendPacket(connection, packet);
            }
        }

        public static ushort GetOpcode(Type t)
        {
            if (!Send.ContainsKey(t))
                throw new NotImplementedException(string.Format("Can't find opcode for packet '{0}'", t.Name));

            return Send[t];
        }

        public static void HandleIncomingPacket(Connection connection, Message message)
        {
            if (Recv.ContainsKey(message.OpCode))
            {
                 Log.Info("Received packet '{0}' with datas:\n{1}", Recv[message.OpCode].Name,message.Data.FormatHex());               
                ((ARecvPacket)Activator.CreateInstance(Recv[message.OpCode])).Process(connection, message.Data);
            }
            else
            {
                Log.Warn("Unknown GsPacket opCode: 0x{0} [{1}]", BitConverter.GetBytes(message.OpCode).Reverse().ToArray().ToHex(), message.Data.Length);

                Log.Warn("Data:\n{0}", message.Data.FormatHex());
            }
        }
    }
}
