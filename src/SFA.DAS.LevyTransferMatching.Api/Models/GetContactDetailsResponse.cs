using SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class GetContactDetailsResponse
    {
        public int AllSectorsCount { get; set; }
        public int AllJobRolesCount { get; set; }
        public int AllLevelsCount { get; set; }
        public int OpportunityAmount { get; set; }
        public string OpportunityDasAccountName { get; set; }
        public IEnumerable<string> OpportunitySectorDescriptions { get; set; }
        public IEnumerable<string> OpportunityJobRoleDescriptions { get; set; }
        public IEnumerable<string> OpportunityLevelDescriptions { get; set; }
        public bool OpportunityIsNamePublic { get; set; }

        public static implicit operator GetContactDetailsResponse(GetContactDetailsResult getContactDetailsResult)
        {
            return new GetContactDetailsResponse()
            {
                AllSectorsCount = getContactDetailsResult.AllSectorsCount,
                AllJobRolesCount = getContactDetailsResult.AllJobRolesCount,
                AllLevelsCount = getContactDetailsResult.AllLevelsCount,
                OpportunityAmount = getContactDetailsResult.OpportunityAmount,
                OpportunityDasAccountName = getContactDetailsResult.OpportunityDasAccountName,
                OpportunitySectorDescriptions = getContactDetailsResult.OpportunitySectorDescriptions,
                OpportunityJobRoleDescriptions = getContactDetailsResult.OpportunityJobRoleDescriptions,
                OpportunityLevelDescriptions = getContactDetailsResult.OpportunityLevelDescriptions,
                OpportunityIsNamePublic = getContactDetailsResult.OpportunityIsNamePublic,
            };
        }
    }
}