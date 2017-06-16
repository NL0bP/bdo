/*
 * This file is part of black desert-emu <http://necroz-team.net>.
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

namespace BDCommon.Utils
{
    /*
     * @autor Maxes727, Karyzir
     * */
    public class TokenGenerator
    {
        const string SALT = @"8vd%h>Jxb6nVTDiU5wc2K<XK!W8>2{q]=.kL,№8A-ZE_Xma!>Q6'izQq_b%ji5Gu";
        public static string GenerateByAccount(AccountData account)
        {
            string token = Funcs.CalculateMd5Hash(account.Login + SALT + account.Password + Funcs.Random().Next(200000));
            account.LoginToken = new LoginToken(token);
            return token;
        }

        public static int GenerateSessionHash()
        {
            return Funcs.Random().Next(1000000, 9999999);
        }
    }
}
