using Gluttony.Abstracts;
using Gluttony.DataTransfer;
using Gluttony.DataTransfer.RestfulRequests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Gluttony
{
    public class RestfulClient : IRequestClient
    {
        public string BaseAddress { get; init; }

        private RestfulClient(string baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public static RestfulClient Create(string url)
        {
            RestfulClient client = new RestfulClient(url);

            return client;
        }

        public async Task<T> SendAsync<T>(IRequest<T> request) where T : Response, new()
        {
            T response = new();

            if(request is IRestfulRequest<T> restfulRequest)
            {
                using HttpClient client = CreateClient(restfulRequest);
                HttpResponseMessage responseMessage = null;

                if (restfulRequest is GetRequest<T> getRequest)
                    responseMessage = await GetAsync(client, getRequest);

                if (restfulRequest is PostRequest<T> postRequest)
                    responseMessage = await PostAsync(client, postRequest);

                if (restfulRequest is PutRequest<T> putRequest)
                    responseMessage = await PutAsync(client, putRequest);

                if (restfulRequest is DeleteRequest<T> deleteRequest)
                    responseMessage = await DeleteAsync(client, deleteRequest);

                if (restfulRequest is PatchRequest<T> patchRequest)
                    responseMessage = await PatchAsync(client, patchRequest);

                if (responseMessage == null)
                {
                    response.ErrorCount++;
                    response.AddMessage("An acceptable verb was not found.");
                    return response;
                }

                return await ResponseMessageBreakDownAsync<T>(restfulRequest, responseMessage);
            }

            response.ErrorCount++;
            response.AddMessage("IRequest not valid. Must use \"IRestfulRequest\" type.");
            return response;
        }

        private HttpClient CreateClient<T>(IRestfulRequest<T> request) where T : Response
        {
            HttpClient client = new();
            client.BaseAddress = new Uri(BaseAddress);
            client.DefaultRequestHeaders.Clear();

            if (string.IsNullOrWhiteSpace(request.Token))
                return client;

            client.DefaultRequestHeaders.Authorization = AuthenticationBreakDown(request);

            return client;
        }

        private async Task<HttpResponseMessage> GetAsync<T>(HttpClient client, GetRequest<T> request) where T : Response
        {
            string endpoint = request.Command;

            int i = 0;
            foreach(KeyValuePair<string,string> kv in request.Parameters)
            {
                endpoint += i == 0 ? "?" : "&";
                endpoint += $"{kv.Key}={kv.Value}";

                i++;
            }

            HttpResponseMessage response = await client.GetAsync(endpoint);
            return response;
        }

        private async Task<HttpResponseMessage> PostAsync<T>(HttpClient client, PostRequest<T> request) where T : Response
        {
            HttpContent content = null;
            if(request.Parameters.Count > 0)
            {
                content = new StringContent(JsonConvert.SerializeObject(request.Parameters));
                content.Headers.Clear();
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            HttpResponseMessage response = await client.PostAsync(request.Command, content);
            return response;
        }

        private async Task<HttpResponseMessage> PutAsync<T>(HttpClient client, PutRequest<T> request) where T : Response
        {
            HttpContent content = null;
            if (request.Parameters.Count > 0)
            {
                content = new StringContent(JsonConvert.SerializeObject(request.Parameters));
                content.Headers.Clear();
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            HttpResponseMessage response = await client.PutAsync(request.Command, content);
            return response;
        }

        private async Task<HttpResponseMessage> DeleteAsync<T>(HttpClient client, DeleteRequest<T> request) where T: Response
        {
            throw new NotImplementedException();
        }

        private async Task<HttpResponseMessage> PatchAsync<T>(HttpClient client, PatchRequest<T> request) where T:Response
        {
            throw new NotImplementedException();
        }

        private async Task<T> ResponseMessageBreakDownAsync<T>(IRestfulRequest<T> request, HttpResponseMessage responseMessage) where T : Response, new()
        {
            T response = new();

            if(responseMessage.IsSuccessStatusCode)
            {
                return await request.SuccessCallBack.Invoke(responseMessage);
            }

            if (request.ErrorCallBack != null)
                response = await request.ErrorCallBack.Invoke(responseMessage);

            return response;
        }

        private AuthenticationHeaderValue AuthenticationBreakDown<T>(IRestfulRequest<T> request) where T : Response
        {
            return request.AuthenticationType switch
            {
                AuthenticationType.Bearer => new AuthenticationHeaderValue("bearer", request.Token),
                _ => null
            };
        }
    }
}
