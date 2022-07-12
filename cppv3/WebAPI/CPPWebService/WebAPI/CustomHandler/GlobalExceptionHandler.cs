using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace WebDemoAPI.CustomHandler
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var result = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Internal Server Error Occurred"),
                ReasonPhrase = "Exception"
            };

            context.Result = new ErrorMessageResult(context.Request, result);
        }

        public class ErrorMessageResult : IHttpActionResult
        {
            private HttpRequestMessage _request;
            private readonly HttpResponseMessage _httpResponseMessage;

            public ErrorMessageResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
            {
                _request = request;
                _httpResponseMessage = httpResponseMessage;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_httpResponseMessage);
            }
        }
    }
}