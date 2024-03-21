using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualifications;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications
{
    public class GetDeleteQualificationsApiResponse
    {
        public string QualificationReference { get; set; }

        public List<ApplicationQualificationApiResponse> Qualifications { get; set; }


        public static implicit operator GetDeleteQualificationsApiResponse(GetDeleteQualificationsQueryResult source)
        {
            return new GetDeleteQualificationsApiResponse
            {
                QualificationReference = source.QualificationReference,
                Qualifications = source.Qualifications.Select(x => (ApplicationQualificationApiResponse)x).ToList()
            };
        }
    }
}
