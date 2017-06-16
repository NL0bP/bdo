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
using BDCommon.EngineInterfaces;
using BlackDesertGame.Handlers;
using BlackDesertGame.Network.Protocol;
using BDCommon.Scripts.Chat;
using BDCommon.Scripts;
using NLog;
using System.IO;

namespace BlackDesertGame.Engines
{
    /* *
     * @author karyzir, Maxes727
     */

    class AdminEngine
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();


        public static Dictionary<AdminCommandAttribute, Type> Commands =
            new Dictionary<AdminCommandAttribute, Type>();

        private static ScriptLoader<IAdminEngine> Loader = new ScriptLoader<IAdminEngine>();
        public static void Init()
        {
            foreach (string path in Directory.GetFiles(".\\Scripts\\Chat", "*.cs"))
            {
                Type t = Loader.LoadScript(path);
                if (t == null)
                    continue;
                object[] objs = t.GetCustomAttributes(typeof(AdminCommandAttribute), false);
                if (objs.Length < 1)
                    continue;
                AdminCommandAttribute cmdAttr = (AdminCommandAttribute)objs[0];
                Log.Debug("Loaded custom command {0}", cmdAttr.Command);
                Commands.Add(cmdAttr, t);
            }

            ChatHandler.OnSendMessage += ProccesActionCommand;
        }

        public static bool ProccesActionCommand(ChatMessage message)
        {
            if (!message.Message.StartsWith("//"))
                return false;

            string msg = message.Message.Replace("//", "");
            string cmdName = msg.Split(' ')[0];
            msg = msg.Replace(cmdName + " ", "");

            KeyValuePair<AdminCommandAttribute, Type> kv = Commands.FirstOrDefault(p => p.Key.Command == cmdName);
            if (kv.Value == null)
                return false;

            if (kv.Key.AccessLevel > ((Connection)message.Sender.Connection).AccountInfo.AccessLevel)
                return false;

            var command = (IAdminEngine)Activator.CreateInstance(kv.Value);
            try
            {
                command.ProcessAction(message.Sender, msg);
            }
            catch (Exception e)
            {
                Log.Warn("[ADMIN ENGINE] Somethins wrong! {0}", e);
            }

            return true;
        }

        /*
        public static bool ProccesActionCommand(Connection connection, string message)
        {
            if (!message.StartsWith("//"))
                return false;

            message = message.Replace("//", "");
            string cmdName = message.Split(' ')[0];
            string msg = message.Replace(cmdName + " ", "");

            KeyValuePair<AdminCommandAttribute, Type> kv = Commands.FirstOrDefault(p => p.Key.Command == cmdName);
            if (kv.Value == null)
                return false;

            if (kv.Key.AccessLevel <= connection.AccountInfo.AccessLevel)
                return false;

            var command = (IAdminEngine)Activator.CreateInstance(kv.Value);
            try
            {
                command.ProcessAction(connection, message.Replace(cmdName + " ", ""));
            }
            catch (Exception e)
            {
                Log.Warn("[ADMIN ENGINE] Somethins wrong! {0}", e);
            }

            return true;
        }
        */
    }
}
