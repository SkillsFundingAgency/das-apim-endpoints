using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails
{
    public class GetContactDetailsResult
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
    }
}