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
    /// Generic template for a DelegatingHandler
    /// </summary>
    /// <remarks>
    /// If you don't have any response processing to do for step 3, you can replace the entire
    /// block of steps 2 through 4 with a single <code>return base.SendAsync(request, cancellationToken);</code>
    /// and remove the async keyword from the method definition (since you don't need the 
    /// continuation behavior of await in that case).
    /// </remarks>
    /// 
    public class ApiKeyHeaderHandler : DelegatingHandler
    {
        // Name of our custom handler to look for
        public const string _apiKeyHeader = "X-API_Key";

        // Name of api query string ey
        public const string _apiQueryString = "api_key";
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // STEP 1: Global message-level logic that must be executed BEFORE the request
            //         is sent on to the action method
            string apiKey = null;

            if (request.Headers.Contains(_apiKeyHeader))
            {
                apiKey = request.Headers.GetValues(_apiKeyHeader).FirstOrDefault();
            }
            else
            {
                // Check if the key is in query string instead
                var queryString = request.GetQueryNameValuePairs();
                var kvp = queryString.FirstOrDefault(a => a.Key.ToLowerInvariant().Equals(_apiQueryString));
                if (!String.IsNullOrEmpty(kvp.Value))
                {
                    apiKey = kvp.Value;
                }
            }

            /*
             * With respect to following single responsibility principle, we will comment the authorization
             * code below, because it should be handled in the authorization or authentication layer
             * 
             * Our class should only find the key and if it is present, should save it in properties
             */
            // If any key was not present then, abort request, abort pipeline completely
            //if (string.IsNullOrEmpty(apiKey))
            //{
            //    // create the response
            //    var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
            //    {
            //        Content = new StringContent("Missing Api Key")
            //    };
            //    return await Task.FromResult(response);
            //}

            // If the key was present then, save the value to properties
            request.Properties.Add(_apiKeyHeader, apiKey);

            // Compress step 2,3 and 4 into one line since we don't need any post-request processing 
            return await base.SendAsync(request, cancellationToken);
        }
    }

    /// <summary>
    /// http request extension for retrieving api key if present
    /// We are defining it here to hide the details of api key name from our action methods
    /// and to abstract the value of api key by calling this method
    /// </summary>
    public static class HttpRequestMessageApiKeyExtension
    {
        /// <summary>
        /// Retrieves the Api key present in the request, or null if none found. 
        public static string GetApiKey(this HttpRequestMessage request)
        {
            if (request == null)
                return null;
            if(request.Properties.TryGetValue(ApiKeyHeaderHandler._apiKeyHeader, out object apiKey))
            {
                return (string)apiKey;
            }

            return null;
        }
    }
}