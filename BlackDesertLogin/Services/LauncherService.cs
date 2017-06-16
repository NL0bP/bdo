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

using BDCommon.Network.Contracts;
using BlackDesertLogin.Configurations;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using NLog;

namespace BlackDesertLogin.Services
{
    class LauncherService
    {
        /*
         * @author Karyzir
         * 
         */
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static IScsServiceApplication _lservice;

        /// <summary>
        /// Initilize launcher service
        /// </summary>
        public static void Init()
        {
            _lservice = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(ServicesConfigs.Default.LauncherService_ip, ServicesConfigs.Default.LauncherService_port));
            _lservice.AddService<ILauncherContract, Bridge>(new Bridge());
            _lservice.Start();

            Log.Info("LauncherService started at {0}:{1}", ServicesConfigs.Default.LauncherService_ip,
                                                                                  ServicesConfigs.Default.LauncherService_port);
        }

        public class Bridge : ScsService, ILauncherContract
        {
            public string RegisterClientToken(string name, string password)
            {
                return AuthService.SetTokenForAccount(name,password);
            }
        }
    }
}
