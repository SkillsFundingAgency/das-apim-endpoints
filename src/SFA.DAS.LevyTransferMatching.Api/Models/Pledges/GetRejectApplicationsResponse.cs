using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetRejectApplications;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetRejectApplicationsResponse
    {
        public IEnumerable<Application> Applications { get; set; }

        public class Application
        {
            public int Id { get; set; }
            public string DasAccountName { get; set; }
            public static implicit operator Application(GetRejectApplicationsQueryResult.Application application)
            {
                return new Application
                {
                    Id = application.Id,
                    DasAccountName = application.DasAccountName,
                };
            }
        }
    }
}
