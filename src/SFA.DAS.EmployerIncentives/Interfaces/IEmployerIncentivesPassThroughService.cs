using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Models.PassThrough;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesPassThroughService
    {
        Task<HealthCheckResult> HealthCheck(CancellationToken cancellationToken = default);
        Task<InnerApiResponse> AddLegalEntity(long accountId, LegalEntityRequest legalEntityRequest, CancellationToken cancellationToken = default);
        Task<InnerApiResponse> RemoveLegalEntity(long accountId, long accountLegalEntityId, CancellationToken cancellationToken = default);
    }
}