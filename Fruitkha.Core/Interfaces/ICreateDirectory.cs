using System.Threading.Tasks;

namespace Fruitkha.Core.Interfaces
{
    public interface ICreateDirectory
    {
        Task CreateDirectoryAsync(string folderPath);
    }
}