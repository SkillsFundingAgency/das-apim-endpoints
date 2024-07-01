using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetEmployerRequestsResult
    {
        public List<EmployerRequest> EmployerRequests { get; set; }
    }
}
