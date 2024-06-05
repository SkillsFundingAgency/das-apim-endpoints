using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.MigrateData;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetMigrateDataTransferApiResponse
    {
        public List<Application> Applications { get; set; }

        public static implicit operator GetMigrateDataTransferApiResponse(MigrateDataQueryResult source)
        {
            return new GetMigrateDataTransferApiResponse
            {
                Applications = source.Applications.Select(x => (Application)x).ToList()
            };
        }

        public class Application
        {
            public ApplicationStatus Status { get; set; }
            

            public static implicit operator Application(GetLegacyApplicationsByEmailApiResponse.Application application)
            {
                return new Application
                {
                    Status = application.Status,
                };
            }
        }
    }
}
