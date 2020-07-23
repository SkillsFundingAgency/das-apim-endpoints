using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IPassThroughApiClient
    {
        Task<InnerApiResponse> Get(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> Get(string uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> Post(string uri, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> Post<TRequest>(string uri, TRequest request, CancellationToken cancellationToken = default) where TRequest : class;
        Task<InnerApiResponse> Delete(string uri, CancellationToken cancellationToken = default);
    }
}
