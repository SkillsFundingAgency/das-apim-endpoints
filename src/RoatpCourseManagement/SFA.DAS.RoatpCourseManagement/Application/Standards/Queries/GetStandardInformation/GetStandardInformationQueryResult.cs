using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation
{
    public class GetStandardInformationQueryResult
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Version { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public string RegulatorName { get; set; }
        public string Sector { get; set; }
        public bool IsRegulatedForProvider { get; set; }

        public static implicit operator GetStandardInformationQueryResult(GetStandardResponse source) =>
            new()
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode,
                Title = source.Title,
                Level = source.Level,
                Version = source.Version,
                ApprenticeshipType = source.ApprenticeshipType,
                RegulatorName = source.ApprovalBody,
                Sector = source.Route,
                IsRegulatedForProvider = source.IsRegulatedForProvider
            };
    }
}
