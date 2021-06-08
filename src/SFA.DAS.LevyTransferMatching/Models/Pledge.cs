using SFA.DAS.LevyTransferMatching.Models.Enums;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public class Pledge : PledgeReference
    {
        public string EncodedAccountId { get; set; }
        public int Amount { get; set; }
        public bool IsNamePublic { get; set; }
        public IEnumerable<Sector> Sectors { get; set; }
        public IEnumerable<JobRole> JobRoles { get; set; }
        public IEnumerable<Level> Levels { get; set; }
    }
}
