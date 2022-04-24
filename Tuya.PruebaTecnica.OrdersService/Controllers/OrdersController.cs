using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Tuya.PruebaTecnica.Models.Dtos.Order;
using Tuya.PruebaTecnica.Models.Models;
using Tuya.PruebaTecnica.OrderService.Data;
using Tuya.PruebaTecnica.OrderService.Repositories;
using Tuya.PruebaTecnica.SDK.Services;

namespace Tuya.PruebaTecnica.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "apiv1")]
    [EnableCors]
    [SwaggerTag("Ordenes")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductServices _productService;
        private readonly IDeliveriesServices _deliveriesServices;

        public OrdersController(IOrderRepository orderRepository, 
            IProductServices productService,
            IDeliveriesServices deliveriesServices)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _deliveriesServices = deliveriesServices;
        }

        /// <summary>
        /// Obtener lista de ordenes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<Order>))]
        public async Task<ActionResult> Get()
        {
            return Ok(await _orderRepository.GetAllAsync());
        }

        /// <summary>
        /// Obtener una orden por id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Order))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _orderRepository.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        /// <summary>
        /// Crear una orden
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Order))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(Dictionary<string, string[]>))]
        public async Task<ActionResult> Post(OrderRequestDto request)
        {
            var products = await _productService
                .GetAllAsync(request.ProductIds);
            if (!products.Any())
            {
                ModelState.AddModelError("", "No se encontraron los productos seleccionados"); 
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var order = new Order()
                    {
                        CreationDate = DateTime.Now,
                        Total = products.Sum(p => p.Price),
                        OrderProducts = products.Select(p => new OrderProduct() { ProductId = p.Id }).ToList()
                    };
                    await _orderRepository.AddAsync(order);


                    var deliveryResponse = await _deliveriesServices.AddAsync(new Delivery() 
                    {
                        ExtimatedShipDate = DateTime.Now.AddDays(2),
                        OrderId = order.Id
                    });

                    if (deliveryResponse.Succeeded())
                    {
                        order.DeliveryId = deliveryResponse.ResponseObject.Id;
                        await _orderRepository.UpdateAsync(order);
                    }

                    return await GetById(order.Id);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "No se pudo crear la entidad");
                }
            }

            return BadRequest(ModelState);
        }
    }
}
