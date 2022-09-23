using Fruitkha.Core.Dtos.Catalog;

namespace Fruitkha.Core.Interfaces;

public interface ICatalogService
{
    public Task<IEnumerable<CategoryDto>> GetCategoriesList();
}