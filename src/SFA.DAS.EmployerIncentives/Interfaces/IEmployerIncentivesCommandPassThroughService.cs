using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesCommandPassThroughService
    {
        Task<InnerApiResponse> PostAsync(string uri, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> PostAsync<TRequest>(string uri, TRequest request, CancellationToken cancellationToken = default) where TRequest : class;
        Task<InnerApiResponse> DeleteAsync(string uri, CancellationToken cancellationToken = default);
    }
}