using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails
{
    public class GetContactDetailsResult
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
    }
}