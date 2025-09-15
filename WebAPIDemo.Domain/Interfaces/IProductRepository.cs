using WebAPIDemo.Application.Products.DTOs;
using WebAPIDemo.Domain.Entities;

namespace WebAPIDemo.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IAsyncEnumerable<ProductSummaryDTO> GetAllAsync();
    }

}
