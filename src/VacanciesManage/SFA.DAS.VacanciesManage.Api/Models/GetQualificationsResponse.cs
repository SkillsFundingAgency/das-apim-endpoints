using System.Collections.Generic;
using SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetQualifications;

namespace SFA.DAS.VacanciesManage.Api.Models
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