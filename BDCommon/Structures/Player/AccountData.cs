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

using System;
using BDCommon.Utils;

namespace BDCommon.Structures.Player
{
    [Serializable, ProtoBuf.ProtoContract]
    public class AccountData
    {
        [ProtoBuf.ProtoMember(1)]
        public int Id;

        [ProtoBuf.ProtoMember(2)]
        public string Login;

        [ProtoBuf.ProtoMember(3)]
        public string Password;

        public int AccessLevel;

        public LoginToken LoginToken;
    }

    [Serializable]
    public class LoginToken
    {
        public readonly string Key;

        public readonly int TokenCreationTime = Funcs.GetRoundedUtc();

        public LoginToken(string key)
        {
            Key = key;
        }
    }
    [Serializable]
    public class AuthToken
    {
        public readonly int Key;

        public readonly int TokenCreationTime = Funcs.GetRoundedUtc();

        public AuthToken(int key)
        {
            Key = key;
        }
    }
}
