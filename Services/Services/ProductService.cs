
using Domain.Models;
using Infraestructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProductService
    {
        private readonly CodigoContext _context;

        public ProductService(CodigoContext context)
        {
            _context = context;
        }

        public List<Product> GetByFilters(string? name)
        {
            IQueryable<Product> query = _context.Products.Where(x => x.IsActive);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.Contains(name));

            /////

            return query.OrderBy(x => x.Price).ToList();

        }

        public void Insert(Product product)
        {

            _context.Products.Add(product);//Un Modelo
            _context.SaveChanges();
        }

        public void Update( Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void UpdatePrice(int id, double price)
        {

            var product = _context.Products.Find(id);
            product.Price = price;
            _context.Entry(product).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var Product = _context.Products.Find(id);
            Product.IsActive = false;
            _context.Entry(Product).State = EntityState.Modified;
            _context.SaveChanges();
        }

    }
}
