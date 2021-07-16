using SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class GetContactDetailsResponse
    {
        public int AllSectorsCount { get; set; }
        public IEnumerable<ReferenceDataItem> PledgeSectors { get; set; }
        public int AllJobRolesCount { get; set; }
        public IEnumerable<ReferenceDataItem> PledgeJobRoles { get; set; }
        public int AllLevelsCount { get; set; }
        public IEnumerable<ReferenceDataItem> PledgeLevels { get; set; }
        public int PledgeAmount { get; set; }
        public string PledgeDasAccountName { get; set; }

        public static implicit operator GetContactDetailsResponse(GetContactDetailsResult getContactDetailsResult)
        {
            return new GetContactDetailsResponse()
            {
                AllJobRolesCount = getContactDetailsResult.AllJobRolesCount,
                AllLevelsCount = getContactDetailsResult.AllLevelsCount,
                AllSectorsCount = getContactDetailsResult.AllSectorsCount,
                PledgeAmount = getContactDetailsResult.PledgeAmount,
                PledgeDasAccountName = getContactDetailsResult.PledgeDasAccountName,
                PledgeJobRoles = getContactDetailsResult.PledgeJobRoles,
                PledgeLevels = getContactDetailsResult.PledgeLevels,
                PledgeSectors = getContactDetailsResult.PledgeSectors,
            };
        }
    }
}