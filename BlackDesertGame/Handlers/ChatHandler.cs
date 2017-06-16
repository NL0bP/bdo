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

using BDCommon.Structures.Player;
using BlackDesertGame.Network.Protocol;
using BlackDesertGame.Network.Packets.Send;
using BlackDesertGame.Network.Packets;
using BlackDesertGame.Services.PlayerService;

namespace BlackDesertGame.Handlers
{
    public class ChatHandler
    {
        public delegate bool ChatHandlerDelegate(ChatMessage msg);

        public static event ChatHandlerDelegate OnSendMessage;

        public static void SendMessage(Player senderPlayer, string msg, MessageType msgType = MessageType.GENERAL, SendMessageType sendType = SendMessageType.BROADCAST)
        {
            SendMessage(new ChatMessage(senderPlayer, msg, msgType, sendType));
        }

        public static void SendWhisperMessage(Player sender, string msg, string target)
        {
            Connection t = PlayerService.GetConnectionFromCharacterName(target);
            if (t == null)
                return;
            ChatMessage message = new ChatMessage(sender, msg, MessageType.WHISPER, SendMessageType.PRIVATE);
            PacketHandler.SendPacket((Connection)sender.Connection, new SpChatMessage(message));
            PacketHandler.SendPacket(t, new SpChatMessage(message));
        }

        public static void SendMessage(ChatMessage message)
        {
            if (!OnSendMessage(message))
            switch (message.SendType)
            {
                case SendMessageType.BROADCAST:
                    PacketHandler.BroadcastPacket(new SpChatMessage(message));
                    break;
                case SendMessageType.PRIVATE:
                    PacketHandler.SendPacket((Connection)message.Sender.Connection, new SpChatMessage(message));
                    break;
            }
        }
    }

    public class ChatMessage
    {
        public Player Sender { get; private set; }
        public string Message { get; private set; }

        public SendMessageType SendType { get; private set; }

        public MessageType MsgType { get; set; }

        public ChatMessage(Player sender, string msg, SendMessageType sendType = SendMessageType.BROADCAST) : this(sender, msg, MessageType.GENERAL, sendType) { }

        public ChatMessage(Player sender, string msg, MessageType msgType, SendMessageType sendType)
        {
            Sender = sender;
            Message = msg;
            MsgType = msgType;
            SendType = sendType;
            if(MsgType == MessageType.WHISPER)
                SendType = SendMessageType.PRIVATE;
        }
    }

    public enum MessageType : byte
    {
        //TODO
        //Need check all!!!
        SYSTEM = 1, 
        SHOUT = 2,
        GENERAL = 3,
        WHISPER = 4,
        SYSTEM2 = 5, 
        GUILD = 6, 
        GROUP = 7 
    }

    public enum SendMessageType
    {
        PRIVATE,
        BROADCAST
    }
}
