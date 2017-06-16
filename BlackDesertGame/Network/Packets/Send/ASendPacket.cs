using System;
using System.IO;
using System.Text;
using BDCommon.Network;
using BDCommon.Utils;
using NLog;

namespace BlackDesertGame.Network.Packets.Send
{
    public abstract class ASendPacket : ISendPacket
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        protected byte[] Datas;
        protected object WriteLock = new object();

        public void Send(IConnection state,int clientState = 0)
        {
            if (state == null || !state.IsValid)
                return;

            if (!PacketHandler.Send.ContainsKey(GetType()))
            {
                Log.Warn("UNKNOWN packet opcode: {0}", GetType().Name);
                return;
            }
            lock (WriteLock)
            {
                ushort opCode = PacketHandler.GetOpcode(GetType());
                if (Datas == null)
                {
                    try
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            using (BinaryWriter writer = new BinaryWriter(stream, new UTF8Encoding()))
                            {
                                writer.Write((byte)0);
                                writer.Write((ushort)0);
                                writer.Write(opCode);

                                Write(writer);

                                stream.Position = 0;

                                writer.Write((byte)clientState);
                                writer.Write((ushort)stream.Length);
                            }

                            Datas = stream.ToArray();
                            Log.Debug("Send packet '{0}'({2})({3}) with datas:\n{1}", GetType().Name, Datas.FormatHex(), opCode.ToString("X4"), Datas.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Can't write packet: {0}", GetType().Name);
                        Log.WarnException("ASendPacket", ex);
                        return;
                    }
                }
            }

            state.SendDatas(Datas);
        }

        public abstract void Write(BinaryWriter writer);

        protected void WriteD(BinaryWriter writer, int val)
        {
            writer.Write(val);
        }

        protected void WriteH(BinaryWriter writer, short val)
        {
            writer.Write(val);
        }

        protected void WriteC(BinaryWriter writer, byte val)
        {
            writer.Write(val);
        }

        protected void WriteDf(BinaryWriter writer, double val)
        {
            writer.Write(val);
        }

        protected void WriteF(BinaryWriter writer, float val)
        {
            writer.Write(val);
        }

        protected void WriteQ(BinaryWriter writer, long val)
        {
            writer.Write(val);
        }

        protected void WriteS(BinaryWriter writer, String text)
        {
            if (text == null)
            {
                writer.Write((short)0);
            }
            else
            {
                Encoding encoding = Encoding.Unicode;
                writer.Write(encoding.GetBytes(text));
                writer.Write((short)0);
            }
        }

        protected void WriteB(BinaryWriter writer, string hex)
        {
            writer.Write(hex.ToBytes());
        }

        protected void WriteSs(BinaryWriter writer, string msg)
        {
            WriteH(writer, (short)(msg.Length * 2 + 2));
            WriteS(writer, msg);
        }

        protected void WriteB(BinaryWriter writer, byte[] data)
        {
            writer.Write(data);
        }
    }
}
