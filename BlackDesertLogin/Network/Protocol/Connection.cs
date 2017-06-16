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
using BDCommon.Network;
using BDCommon.Structures.Player;
using BlackDesertLogin.Network.Packets.Recv;
using BlackDesertLogin.Network.Protocol.WireProtocol;
using Hik.Communication.Scs.Server;
using BDCommon.Utils;
using NLog;

namespace BlackDesertLogin.Network.Protocol
{
    public class Connection : IConnection
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public IScsServerClient Client { get; set; }
        public bool IsValid { get; private set; }

        protected List<byte[]> SendData = new List<byte[]>();

        protected int SendDataSize;

        protected object SendLock = new object();
        public byte[] Buffer;

        public AccountData Account { get; set; }

        public Connection(IScsServerClient client, bool isValid)
        {
            IsValid = isValid;
            Client = client;
            Client.WireProtocol = new GameProtocol();

            Client.Disconnected += Client_Disconnected;
            Client.MessageReceived += Client_MessageReceived;
        }

        void Client_MessageReceived(object sender, Hik.Communication.Scs.Communication.Messages.MessageEventArgs e)
        {
            Message.Message message = (Message.Message)e.Message;
            Buffer = message.Data;

            if (OpCodes.Recv.ContainsKey(message.OpCode))
            {
                ((ARecvPacket)Activator.CreateInstance(OpCodes.Recv[message.OpCode])).Process(this);
            }
            else
            {
                string opCodeLittleEndianHex = BitConverter.GetBytes(message.OpCode).ToHex();
                Log.Debug("Unknown LsPacket opCode: 0x{0}{1} [{2}]",
                                 opCodeLittleEndianHex.Substring(2),
                                 opCodeLittleEndianHex.Substring(0, 2),
                                 Buffer.Length);

                Log.Debug("Data:\n{0}", Buffer.FormatHex());
            }
        }

        void Client_Disconnected(object sender, EventArgs e)
        {
      
        }
        public void CloseConnection(bool force = false)
        {
            
        }

        public void SendDatas(byte[] data)
        {
            Client.SendMessage(new Message.Message { Data = data });
        }
    }
}
