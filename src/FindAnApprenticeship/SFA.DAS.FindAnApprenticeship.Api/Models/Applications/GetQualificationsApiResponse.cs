using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetQualificationsApiResponse
    {
        public bool? IsSectionCompleted { get; set; }

        public List<Qualification> Qualifications { get; set; }

        public class Qualification
        {
            public static implicit operator Qualification(GetQualificationsQueryResult.Qualification source)
            {
                return new Qualification();
            }
        }

        public static implicit operator GetQualificationsApiResponse(GetQualificationsQueryResult source)
        {
            return new GetQualificationsApiResponse
            {
                IsSectionCompleted = source.IsSectionCompleted,
                Qualifications = source.Qualifications.Select(x => (Qualification)x).ToList()
            };
        }
    }
}
