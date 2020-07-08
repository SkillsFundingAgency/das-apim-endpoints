using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IPassThroughApiClient
    {
        Task<InnerApiResponse> GetAsync(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> GetAsync(string uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> PostAsync(string uri, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> PostAsync<TRequest>(string uri, TRequest request, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> DeleteAsync(string uri, CancellationToken cancellationToken = default);

    }
}
