using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Tuya.PruebaTecnica.Models.Models;
using Tuya.PruebaTecnica.ProductsService.Data;
using Tuya.PruebaTecnica.ProductsService.Repositories;

namespace Tuya.PruebaTecnica.ProductsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "apiv1")]
    [EnableCors]
    [SwaggerTag("Productos")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Obtener lista de productos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<Product>))]
        public async Task<ActionResult> Get([FromQuery] int[] ids = null)
        {
            return Ok(await _productRepository.GetAllAsync(ids));
        }

        /// <summary>
        /// Obtener un producto por id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Product))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _productRepository.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        /// <summary>
        /// Edita un producto
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="model">modelo</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(Dictionary<string, string[]>))]
        public async Task<IActionResult> Put(int id, Product model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.UpdateAsync(model);
                    return Ok();
                }
                catch (Exception)
                {
                    if (!ProductExists(model.Id))
                    {
                        return NotFound();
                    }
                    ModelState.AddModelError("", "No se pudo editar la entidad");
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Agrega un producto
        /// </summary>
        /// <param name="brand">marca</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(Dictionary<string, string[]>))]
        public async Task<ActionResult> Post(Product model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.AddAsync(model);
                    return Ok();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "No se pudo crear la entidad");
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Elimina un producto
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(Dictionary<string, string[]>))]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _productRepository
                .GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            try
            {
                await _productRepository.RemoveAsync(model);
                return Ok();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se pudo eliminar la entidad");
            }

            return BadRequest(ModelState);
        }

        #region Helpers
        private bool ProductExists(int id)
        {
            return _productRepository.AnyByIdAsync(id);
        }
        #endregion
    }
}
