using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses
{
    public class GetStandardResponse
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public string LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Version { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public string ApprovalBody { get; set; }
        public string Route { get; set; }
        public int SectorSubjectAreaTier1 { get; set; }
        public bool IsRegulatedForProvider { get; set; }

        public static implicit operator GetStandardResponse(GetStandardResponseFromCoursesApi source) =>
            new()
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode.ToString(),
                Title = source.Title,
                Level = source.Level,
                Version = source.Version,
                ApprenticeshipType = source.ApprenticeshipType,
                ApprovalBody = source.ApprovalBody,
                Route = source.Route,
                SectorSubjectAreaTier1 = source.SectorSubjectAreaTier1,
                IsRegulatedForProvider = source.IsRegulatedForProvider
            };
    }

    public class GetStandardResponseFromCoursesApi
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Version { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public string ApprovalBody { get; set; }
        public string Route { get; set; }
        public int SectorSubjectAreaTier1 { get; set; }
        public bool IsRegulatedForProvider { get; set; }
    }
}
