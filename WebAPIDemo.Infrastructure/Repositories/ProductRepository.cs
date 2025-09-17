using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Application.Common.Interfaces;
using WebAPIDemo.Application.Products.DTOs;
using WebAPIDemo.Domain.Entities;
using WebAPIDemo.Infrastructure.Data;

namespace WebAPIDemo.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public IAsyncEnumerable<ProductSummaryDTO> GetAllAsync()
                        => _context.Products
                                .Select(p => new ProductSummaryDTO(p.Id, p.Name))
                                .AsNoTracking()
                                .AsAsyncEnumerable();
    }
}
