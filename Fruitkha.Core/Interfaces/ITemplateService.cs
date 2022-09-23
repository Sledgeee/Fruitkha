using System.Threading.Tasks;

namespace Fruitkha.Core.Interfaces
{
    public interface ITemplateService
    {
        Task<string> GetTemplateHtmlAsStringAsync<T>(string viewName, T model) where T : class, new();
    }
}