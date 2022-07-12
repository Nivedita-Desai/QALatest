using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebAPI.Models.Api;

namespace WebAPI.CustomHandler
{
    public class RequestResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var requestedMethod = request.Method;
            var userHostAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "0.0.0.0";
            var useragent = request.Headers.UserAgent.ToString();
            var requestMessage = request.Content.ReadAsByteArrayAsync();
            var uriAccessed = request.RequestUri.AbsoluteUri;

            var responseHeadersString = new StringBuilder();
            foreach (var header in request.Headers)
            {
                responseHeadersString.Append($"{header.Key}: {String.Join(", ", header.Value)}{Environment.NewLine}");
            }

            var messageLoggingHandler = new MessageLogging();
            var message = await requestMessage;
            var requestLog = new ApiLog()
            {
                Headers = responseHeadersString.ToString(),
                AbsoluteUri = uriAccessed,
                Host = userHostAddress,
                RequestBody =  Encoding.UTF8.GetString(message),
                UserHostAddress = userHostAddress,
                UserAgent = useragent,
                RequestedMethod = requestedMethod.ToString(),
                StatusCode = string.Empty
            };

            messageLoggingHandler.IncomingMessageAsync(requestLog);

            var response = await base.SendAsync(request, cancellationToken);
            if (response.Content == null )
                return response;
            byte[] responseMessage;
            if (response.IsSuccessStatusCode)
                responseMessage = await response.Content.ReadAsByteArrayAsync();
            else
                responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);

            var responseLog = new ApiLog()
            {
                Headers = responseHeadersString.ToString(),
                AbsoluteUri = uriAccessed,
                Host = userHostAddress,
                RequestBody = Encoding.UTF8.GetString(responseMessage),
                UserHostAddress = userHostAddress,
                UserAgent = useragent,
                RequestedMethod = requestedMethod.ToString(),
                StatusCode = string.Empty
            };

            messageLoggingHandler.OutgoingMessageAsync(responseLog);
            return response;
        }

        public async Task<string> GetString(String text)
        {
            //System.Threading.Thread.Sleep(5000);
            return await Task.FromResult("Hello");
        }
    }
}