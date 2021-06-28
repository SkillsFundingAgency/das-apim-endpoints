using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public class Pledge
    {
        public long AccountId { get; set; }
        public int Amount { get; set; }
        public bool IsNamePublic { get; set; }
        public IEnumerable<string> Sectors { get; set; }
        public IEnumerable<string> JobRoles { get; set; }
        public IEnumerable<string> Levels { get; set; }
    }
}
