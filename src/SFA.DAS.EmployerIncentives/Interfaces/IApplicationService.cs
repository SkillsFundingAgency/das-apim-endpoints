using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IApplicationService
    {
        Task CreateIncentiveApplication(CreateIncentiveApplicationRequestData requestData);
        Task UpdateIncentiveApplication(UpdateIncentiveApplicationRequestData requestData);
        Task<long> GetApplicationLegalEntity(long accountId, Guid applicationId);
        Task ConfirmIncentiveApplication(ConfirmIncentiveApplicationRequest request, CancellationToken cancellationToken = default);
        Task<IncentiveApplicationDto> GetApplication(long accountId, Guid applicationId);
        Task<GetApplicationsResponse> GetApprenticeApplications(long accountId, long accountLegalEntityId);
    }
}
