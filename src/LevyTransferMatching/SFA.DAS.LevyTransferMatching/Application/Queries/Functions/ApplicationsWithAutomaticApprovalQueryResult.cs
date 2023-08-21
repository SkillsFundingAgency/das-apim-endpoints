using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching.GetApplicationsResponse;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions
{
    public class ApplicationsWithAutomaticApprovalQueryResult
    {
        public IEnumerable<Application> Applications { get; set; }

        public class Application
        {
            public int Id { get; set; }
            public long EmployerAccountId { get; set; }
            public int PledgeId { get; set; }
            public int MatchPercentage { get; set; }
            public AutomaticApprovalOption PledgeAutomaticApprovalOption { get; set; }
            public int Amount { get; set; }
            public DateTime CreatedOn { get; set; }
            public string Status { get; set; }

            public static implicit operator Application(GetApplicationsResponse.Application source)
            {
                if (source == null)
                {
                    return null;
                }

                return new Application
                {
                    Id = source.Id,
                    EmployerAccountId = source.EmployerAccountId,
                    PledgeId = source.PledgeId,
                    MatchPercentage = source.MatchPercentage,
                    PledgeAutomaticApprovalOption = source.PledgeAutomaticApprovalOption,
                    Amount = source.Amount,
                    CreatedOn = source.CreatedOn,
                    Status = source.Status
                };
            }
        }        
    }
}
