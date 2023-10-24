using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions
{
    public class GetApplicationsForAutomaticRejectionResponse
    {
        public IEnumerable<Application> Applications { get; set; }

        public class Application
        {
            public int Id { get; set; }
            public int PledgeId { get; set; }
        }

        public static implicit operator GetApplicationsForAutomaticRejectionResponse(
            GetApplicationsForAutomaticRejectionQueryResult source)
        {
            return new GetApplicationsForAutomaticRejectionResponse
            {
                Applications = source.Applications.Select(x => new Application { Id = x.Id, PledgeId = x.PledgeId })
            };
        }
    }
}
