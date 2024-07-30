using Domain.Models;
using APICodigoEFC.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Contexts;
using Services.Services;

namespace APICodigoEFC.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DetailsController : ControllerBase
    {
        private readonly CodigoContext _context;
        private DetailService _service;

        public DetailsController(CodigoContext context)
        {
            _context = context;
            _service = new DetailService(_context);
        }

        [HttpPost]
        public void Insert([FromBody] Detail detail)
        {
            //_context.Details.Add(detail);
            //_context.SaveChanges();
            _service.Insert(detail);
        }

        [HttpGet]
        public List<Detail> Get()
        {
            //IQueryable<Detail> query = _context.Details
            //    .Include(x => x.Product)
            //    .Include(x => x.Invoice).ThenInclude(y => y.Customer)
            //    .Where(x => x.IsActive);


            //return query.ToList();
            var details = _service.Get();
            return details;
        }
        //Listar todos los detalles y buscar por nombre de cliente.
        [HttpGet]
        public List<Detail> GetByFilters(string? customerName, string? invoiceNumber)
        {
            //IQueryable<Detail> query = _context.Details
            //   .Include(x => x.Product)
            //   .Include(x => x.Invoice).ThenInclude(y => y.Customer)
            //   .Where(x => x.IsActive);

            //if (!string.IsNullOrEmpty(customerName))
            //    query = query.Where(x => x.Invoice.Customer.Name.Contains(customerName));
            //if (!string.IsNullOrEmpty(invoiceNumber))
            //    query = query.Where(x => x.Invoice.Number.Contains(invoiceNumber));


            //return query.ToList();
            var details = _service.GetByFilters(customerName, invoiceNumber);
            return details;
        }

        [HttpGet]
        public List<DetailResponseV1> GetByInvoiceNumber(string? invoiceNumber)
        {

            IQueryable<Detail> query = _context.Details
                .Include(x => x.Product)
                .Include(x => x.Invoice)
                .Where(x => x.IsActive);
            if (!string.IsNullOrEmpty(invoiceNumber))
                query = query.Where(x => x.Invoice.Number.Contains(invoiceNumber));

            //Todos los detalles del modelo
            var details = query.ToList();


            //Convertir modelo al response
            var response = details
                           .Select(x => new DetailResponseV1
                           {
                               InvoiceNumber = x.Invoice.Number,
                               ProductName = x.Product.Name,
                               SubTotal = x.SubTotal
                           }).ToList();

            return response;
        }
    }
}
