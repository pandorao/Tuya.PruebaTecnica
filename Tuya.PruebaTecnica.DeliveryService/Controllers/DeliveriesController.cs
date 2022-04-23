using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Tuya.PruebaTecnica.Models.Models;
using Tuya.PruebaTecnica.DeliveryService.Data;

namespace Tuya.PruebaTecnica.DeliveryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "apiv1")]
    [EnableCors]
    [SwaggerTag("Deliveries")]
    public class DeliveriesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;

        public DeliveriesController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        /// <summary>
        /// Obtener lista de entregas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<Product>))]
        public async Task<ActionResult> Get()
        {
            return Ok(await _dbcontext.Deliveries.AsNoTracking().ToListAsync());
        }

        /// <summary>
        /// Obtener una entrega por id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Product))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var qry = _dbcontext.Deliveries.AsNoTracking();

            var model = await qry.FirstOrDefaultAsync(p => p.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        /// <summary>
        /// Agrega una entrega
        /// </summary>
        /// <param name="model">delivery</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(Dictionary<string, string[]>))]
        public async Task<ActionResult> Post(Delivery model)
        {
            if (ModelState.IsValid)
            {
                _dbcontext.Deliveries.Add(model);
                try
                {
                    await _dbcontext.SaveChangesAsync();
                    return Ok(model);
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
