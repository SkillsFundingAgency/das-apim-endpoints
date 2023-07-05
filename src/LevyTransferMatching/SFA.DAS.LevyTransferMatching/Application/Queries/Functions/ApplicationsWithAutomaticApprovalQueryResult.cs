
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System;
using System.Collections.Generic;

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
            public bool? AutoApproveFullMatches { get; set; }
            public int TotalAmount { get; set; }
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
                    AutoApproveFullMatches = source.AutoApproveFullMatches,
                    TotalAmount = source.TotalAmount,
                    CreatedOn = source.CreatedOn,
                    Status = source.Status
                };
            }
        }        
    }
}
