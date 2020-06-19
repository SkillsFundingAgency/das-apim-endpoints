using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request);
        Task<string> Ping();
    }
}
