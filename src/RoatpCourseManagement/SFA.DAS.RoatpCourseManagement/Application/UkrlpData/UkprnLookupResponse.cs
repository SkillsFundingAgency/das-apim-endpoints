using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData
{
    public class UkprnLookupResponse
    {
        public bool Success { get; set; }
        public List<ProviderDetails> Results { get; set; }
    }
}
