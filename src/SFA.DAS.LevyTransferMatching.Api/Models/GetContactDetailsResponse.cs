using SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class GetContactDetailsResponse
    {
        public IEnumerable<ReferenceDataItem> AllSectors { get; set; }
        public IEnumerable<ReferenceDataItem> AllJobRoles { get; set; }
        public IEnumerable<ReferenceDataItem> AllLevels { get; set; }
        public int Amount { get; set; }
        public string DasAccountName { get; set; }
        public IEnumerable<string> Sectors { get; set; }
        public IEnumerable<string> JobRoles { get; set; }
        public IEnumerable<string> Levels { get; set; }
        public IEnumerable<string> Locations { get; set; }
        public bool IsNamePublic { get; set; }

        public static implicit operator GetContactDetailsResponse(GetContactDetailsResult getContactDetailsResult)
        {
            return new GetContactDetailsResponse()
            {
                AllSectors = getContactDetailsResult.AllSectors,
                AllJobRoles = getContactDetailsResult.AllJobRoles,
                AllLevels = getContactDetailsResult.AllLevels,
                Amount = getContactDetailsResult.Amount,
                DasAccountName = getContactDetailsResult.DasAccountName,
                Sectors = getContactDetailsResult.Sectors,
                JobRoles = getContactDetailsResult.JobRoles,
                Levels = getContactDetailsResult.Levels,
                Locations = getContactDetailsResult.Locations,
                IsNamePublic = getContactDetailsResult.IsNamePublic,
            };
        }
    }
}