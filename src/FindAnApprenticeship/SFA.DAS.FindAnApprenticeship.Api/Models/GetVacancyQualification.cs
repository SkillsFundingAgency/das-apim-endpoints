using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class GetVacancyQualification
    {
        public QualificationWeighting Weighting { get ; set ; }
        public string QualificationType { get ; set ; }
        public string Subject { get ; set ; }
        public string Grade { get ; set ; }
        
        public static implicit operator GetVacancyQualification(GetVacancyQualificationResponseItem source)
        {
            return new GetVacancyQualification
            {
                QualificationType = source.QualificationType,
                Grade = source.Grade,
                Subject = source.Subject,
                Weighting = (QualificationWeighting)source.Weighting
            };
        }

        public enum QualificationWeighting
        {
            Essential,
            Desired
        }
    }
}