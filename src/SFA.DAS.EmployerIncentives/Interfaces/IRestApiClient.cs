using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IRestApiClient
    {
        Task<string> GetAsync(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<string> GetAsync(string uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<T> GetAsync<T>(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<T> GetAsync<T>(string uri, object queryData = null, CancellationToken cancellationToken = default);

        Task<string> PostAsync(string uri, CancellationToken cancellationToken = default);
        Task<string> PostAsync<TRequest>(string uri, TRequest request, CancellationToken cancellationToken = default) where TRequest : class;

        Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest requestData, CancellationToken cancellationToken = default) where TRequest : class where TResponse : class, new();
    }
}
