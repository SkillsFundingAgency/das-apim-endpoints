using System;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharedStandardCertificate;

namespace SFA.DAS.DigitalCertificates.Api.Models.Sharing
{
    public class GetSharedStandardCertificateResponse
    {
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public string CertificateType { get; set; }
        public string CertificateReference { get; set; }
        public string CourseName { get; set; }
        public string CourseOption { get; set; }
        public int? CourseLevel { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string OverallGrade { get; set; }
        public string ProviderName { get; set; }
        public DateTime? StartDate { get; set; }
        public bool CoronationEmblem { get; set; }

        public static implicit operator GetSharedStandardCertificateResponse(GetSharedStandardCertificateQueryResult source)
        {
            if (source == null) return null;

            return new GetSharedStandardCertificateResponse
            {
                FamilyName = source.FamilyName,
                GivenNames = source.GivenNames,
                CertificateType = source.CertificateType,
                CertificateReference = source.CertificateReference,
                CourseName = source.CourseName,
                CourseOption = source.CourseOption,
                CourseLevel = source.CourseLevel,
                DateAwarded = source.DateAwarded,
                OverallGrade = source.OverallGrade,
                ProviderName = source.ProviderName,
                StartDate = source.StartDate,
                CoronationEmblem = source.CoronationEmblem
            };
        }
    }
    
}
