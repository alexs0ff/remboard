using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Helpers
{
    public static class GridViewCustomBindingSummaryCache
    {
        static HttpContext Context { get { return HttpContext.Current; } }

        static Dictionary<string, int> Cache(string cacheKey)
        {
            if (Context.Items[cacheKey] == null)
                Context.Items[cacheKey] = new Dictionary<string, int>();
            return (Dictionary<string, int>)Context.Items[cacheKey];
        }

        public static bool TryGetCount(string casheKeyId,string key, out int count)
        {
            count = 0;
            if (!Cache(casheKeyId).ContainsKey(key))
                return false;
            count = Cache(casheKeyId)[key];
            return true;
        }
        public static void SaveCount(string casheKeyId, string key, int count)
        {
            Cache(casheKeyId)[key] = count;
        }
    }
}