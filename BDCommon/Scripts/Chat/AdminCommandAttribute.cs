using System;

namespace BDCommon.Scripts.Chat
{
    public class AdminCommandAttribute : Attribute
    {
        public string Command { get; private set; }
        public int AccessLevel { get; private set; }

        public AdminCommandAttribute(string cmd, int accessLevel)
        {
            Command = cmd;
            AccessLevel = accessLevel;
        }
    }
}
