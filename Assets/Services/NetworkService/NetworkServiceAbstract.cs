using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Assets.Services.Json;

namespace Assets.Services.NetworkService
{
    /// <summary>
    /// this is an abstract implementation of network post and get
    /// </summary>
    public abstract class NetworkServiceAbstract
    {
        private readonly HttpClient client;

        private readonly IJsonService jsonService;

        protected NetworkServiceAbstract(IJsonService jsonService)
        {
            this.jsonService = jsonService;
        }


        ~NetworkServiceAbstract()
        {
            client.Dispose();
        }

        protected void CancelAll()
        {
            client.CancelPendingRequests();
        }

        protected Task<HttpResponseMessage> PostAsync<T>(Uri uri, T postData, CancellationToken token)
        {
            return client.PostAsync(uri, new StringContent(jsonService.Serialize(postData)), token);

        }
        protected Task<HttpResponseMessage> PostAsync<T>(Uri uri, T postData)
        {
            return PostAsync(uri, postData, CancellationToken.None);
        }

        protected Task<HttpResponseMessage> GetAsync(Uri uri, HttpCompletionOption options, CancellationToken token)
        {
            return client.GetAsync(uri, options, token);

        }

        protected Task<HttpResponseMessage> GetAsync(Uri uri, HttpCompletionOption options)
        {
            return GetAsync(uri, options, CancellationToken.None);
        }

        protected Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken token)
        {
            return GetAsync(uri, HttpCompletionOption.ResponseContentRead, token);
        }
        protected Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            return GetAsync(uri, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
        }

    }
}