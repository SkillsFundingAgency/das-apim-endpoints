using System.Collections.Generic;
using SFA.DAS.Approvals.Types;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class ProviderStandardResults
    {
        public bool IsMainProvider { get; set; }
        public ICollection<Standard> ProviderStandards { get; set; }
    }
}
