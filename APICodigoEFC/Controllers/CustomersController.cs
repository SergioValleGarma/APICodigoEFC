using APICodigoEFC.Models;
using APICodigoEFC.Request;
using APICodigoEFC.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICodigoEFC.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CodigoContext _context;

        public CustomersController(CodigoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Customer> GetByFilters(string? name,string? documentNumber )
        {
            IQueryable<Customer> query = _context.Customers.Where(x=>x.IsActive);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.Contains(name));

            if (!string.IsNullOrEmpty(documentNumber))
                query = query.Where(x => x.DocumentNumber.Contains(documentNumber));

            return query.ToList();
        }

        [HttpPost]
        public void Insert([FromBody] Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
        [HttpPut]
        public void Update([FromBody] Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
        }
        [HttpPut]
        public ResponseBase UpdateName([FromBody] CustomerUpdateRequest request)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                    var customer = _context.Customers.Find(request.Id);
                if (customer == null)
                {
                    response.Code = -1000;
                    response.Message = "El cliente no existe";
                }
                customer.Name = request.Name;
                _context.Entry(customer).State = EntityState.Modified;
                _context.SaveChanges();

                response.Code = 0000;
                response.Message = "Actualización Correcta";
                return response;
            }
            catch (Exception ex)
            {

                response.Message = "Error No controlado";
                response.Code = -1000;
                return response;
            }

            
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //Eliminación Física  
            //_context.Customers.Remove(customer);

            var customer =  _context.Customers.Find(id);
            customer.IsActive = false;
            _context.Entry(customer).State = EntityState.Modified;         
            _context.SaveChanges();          
        }



    }
}
