using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails
{
    public class GetContactDetailsResult
    {
        public int AllSectorsCount { get; set; }
        public IEnumerable<ReferenceDataItem> PledgeSectors { get; set; }
        public int AllJobRolesCount { get; set; }
        public IEnumerable<ReferenceDataItem> PledgeJobRoles { get; set; }
        public int AllLevelsCount { get; set; }
        public IEnumerable<ReferenceDataItem> PledgeLevels { get; set; }
        public int PledgeAmount { get; set; }
        public string PledgeDasAccountName { get; set; }
    }
}