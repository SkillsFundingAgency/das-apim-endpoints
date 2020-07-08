using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IRestApiClient
    {
        Task<string> Ping(CancellationToken cancellationToken = default);
        Task<string> Get(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<string> Get(string uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<T> Get<T>(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<T> Get<T>(string uri, object queryData = null, CancellationToken cancellationToken = default);

        Task<string> Post(string uri, CancellationToken cancellationToken = default);
        Task<string> Post<TRequest>(string uri, TRequest request, CancellationToken cancellationToken = default);
        Task<TResponse> Post<TRequest, TResponse>(string uri, TRequest requestData, CancellationToken cancellationToken = default);
    }
}
