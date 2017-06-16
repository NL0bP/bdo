
using System;

namespace BDCommon.Utils
{
    public static class GUIDGenerator
    {
        private static int _maxGUID = 0;
        private static object lockGUID = new object();

        public static void SetGUID(int id)
        {
            lock(lockGUID)
                _maxGUID = _maxGUID < id ? id : _maxGUID;
        }

        public static int NextGUID()
        {
            lock (lockGUID)
                return ++_maxGUID;
        }
    }
}
