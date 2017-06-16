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
using System.IO;
using System.Text;
using BDCommon.Network;
using BlackDesertLogin.Network.Protocol;
using NLog;

namespace BlackDesertLogin.Network.Packets.Recv
{
    public abstract class ARecvPacket : IRecvPacket
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public BinaryReader Reader;
        public Connection Connection;

        public void Process(Connection connection)
        {
            Connection = connection;

            using (Reader = new BinaryReader(new MemoryStream(connection.Buffer)))
            {
                Read();
            }

            try
            {
                Process();
            }
            catch (Exception ex)
            {
                Log.WarnException("ARecvPacket", ex);
            }
        }

        public abstract void Read();

        public abstract void Process();

        protected int ReadD()
        {
            try
            {
                return Reader.ReadInt32();
            }
            catch (Exception)
            {
                Log.Warn("Missing D for: {0}", GetType());
            }
            return 0;
        }

        protected int ReadC()
        {
            try
            {
                return Reader.ReadByte() & 0xFF;
            }
            catch (Exception)
            {
                Log.Warn("Missing C for: {0}", GetType());
            }
            return 0;
        }

        protected int ReadH()
        {
            try
            {
                return Reader.ReadInt16() & 0xFFFF;
            }
            catch (Exception)
            {
                Log.Warn("Missing H for: {0}", GetType());
            }
            return 0;
        }

        protected double ReadDf()
        {
            try
            {
                return Reader.ReadDouble();
            }
            catch (Exception)
            {
                Log.Warn("Missing DF for: {0}", GetType());
            }
            return 0;
        }

        protected float ReadF()
        {
            try
            {
                return Reader.ReadSingle();
            }
            catch (Exception)
            {
                Log.Warn("Missing F for: {0}", GetType());
            }
            return 0;
        }

        protected long ReadQ()
        {
            try
            {
                return Reader.ReadInt64();
            }
            catch (Exception)
            {
                Log.Warn("Missing Q for: {0}", GetType());
            }
            return 0;
        }

        protected String ReadS()
        {
            Encoding encoding = Encoding.Unicode;
            String result = "";
            try
            {
                short ch;
                while ((ch = Reader.ReadInt16()) != 0)
                    result += encoding.GetString(BitConverter.GetBytes(ch));
            }
            catch (Exception)
            {
                Log.Warn("Missing S for: {0}", GetType());
            }
            return result;
        }

        protected byte[] ReadB(int length)
        {
            byte[] result = new byte[length];
            try
            {
                Reader.Read(result, 0, length);
            }
            catch (Exception)
            {
                Log.Warn("Missing byte[] for: {0}", GetType());
            }
            return result;
        }
    }
}
