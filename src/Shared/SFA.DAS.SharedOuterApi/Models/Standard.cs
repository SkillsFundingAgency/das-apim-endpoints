using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Models
{
    [ExcludeFromCodeCoverage]
    public class Standard
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Route { get; set; }
        public IEnumerable<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }

        public static explicit operator Standard(InnerApi.Responses.Courses.StandardDetailResponse source)
        {
            if (source == null) return null;

            return new Standard()
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode,
                Title = source.Title,
                Level = source.Level,
                Route = source.Route,
                ApprenticeshipFunding = source.ApprenticeshipFunding
            };
        }
    }
}
