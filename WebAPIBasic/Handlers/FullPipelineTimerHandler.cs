using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebAPIBasic.Handlers
{
    /// <summary>
    /// Generic template for a DelegatingHandler
    /// </summary>
    /// <remarks>
    /// If you don't have any response processing to do for step 3, you can replace the entire
    /// block of steps 2 through 4 with a single <code>return base.SendAsync(request, cancellationToken);</code>
    /// and remove the async keyword from the method definition (since you don't need the 
    /// continuation behavior of await in that case).
    /// </remarks>
    /// 
    public class FullPipelineTimerHandler : DelegatingHandler
    {
        // Header name
        const string _header = "X-API_Timer";
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // STEP 1: Global message-level logic that must be executed BEFORE the request
            //         is sent on to the action method
            var timer = Stopwatch.StartNew();

            // STEP 2: Call the rest of the pipeline, all the way to a response message
            var response = await base.SendAsync(request, cancellationToken);

            // STEP 3: Any global message-level logic that must be executed AFTER the request
            //          has executed, before the final HTTP response message
            var elapsed = timer.ElapsedMilliseconds;

            // Log the elapsed time
            Trace.WriteLine(" --Pipeline+Action time msec: " + elapsed);

            // Add the elapsed new in the reponse message under a new header name
            response.Headers.Add(_header, elapsed + " msec");

            // STEP 4:  Return the final HTTP response
            return response;
        }
    }
}
