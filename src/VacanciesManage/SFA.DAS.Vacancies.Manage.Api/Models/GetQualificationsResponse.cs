using System.Collections.Generic;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetQualifications;

namespace SFA.DAS.Vacancies.Manage.Api.Models
{
    public class GetQualificationsResponse
    {
        public List<string> Qualifications { get ; set ; }

        public static implicit operator GetQualificationsResponse(
            GetQualificationsQueryResponse source)
        {
            if (source.Qualifications == null)
            {
                return new GetQualificationsResponse
                {
                    Qualifications = new List<string>()
                };
            }
            
            return new GetQualificationsResponse
            {
                Qualifications = source.Qualifications
            };
        }
    }
}