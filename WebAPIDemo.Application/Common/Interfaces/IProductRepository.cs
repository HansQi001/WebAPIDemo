using WebAPIDemo.Application.Products.DTOs;
using WebAPIDemo.Domain.Entities;

namespace WebAPIDemo.Application.Common.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IAsyncEnumerable<ProductSummaryDTO> GetAllAsync();
    }

}
