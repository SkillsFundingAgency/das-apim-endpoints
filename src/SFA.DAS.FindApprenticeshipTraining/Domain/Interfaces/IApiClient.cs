using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<string> Ping();
    }
}
