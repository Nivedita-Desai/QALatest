﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace WebAPI.Helper
{
    
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class RequestThrottling : ActionFilterAttribute
        {
            /// <summary>
            /// A unique name for this Throttle.
            /// </summary>
            /// <remarks>
            /// We'll be inserting a Cache record based on this name and client IP, e.g. "name-192.168.0.1"
            /// </remarks>
            public string Name { get; set; }

            /// <summary>
            /// The number of seconds clients must wait before executing this decorated route again.
            /// </summary>
            public int Seconds { get; set; }

            /// <summary>
            /// A text message that will be sent to the client upon throttling.  You can include the token {n} to
            /// show this.Seconds in the message, e.g. "Wait {n} seconds before trying again".
            /// </summary>
            public string Message { get; set; }

            public override void OnActionExecuting(ActionExecutingContext c)
            {
                var key = string.Concat(Name, "-", c.HttpContext.Request.UserHostAddress);
                var allowExecute = false;

                if (HttpRuntime.Cache[key] == null)
                {
                    HttpRuntime.Cache.Add(key,
                        true, // is this the smallest data we can have?
                        null, // no dependencies
                        DateTime.Now.AddSeconds(Seconds), // absolute expiration
                        Cache.NoSlidingExpiration,
                        CacheItemPriority.Low,
                        null); // no callback

                    allowExecute = true;
                    c.RouteData.Values.Add("IsAllowed", allowExecute);
                    c.RouteData.Values.Add("Msg", "");
                }

                if (!allowExecute)
                {
                    if (String.IsNullOrEmpty(Message))
                        Message = "You may only perform this action every {n} seconds.";

                    c.RouteData.Values.Add("IsAllowed", allowExecute);
                    c.RouteData.Values.Add("Msg", Message.Replace("{n}", Seconds.ToString()));
                    //c.Result = new ContentResult { Content = Message.Replace("{n}", Seconds.ToString()) };
                    //// see 409 - http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
                    //c.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                }
            }
        }
    
}