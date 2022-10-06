using System.Collections.Generic;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.Ukrlp;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData
{
    public class UkprnLookupResponse
    {
        public bool Success { get; set; }
        public List<ProviderAddress> Results { get; set; }
    }
}
