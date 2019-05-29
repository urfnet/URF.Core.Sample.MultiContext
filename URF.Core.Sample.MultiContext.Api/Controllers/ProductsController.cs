using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URF.Core.Sample.MultiContext.EF.Products;

namespace URF.Core.Sample.MultiContext.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsUnitOfWork _unitOfWork;

        public ProductsController(IProductsUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _unitOfWork.ProductsRepository.Query().SelectAsync();
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(id);
            if (product == null)
                return NotFound();
            return product;
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            _unitOfWork.ProductsRepository.Update(product);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(id))
                    return NotFound();
                else
                    throw;
            }
            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //_unitOfWork.Products.Add(product);
            _unitOfWork.ProductsRepository.Insert(product);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _unitOfWork.ProductsRepository.DeleteAsync(id);
            if (!result)
                return NotFound();
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _unitOfWork.ProductsRepository.ExistsAsync(id);
        }
    }
}
