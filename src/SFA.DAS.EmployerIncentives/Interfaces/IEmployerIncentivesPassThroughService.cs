using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Models.PassThrough;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesPassThroughService
    {
        Task<InnerApiResponse> AddLegalEntity(long accountId, LegalEntityRequest legalEntityRequest, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> RemoveLegalEntity(long accountId, long accountLegalEntityId, CancellationToken cancellationToken = default);
    }
}