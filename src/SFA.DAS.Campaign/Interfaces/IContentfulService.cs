using System.Threading;
using System.Threading.Tasks;
using Contentful.Core.Search;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Interfaces
{
    public interface IContentfulService
    {
        /// <summary>
        /// this won't resolve any child objects for the requested type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entryId"></param>
        /// <param name="queryBuilder"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> GetEntryForAsync<T>(string entryId, QueryBuilder<T> queryBuilder,
            CancellationToken cancellationToken = default) where T : IContentType;

        /// <summary>
        /// This resolves the child objects for the requested article
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Article> GetArticleForAsync(string entryId, CancellationToken cancellationToken = default);

        Task<Article> GetArticleForAsync(string hubType, string slug, CancellationToken cancellationToken = default);
    }
}