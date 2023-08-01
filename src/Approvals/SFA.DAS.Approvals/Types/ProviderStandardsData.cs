using System.Collections.Generic;

namespace SFA.DAS.Approvals.Types
{
    public class ProviderStandardsData
    {
        public bool IsMainProvider { get; set; }
        public IEnumerable<Standard> Standards { get; set; }
    }
}
