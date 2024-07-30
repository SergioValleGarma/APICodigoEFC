using Domain.Models;
using APICodigoEFC.Request;
using Infraestructure.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Services;

namespace APICodigoEFC.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CodigoContext _context;
        private ProductService _service;

        public ProductsController(CodigoContext context)
        {
            _context = context;
            _service= new ProductService(_context);
        }

        [HttpGet]
        ////[Authorize]
        public List<Product> GetByFilters(string? name)
        {
            var products = _service.GetByFilters(name);
            return products;
            //IQueryable<Product> query = _context.Products.Where(x => x.IsActive);

            //if (!string.IsNullOrEmpty(name))
            //    query = query.Where(x => x.Name.Contains(name));

            ///////

            //return query.OrderBy(x=>x.Price) .ToList();

        }

        [HttpPost]
        public void Insert([FromBody] ProductInsertRequest request)
        {
            //Convertir el request => Model (Serializar)

            Product product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            _service.Insert(product);//Un Modelo

        }
        [HttpPut]
        public void Update([FromBody] Product product)
        {
            _service.Update(product);
        }

        [HttpPut]
        public void UpdatePrice([FromBody] ProductUpdateRequest request)
        {

            _service.UpdatePrice(request.Id, request.Price);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }
    }
}
