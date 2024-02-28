using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using System;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions
{
    public class GetApplicationsForAutomaticRejectionQueryResult
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

            public static  Application BuildApplication(GetApplicationsResponse.Application application)
            {
                if (application == null)
                {
                    return null;
                }

                return new Application
                {
                    Id = application.Id,
                    EmployerAccountId = application.EmployerAccountId,
                    PledgeId = application.PledgeId,
                    MatchPercentage = application.MatchPercentage,
                    PledgeAutomaticApprovalOption = application.PledgeAutomaticApprovalOption,
                    Amount = application.Amount,
                    CreatedOn = application.CreatedOn,
                    Status = application.Status
                };
            }
         
        }        
    }
}
