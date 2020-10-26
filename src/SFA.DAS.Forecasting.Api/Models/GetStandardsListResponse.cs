using System.Collections.Generic;

namespace SFA.DAS.Forecasting.Api.Models
{
    public class GetStandardsListResponse
    {
        public IEnumerable<ApprenticeshipCourse> Standards { get; set; }
    }
}