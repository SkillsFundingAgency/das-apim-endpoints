using System;
using System.Collections.Generic;

namespace SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor
{
    public class GetFrameworkCertificateResponse
    {
        public Guid Id { get; set; }
        public string FrameworkCertificateNumber { get; set; }
        public string ApprenticeForename { get; set; }
        public string ApprenticeSurname { get; set; }
        public long? ApprenticeULN { get; set; }
        public string FrameworkName { get; set; }
        public string PathwayName { get; set; }
        public string ApprenticeshipLevelName { get; set; }
        public List<QualificationAndAwardingBody> QualificationsAndAwardingBodies { get; set; }
        public string ProviderName { get; set; }
        public string EmployerName { get; set; }
        public DateTime? ApprenticeStartdate { get; set; }
        public DateTime? PrintRequestedAt { get; set; }
        public string PrintRequestedBy { get; set; }
        public DateTime CertificationDate { get; set; }
        public string CertificateReference { get; set; }
    }

    public class QualificationAndAwardingBody
    {
        public string Name { get; set; }
        public string AwardingBody { get; set; }
    }
}