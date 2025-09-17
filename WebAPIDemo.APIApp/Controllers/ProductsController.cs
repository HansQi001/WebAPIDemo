using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebAPIDemo.Application.Common.Interfaces;
using WebAPIDemo.Application.Products.DTOs;
using WebAPIDemo.Domain.Entities;

namespace WebAPIDemo.APIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public ProductsController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task GetAll()
        {
            var response = HttpContext.Response;
            response.ContentType = "application/x-ndjson";

            await foreach (var product in _productRepo.GetAllAsync())
            {
                var json = JsonSerializer.Serialize(product);
                await response.WriteAsync(json + "\n");
                await response.Body.FlushAsync();
            }
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