using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IApplicationService
    {
        Task Create(CreateIncentiveApplicationRequestData requestData);
        Task Update(UpdateIncentiveApplicationRequestData requestData);
        Task<long> GetApplicationLegalEntity(long accountId, Guid applicationId);
        Task Confirm(ConfirmIncentiveApplicationRequest request, CancellationToken cancellationToken = default);
        Task<IncentiveApplicationDto> Get(long accountId, Guid applicationId);
        Task<PaymentApplicationsDto> GetPaymentApplications(long accountId, long accountLegalEntityId);
    }
}
