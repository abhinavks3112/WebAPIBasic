using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Web;

namespace WebAPIBasic.Handlers
{
    /// <summary>
    /// Removing bad headers from response so as to prevent giving extra information
    /// to attackers
    /// 
    /// Note: This doesn't work, this is just an example to show the limitation of 
    /// delegating handler. Some of the settings work in much broader context of IIS or ASP.NET
    /// and are so controlled by it, so we can't change them from here. Some of them can be
    /// changed in web.config(eg. some headers), or in IIS itself.
    /// </summary>
    public class RemoveBadHeadersHandler : DelegatingHandler
    {
        // Name of our custom handler to look for
        readonly string[] _badHeaders = { "X-Powered-By", "X-AspNet-Version", "Server" };
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // STEP 2: Call the rest of the pipeline, all the way to a response message
            var response = await base.SendAsync(request, cancellationToken);

            // STEP 3: Any global message-level logic that must be executed AFTER the request
            //          has executed, before the final HTTP response message
            foreach (var h in _badHeaders)
            {
                response.Headers.Remove(h);
            }

            // STEP 4:  Return the final HTTP response
            return response;
        }
    }
}