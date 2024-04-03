using System.Collections.Generic;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetEmployerRequestsResult
    {
        public List<EmployerRequest> EmployerRequests { get; set; }
    }
}
