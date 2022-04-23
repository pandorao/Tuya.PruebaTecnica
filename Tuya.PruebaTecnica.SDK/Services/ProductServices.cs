
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
    public interface IProductServices 
    {
        /// <summary>
        /// Obtener lista
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetAllAsync();

        /// <summary>
        /// Obtener uno por id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<Product> GetByIdAsync(int id);

        /// <summary>
        /// Editar
        /// </summary>
        /// <param name="model">modelo</param>
        /// <returns></returns>
        Task<ServiceResult> UpdateAsync(Product model);

        /// <summary>
        /// Agrega
        /// </summary>
        /// <param name="model">modelo</param>
        /// <returns></returns>
        Task<ServiceResult> AddAsync(Product model);

        /// <summary>
        /// Eliminar
        /// </summary>
        /// <param name="model">modelo</param>
        /// <returns></returns>
        Task<ServiceResult> RemoveAsync(Product model);
    }

    public class ProductServices
    {
        private readonly HttpClient _client;

        public ProductServices(IOptions<ServicesEndPointOptions> auth)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"{auth.Value.ProductServiceEndPoint}/api"),
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Obtener lista
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> GetAllAsync()
        {
            List<Product> list = new List<Product>();
            HttpResponseMessage response = await _client.GetAsync($"Products/");
            if (response.IsSuccessStatusCode)
            {
                list = await response.Content.ReadAsAsync<List<Product>>();
            }
            return list;
        }

        /// <summary>
        /// Obtener uno por id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<Product> GetByIdAsync(int id)
        {
            Product model = null;
            HttpResponseMessage response = await _client.GetAsync($"Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsAsync<Product>();
            }
            return model;
        }

        /// <summary>
        /// Editar
        /// </summary>
        /// <param name="model">modelo</param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateAsync(Product model)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"Products/{model.Id}", model);

            var result = new ServiceResult();
            result.StatusCode = response.StatusCode;
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                result.Errors = await response.Content.ReadAsAsync<Dictionary<string, string[]>>();
            }
            return result;
        }

        /// <summary>
        /// Agrega
        /// </summary>
        /// <param name="model">modelo</param>
        /// <returns></returns>
        public async Task<ServiceResult> AddAsync(Product model)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("Products", model);

            var result = new ServiceResult();
            result.StatusCode = response.StatusCode;
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                result.Errors = await response.Content.ReadAsAsync<Dictionary<string, string[]>>();
            }
            return result;
        }

        /// <summary>
        /// Eliminar
        /// </summary>
        /// <param name="model">modelo</param>
        /// <returns></returns>
        public async Task<ServiceResult> RemoveAsync(Product model)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"Products/{model.Id}");

            var result = new ServiceResult();
            result.StatusCode = response.StatusCode;
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                result.Errors = await response.Content.ReadAsAsync<Dictionary<string, string[]>>();
            }
            return result;
        }
    }
}
