using System;
using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharedStandardCertificate
{
    public class GetSharedStandardCertificateQueryResult
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
        

        public static implicit operator GetSharedStandardCertificateQueryResult(GetStandardCertificateResponse source)
        {
            if (source == null) return null;

            return new GetSharedStandardCertificateQueryResult
            {
                FamilyName = source.LearnerFamilyName,
                GivenNames = source.LearnerGivenNames,
                CertificateType = source.Type,
                CertificateReference = source.CertificateReference,
                CourseName = source.StandardName,
                CourseOption = source.CourseOption,
                CourseLevel = source.StandardLevel,
                DateAwarded = source.AchievementDate,
                OverallGrade = source.OverallGrade,
                ProviderName = source.ProviderName,
                StartDate = source.LearningStartDate,
            };
        }
    }
}
