using SFA.DAS.SharedOuterApi.Domain;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses
{
    [ExcludeFromCodeCoverage]
    public class StandardDetailResponse : StandardApiResponseBase
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Status { get; set; }
        public float? SearchScore { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public bool CoronationEmblem { get; set; }
        public string Version { get; set; }
        public string OverviewOfRole { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public int RouteCode { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public string TrailBlazerContact { get; set; }
        public string TypicalJobTitles { get; set; }
        public List<string> Skills { get; set; }
        public string ApprenticeshipType { get; set; }
        public List<KsbResponse> Ksbs { get; set; }
        public List<RelatedOccupation> RelatedOccupations { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }
        public int? SectorSubjectAreaTier1 { get; set; }
        public string SectorSubjectAreaTier1Description { get; set; }

        public StandardVersionDetailResponse VersionDetail { get; set; }

        public EqaProviderResponse EqaProvider { get; set; }

        public bool OtherBodyApprovalRequired { get; set; }
        public string ApprovalBody { get; set; }
        public List<string> Duties { get; set; }
        public bool CoreAndOptions { get; set; }
        public List<string> CoreDuties { get; set; }
        public bool IntegratedApprenticeship { get; set; }
        public List<string> Options { get; set; }
        public int SectorCode { get; set; }
        public bool EPAChanged { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public bool EpaoMustBeApprovedByRegulatorBody { get; set; }
    }
}
