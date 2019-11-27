using System;

namespace PharmacyApp.Constants
{
    using System;
    public class CacheSetting
    {
        public static class SitemapNodes
        {
            public const string Key = "SitemapNodes";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(1);
        }

    }
}