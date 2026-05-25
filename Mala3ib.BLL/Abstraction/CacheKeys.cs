using System;
using System.Collections.Generic;
using System.Text;

namespace Mala3ib.BLL.Abstraction
{
    public static class CacheKeys
    {
        public static string ProfileKey(string id) => $"profile-{id}";
    }
}
