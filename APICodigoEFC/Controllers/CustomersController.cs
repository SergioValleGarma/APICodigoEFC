using Domain.Models;

using APICodigoEFC.Response;
using Infraestructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Services;
using APICodigoEFC.Request;

namespace APICodigoEFC.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CodigoContext _context;
        private CustomerService _service;

        public CustomersController(CodigoContext context)
        {
            _context = context;
            _service = new CustomerService(_context);
        }

        [HttpGet]
        public List<Customer> GetByFilters(string? name,string? documentNumber )
        {
            var customers = _service.GetByFilters(name, documentNumber);
            return customers;
        }

        [HttpPost]
        public void Insert([FromBody] Customer customer)
        {
            _service.Insert(customer);
        }
        [HttpPut]
        public void Update([FromBody] Customer customer)
        {
            _service?.Update(customer);
        }
        [HttpPut]
        public ResponseBase UpdateName([FromBody] CustomerUpdateRequest request)
        {
            ResponseBase response = new ResponseBase();
            int code = 0;
            try
            {
                response.Code = 0;
                response.Message = "Registro exitoso";

                code = _service.UpdateName(request.Id, request.Name);

                if (code != 0)
                {
                    response.Message = "Error controlado";
                    response.Code = code;
                }

                    
                return response;
            }
            catch (Exception ex)
            {

                response.Message = "Error No controlado :" + ex;
                response.Code = -1001;
                return response;
            }


        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service?.Delete(id);
        }



    }
}
