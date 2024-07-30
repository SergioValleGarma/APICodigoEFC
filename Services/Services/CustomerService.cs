using Domain.Models;
using Infraestructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Services.Services
{
    public class CustomerService
    {
        private readonly CodigoContext _context;

        public CustomerService(CodigoContext context)
        {
            _context = context;
        }
        public List<Customer> GetByFilters(string? name, string? documentNumber)
        {
            IQueryable<Customer> query = _context.Customers.Where(x => x.IsActive);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.Contains(name));

            if (!string.IsNullOrEmpty(documentNumber))
                query = query.Where(x => x.DocumentNumber.Contains(documentNumber));

            return query.ToList();
        }

        public void Insert(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public int UpdateName( int id, string name)
        {
            int code = 0;

                Customer customer = _context.Customers.Find(id);
                if (customer == null)
                {
                    code = -1000;
                    ////response.Message = "El cliente no existe";
                return code;
                }
                customer.Name = name;
                _context.Entry(customer).State = EntityState.Modified;
                _context.SaveChanges();
            return code;


            

        }
        public void Delete(int id)
        {
            //Eliminación Física  
            //_context.Customers.Remove(customer);

            var customer = _context.Customers.Find(id);
            customer.IsActive = false;
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
