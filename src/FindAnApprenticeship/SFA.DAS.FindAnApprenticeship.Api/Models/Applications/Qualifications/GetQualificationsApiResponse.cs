using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications
{
    public class GetQualificationsApiResponse
    {
        public bool? IsSectionCompleted { get; set; }

        public List<ApplicationQualificationApiResponse> Qualifications { get; set; }

        public List<QualificationTypeApiResponse> QualificationTypes { get; set; }

        
        public static implicit operator GetQualificationsApiResponse(GetQualificationsQueryResult source)
        {
            return new GetQualificationsApiResponse
            {
                IsSectionCompleted = source.IsSectionCompleted,
                Qualifications = source.Qualifications.Select(x=>(ApplicationQualificationApiResponse)x).ToList(),
                QualificationTypes = source.QualificationTypes.Select(x => (QualificationTypeApiResponse)x).ToList()
            };
        }
    }
}
