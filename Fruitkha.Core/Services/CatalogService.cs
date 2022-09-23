using AutoMapper;
using Fruitkha.Core.Dtos.Catalog;
using Fruitkha.Core.Entities;
using Fruitkha.Core.Interfaces;

namespace Fruitkha.Core.Services;

public class CatalogService : ICatalogService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;

    public CatalogService(IRepository<Category> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<CategoryDto>> GetCategoriesList()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}