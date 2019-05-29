using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URF.Core.Sample.MultiContext.EF.Customers;

namespace URF.Core.Sample.MultiContext.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersUnitOfWork _unitOfWork;

        public CustomersController(ICustomersUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _unitOfWork.CustomersRepository.Query().SelectAsync();
            return Ok(customers);
        }
        
        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _unitOfWork.CustomersRepository.FindAsync(id);
            if (customer == null)
                return NotFound();
            return customer;
        }
        
        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
                return BadRequest();
            _unitOfWork.CustomersRepository.Update(customer);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExists(id))
                    return NotFound();
                else
                    throw;
            }
            return Ok(customer);
        }
        
        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _unitOfWork.CustomersRepository.Insert(customer);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var result = await _unitOfWork.CustomersRepository.DeleteAsync(id);
            if (!result)
                return NotFound();
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> CustomerExists(int id)
        {
            return await _unitOfWork.CustomersRepository.ExistsAsync(id);
        }
    }
}
