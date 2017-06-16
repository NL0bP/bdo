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
using System.Threading;
using BlackDesertLogin.Configurations;
using BlackDesertLogin.Network;
using BDCommon.Utils;
using BDCommon.Database;
using BlackDesertLogin.Services;
using NLog;

namespace BlackDesertLogin
{
    class LoginServer
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        protected static TcpServer Server;
        protected static GsService GameBridgeService;

        public static bool IsWorked { get; private set; }

        static void Main()
        {
            LogManager.Configuration = Funcs.NLogDefaultConfiguration;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            IsWorked = true;
            try
            {
                Started();
            }
            catch (Exception e)
            {               
               Log.ErrorException("Hyuston! We hame some a problem!",e);
            }

            GC.Collect();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            IsWorked = false;
            Log.FatalException("UnhandledException", (Exception)unhandledExceptionEventArgs.ExceptionObject);
            while (true)
                Thread.Sleep(1);
        }

        protected static void Started()
        {
            MysqlDB.Init(new MysqlConfig(MysqlS.Default.Host, MysqlS.Default.Database, MysqlS.Default.User, MysqlS.Default.Password));
            AuthService.Init();
            LauncherService.Init();
            Server = new TcpServer(NetworkS.Default.Ip, NetworkS.Default.Port, NetworkS.Default.MaxConnections);
            GameBridgeService = new GsService();

            GameBridgeService.Init("TRUEPASSWORD");
            Server.BeginListening();
        }
    }
}
