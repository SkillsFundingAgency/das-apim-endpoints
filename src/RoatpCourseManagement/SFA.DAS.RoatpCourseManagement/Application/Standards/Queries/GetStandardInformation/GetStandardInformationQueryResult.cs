using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation
{
    public class GetStandardInformationQueryResult
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public string LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Version { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public string RegulatorName { get; set; }
        public string Sector { get; set; }
        public bool IsRegulatedForProvider { get; set; }

        public static implicit operator GetStandardInformationQueryResult(GetStandardResponseFromCoursesApi source) =>
            new()
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode.ToString(),
                Title = source.Title,
                Level = source.Level,
                ApprenticeshipType = source.ApprenticeshipType,
                RegulatorName = source.ApprovalBody,
                Sector = source.Route,
                IsRegulatedForProvider = source.IsRegulatedForProvider
            };
    }
}
