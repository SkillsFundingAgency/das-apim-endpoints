using System.Collections.Generic;

namespace SFA.DAS.Forecasting.Api.Models
{
    public class GetFrameworksListResponse
    {
        public IEnumerable<ApprenticeshipCourse> Frameworks { get; set; }
    }
}