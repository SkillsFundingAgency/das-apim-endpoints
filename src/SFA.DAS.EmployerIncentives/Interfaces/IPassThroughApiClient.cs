using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IPassThroughApiClient
    {
        Task<T> GetAsync<T>(string uri, CancellationToken cancellationToken = default); 
        Task<InnerApiResponse> GetAsync(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> GetAsync(string uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> PostAsync(string uri, bool readAsJson, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> PostAsync<TRequest>(string uri, TRequest request, bool readAsJson, CancellationToken cancellationToken = default) where TRequest : class;
        Task<InnerApiResponse> DeleteAsync(string uri, bool readAsJson, CancellationToken cancellationToken = default);
    }
}
