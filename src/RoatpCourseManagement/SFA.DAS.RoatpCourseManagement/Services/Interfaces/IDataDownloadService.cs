using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Services.Interfaces
{
    public interface IDataDownloadService
    {
        Task<Stream> GetFileStream(string downloadPath);
    }
}