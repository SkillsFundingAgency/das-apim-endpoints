using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface ICommitmentsService
    {
        Task<bool> IsHealthy();
        Task<GetApprenticeshipListResponse> Apprenticeships(long accountId, long accountLegalEntityId, DateTime startDateFrom, DateTime startDateTo, int pageNumber, int pageSize);
        Task<ApprenticeshipResponse[]> GetApprenticeshipDetails(long accountId, IEnumerable<long> apprenticeshipIds);
    }
}