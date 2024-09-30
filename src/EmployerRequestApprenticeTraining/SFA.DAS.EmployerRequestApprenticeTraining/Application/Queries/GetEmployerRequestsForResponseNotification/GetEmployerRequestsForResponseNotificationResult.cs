using SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining;
using System.Collections.Generic;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification
{
    public class GetEmployerRequestsForResponseNotificationResult
    {
        public List<EmployerRequestForResponseNotification> EmployerRequests { get; set; }
    }
}
