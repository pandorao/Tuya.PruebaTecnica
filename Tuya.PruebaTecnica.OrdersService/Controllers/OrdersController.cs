using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Tuya.PruebaTecnica.Models.Dtos.Order;
using Tuya.PruebaTecnica.Models.Models;
using Tuya.PruebaTecnica.OrderService.Data;
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
        private readonly ApplicationDbContext _dbcontext;
        private readonly IProductServices _productService;

        public OrdersController(ApplicationDbContext dbcontext, 
            IProductServices productService)
        {
            _dbcontext = dbcontext;
            _productService = productService;
        }

        /// <summary>
        /// Obtener lista de ordenes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<Order>))]
        public async Task<ActionResult> Get()
        {
            return Ok(await _dbcontext.Orders
                .Include(p => p.OrderProducts)
                .AsNoTracking()
                .ToListAsync());
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
            var qry = _dbcontext.Orders
                .Include(p => p.OrderProducts)
                .AsNoTracking();

            var model = await qry.FirstOrDefaultAsync(p => p.Id == id);
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
                    _dbcontext.Orders.Add(order);
                    await _dbcontext.SaveChangesAsync();
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
