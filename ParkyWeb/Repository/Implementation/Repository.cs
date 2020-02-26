using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;

        // Here we are going to make Http calls, so we need to inject the IClientFactory.
        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory; 
        }
        public async Task<bool> CreateAsync(string url, T objectToCreate)
        {
            // In order to create a record a request to the api needs to be made first, then wait for a response 
            // if it was successful or not.
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if(objectToCreate != null)
            {
                request.Content = new StringContent(JsonConvert
                    .SerializeObject(objectToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if(response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string url, int Id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+Id);
            HttpResponseMessage response = await _clientFactory.CreateClient().SendAsync(request);
            if(response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage response = await _clientFactory.CreateClient().SendAsync(request);
            return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<IEnumerable<T>>
                (await response.Content.ReadAsStringAsync()): null;
        }

        public async Task<T> GetAsync(string url, int Id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + Id);
            HttpResponseMessage response = await _clientFactory.CreateClient().SendAsync(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var objectToGet = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(objectToGet);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(string url, T objectToUpdate)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if(objectToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objectToUpdate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if(response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
