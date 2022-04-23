
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tuya.PruebaTecnica.Models.Models;
using Tuya.PruebaTecnica.SDK.Models;

namespace Tuya.PruebaTecnica.SDK.Services
{
    public interface IDeliveriesServices
    {
        /// <summary>
        /// Obtener lista
        /// </summary>
        /// <returns></returns>
        Task<List<Delivery>> GetAllAsync();

        /// <summary>
        /// Obtener uno por id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<Delivery> GetByIdAsync(int id);

        /// <summary>
        /// Agrega
        /// </summary>
        /// <param name="model">modelo</param>
        /// <returns></returns>
        Task<ServiceResult<Delivery>> AddAsync(Delivery model);
    }

    public class DeliveriesServices : IDeliveriesServices
    {
        private readonly HttpClient _client;

        public DeliveriesServices(IOptions<ServicesEndPointOptions> auth)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"{auth.Value.DeliveryServiceEndPoint}/api/"),
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Obtener lista
        /// </summary>
        /// <returns></returns>
        public async Task<List<Delivery>> GetAllAsync()
        {
            List<Delivery> list = new List<Delivery>();
            HttpResponseMessage response = await _client.GetAsync($"Deliveries");
            if (response.IsSuccessStatusCode)
            {
                list = await response.Content.ReadAsAsync<List<Delivery>>();
            }
            return list;
        }

        /// <summary>
        /// Obtener uno por id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<Delivery> GetByIdAsync(int id)
        {
            Delivery model = null;
            HttpResponseMessage response = await _client.GetAsync($"Deliveries/{id}");
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsAsync<Delivery>();
            }
            return model;
        }

        /// <summary>
        /// Agrega
        /// </summary>
        /// <param name="model">modelo</param>
        /// <returns></returns>
        public async Task<ServiceResult<Delivery>> AddAsync(Delivery model)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("Deliveries", model);

            var result = new ServiceResult<Delivery>();
            result.StatusCode = response.StatusCode;
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                result.Errors = await response.Content.ReadAsAsync<Dictionary<string, string[]>>();
            }else if (response.IsSuccessStatusCode)
            {
                result.ResponseObject = await response.Content.ReadAsAsync<Delivery>();
            }
            return result;
        }
    }
}
