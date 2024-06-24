using System.Collections.Generic;
using System.Threading.Tasks;
using Contentful.Core.Models;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.Client
{
    public interface IContentClient
    {
        Task<List<T>> GetEntries<T>(string contentType, string field, string value) where T : IEntity;
        Task<ContentfulCollection<Page>> GetAllPages();
        Task<Page> GetByEntryId(string id);
        Task<ContentfulCollection<T>> GetPagesByContentType<T>(string contentType) where T : IEntity;
        Task<ContentfulCollection<Page>> GetByEntryIdWithChildren(string id);
        Task<ContentfulCollection<Page>> GetBySlugWithChildren(string slug);
    }
}