using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tuya.PruebaTecnica.Models.Models;
using Tuya.PruebaTecnica.ProductsService.Controllers;
using Tuya.PruebaTecnica.ProductsService.Repositories;
using Xunit;

namespace Tuya.PruebaTecnica.ProductsService.Test.Controllers
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductRepository> _productsService;
        private readonly ProductsController _productsController;

        public ProductsControllerTest()
        {
            _productsService = new Mock<IProductRepository>();
            _productsController = new ProductsController(_productsService.Object);
        }

        [Fact]
        public async Task Get_Test()
        {
            var list = new List<Product>() { new Product() { Id = 1, Name = "Nombre de producto", Price = 15000 } };

            _productsService.Setup(p => p.GetAllAsync(It.IsAny<int[]>()))
                .ReturnsAsync(list);

            var result = await _productsController.Get(It.IsAny<int[]>());
            var obj = ((OkObjectResult)result).Value as List<Product>;

            Assert.True(list.First().Id == obj.First().Id, "No retorna la lista correctamente");
            _productsService.Verify(p => p.GetAllAsync(It.IsAny<int[]>()));
        }

        [Fact]
        public async Task GetById_Succeded_Test()
        {
            var model = new Product() { Id = 1, Name = "Nombre de producto", Price = 15000 };

            _productsService.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(model);

            var result = await _productsController.GetById(It.IsAny<int>());
            var obj = ((OkObjectResult)result).Value as Product;

            Assert.True(model.Id == obj.Id, "No retorna el modelo correctamente");
            _productsService.Verify(p => p.GetByIdAsync(It.IsAny<int>()));
        }

        [Fact]
        public async Task GetById_NotFound_Test()
        {
            var model = new Product() { Id = 1, Name = "Nombre de producto", Price = 15000 };

            _productsService.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Product) null);

            var result = (NotFoundResult) await _productsController.GetById(It.IsAny<int>());

            Assert.True(result != null, "No retorna el resultado esperado");
            _productsService.Verify(p => p.GetByIdAsync(It.IsAny<int>()));
        }

        [Fact]
        public async Task Put_Test()
        {
            var model = new Product() { Id = 1, Name = "Nombre de producto", Price = 15000 };

            _productsService.Setup(p => p.UpdateAsync(model));

            var result = (OkResult)await _productsController.Put(1, model);

            Assert.True(result != null, "No retorna el resultado esperado");
            _productsService.Verify(p => p.UpdateAsync(model));
        }

        [Fact]
        public async Task Post_Test()
        {
            var model = new Product() { Id = 1, Name = "Nombre de producto", Price = 15000 };

            _productsService.Setup(p => p.AddAsync(model));

            var result = (OkResult)await _productsController.Post(model);

            Assert.True(result != null, "No retorna el resultado esperado");
            _productsService.Verify(p => p.AddAsync(model));
        }

        [Fact]
        public async Task Delete_Test()
        {
            var model = new Product() { Id = 1, Name = "Nombre de producto", Price = 15000 };

            _productsService.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(model);
            _productsService.Setup(p => p.RemoveAsync(model));

            var result = (OkResult)await _productsController.Delete(1);

            Assert.True(result != null, "No retorna el resultado esperado");
            _productsService.Verify(p => p.RemoveAsync(model));
        }

        [Fact]
        public async Task Delete_NotFound_Test()
        {
            var model = new Product() { Id = 1, Name = "Nombre de producto", Price = 15000 };

            _productsService.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Product)null);

            var result = (NotFoundResult)await _productsController.Delete(It.IsAny<int>());

            Assert.True(result != null, "No retorna el resultado esperado");
        }
    }
}