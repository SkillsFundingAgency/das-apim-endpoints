using System.Threading.Tasks;
using Contentful.Core.Models;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.Services
{
    public interface IContentService
    {
        Task<ContentfulCollection<Page>> GetCategoriesByContentType<T>(string contentType);
        Task<Page> GetPageById(string id);
    }
}