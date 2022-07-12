using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace WebAPI.Helper
{
    public class Throttler
    {
        private int _requestLimit;
        private int _timeoutInSeconds;
        private string _key;

        public Throttler(string key, int requestLimit = 1, int timeoutInSeconds = 10)
        {
            _requestLimit = requestLimit;
            _timeoutInSeconds = timeoutInSeconds;
            _key = key;
        }

        public bool RequestShouldBeThrottled()
        {
            ThrottleInfo throttleInfo = (ThrottleInfo)HttpRuntime.Cache[_key];

            if (throttleInfo == null) throttleInfo = new ThrottleInfo
            {
                ExpiresAt = DateTime.Now.AddSeconds(_timeoutInSeconds),
                RequestCount = 0
            };

            throttleInfo.RequestCount++;

            HttpRuntime.Cache.Add(_key,
          throttleInfo,
          null,
          throttleInfo.ExpiresAt,
          Cache.NoSlidingExpiration,
          CacheItemPriority.Normal,
          null);

            return (throttleInfo.RequestCount > _requestLimit);
        }

        private class ThrottleInfo
        {
            public DateTime ExpiresAt { get; set; }
            public int RequestCount { get; set; }
        }
    }
}