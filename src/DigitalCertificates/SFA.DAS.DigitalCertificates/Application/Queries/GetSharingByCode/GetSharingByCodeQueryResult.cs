using SFA.DAS.DigitalCertificates.Models;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByCode
{
    public class GetSharingByCodeQueryResult
    {
        public SharingByCode Response { get; set; }
        public bool BothFound { get; set; }
    }
}
