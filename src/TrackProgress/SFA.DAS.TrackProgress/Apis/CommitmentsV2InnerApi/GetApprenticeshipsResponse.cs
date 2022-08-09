using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi
{
    public class GetApprenticeshipsResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public IEnumerable<ApprenticeshipDetailsResponse>? Apprenticeships { get; set; }
        public int TotalApprenticeshipsFound { get; set; }

        public class ApprenticeshipDetailsResponse
        {
            public long Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Uln { get; set; }
            public string EmployerName { get; set; }
            public string ProviderName { get; set; }
            public long ProviderId { get; set; }
            public string CourseName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime PauseDate { get; set; }
            public DateTime DateOfBirth { get; set; }            
        }
    }
}
