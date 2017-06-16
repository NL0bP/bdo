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
using BDCommon.Structures.LoginServer;
using BDCommon.Utils;
using BlackDesertGame.Managers.Tasks;
using BlackDesertGame.Network;
using BlackDesertGame.Services;
using BlackDesertGame.Engines;
using BlackDesertGame.Services.PlayerService;
using NLog;

namespace BlackDesertGame
{
    class GameServer
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static TcpServer Server;
        public static LoginService LService;

        public static bool IsWorked { get; private set; }

        static void Main()
        {
            LogManager.Configuration = Funcs.NLogDefaultConfiguration;
           
            IsWorked = true;
            try
            {
                Started();
            }
            catch (Exception e)
            {              
                Log.FatalException("Hyuston! We have some problem!", e);
                Console.Read();
            }
        }
        private static void Started()
        {
            Server = new TcpServer("127.0.0.1", 8889, 100);
            LService = new LoginService(
                "127.0.0.1", 6668, "TRUEPASSWORD",
                          new GsInfo(1, "NecrozEMU", "127.0.0.1", 8889));
            LService.Start();

            AdminEngine.Init();
            PlayerService.Init();
            TaskManager.Init();

            Server.BeginListening();
        }
    }
}
