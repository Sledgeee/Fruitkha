namespace Fruitkha.Core.Dtos.Catalog;

public class CategoryDto
{
    public int Id { get; set; }
    public int ParentId { get; set; }
    public int RealCategory { get; set; }
    public string Name { get; set; } = string.Empty;
}