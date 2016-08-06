using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.Business
{
    internal static class UniqueKeyName
    {
        public static string CreateUniqueKey(string term)
        {
            // just use the current time as a salt, don't feel like doing a GUID stuff.
            return string.Format("{1}-{0}", DateTime.UtcNow.ToBinary().ToString("x2"), term);
        }
    }
}
