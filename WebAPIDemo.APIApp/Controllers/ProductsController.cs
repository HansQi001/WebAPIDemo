using Microsoft.AspNetCore.Mvc;
using WebAPIDemo.Application.Products.DTOs;
using WebAPIDemo.Domain.Entities;
using WebAPIDemo.Domain.Interfaces;

namespace WebAPIDemo.APIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public ProductsController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public IAsyncEnumerable<ProductSummaryDTO> GetAll()
        {
            return _productRepo.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            // ModelState is automatically checked when [ApiController] is present
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };

            await _productRepo.AddAsync(product);
            await _productRepo.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { id = product.Id }, product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSummaryDTO>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            await _productRepo.DeleteByIdAsync(id);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            var existing = await _productRepo.GetByIdAsync(request.Id);
            if (existing == null)
                return NotFound();

            existing.Name = request.Name;
            existing.Price = request.Price;
            existing.Stock = request.Stock;

            _productRepo.Update(existing);
            await _productRepo.SaveChangesAsync();

            return Ok(existing);
        }
    }
}