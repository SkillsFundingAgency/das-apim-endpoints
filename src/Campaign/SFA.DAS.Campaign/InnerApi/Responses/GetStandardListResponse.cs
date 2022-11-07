using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace SFA.DAS.Campaign.InnerApi.Responses
{
    public class GetStandardListResponse
    {
        [JsonPropertyName("standardUId")]
        public string StandardUId { get; set; }

        [JsonPropertyName("iFateReferenceNumber")]
        public string iFateReferenceNumber { get; set; }

        [JsonPropertyName("larsCode")]
        public int LarsCode { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("searchScore")]
        public int? SearchScore { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("overviewOfRole")]
        public string OverviewOfRole { get; set; }

        [JsonPropertyName("keywords")]
        public string Keywords { get; set; }

        [JsonPropertyName("route")]
        public string Route { get; set; }

        [JsonPropertyName("assessmentPlanUrl")]

        public string AssessmentPlanUrl { get; set; }
        [JsonPropertyName("trailblazerContact")]
        public string TrailblazerContact { get; set; }

        [JsonPropertyName("typicalJobTitles")]
        public string TypicalJobTitles { get; set; }

        [JsonPropertyName("skills")]
        public List<string> Skills { get; set; }

        [JsonPropertyName("knowledge")]
        public List<string> Knowledge { get; set; }

        [JsonPropertyName("behaviours")]
        public List<string> Behaviours { get; set; }

        [JsonPropertyName("standardPageUrl")]
        public string StandardPageUrl { get; set; }

        [JsonPropertyName("integratedDegree")]
        public string IntegratedDegree { get; set; }

        [JsonPropertyName("sectorSubjectAreaTier2")]
        public double SectorSubjectAreaTier2 { get; set; }

        [JsonPropertyName("sectorSubjectAreaTier2Description")]
        public string SectorSubjectAreaTier2Description { get; set; }

        [JsonPropertyName("apprenticeshipFunding")]
        public List<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }

        [JsonPropertyName("standardDates")]
        public StandardDate StandardDates { get; set; }

        [JsonPropertyName("versionDetail")]
        public VersionDetail VersionDetail { get; set; }

        [JsonPropertyName("eqaProvider")]
        public EqaProvider EqaProvider { get; set; }

        [JsonPropertyName("otherBodyApprovalRequired")]
        public bool OtherBodyApprovalRequired { get; set; }

        [JsonPropertyName("approvalBody")]
        public string ApprovalBody { get; set; }

        [JsonPropertyName("duties")]
        public List<string> Duties { get; set; }

        [JsonPropertyName("coreAndOptions")]
        public bool CoreAndOptions { get; set; }

        [JsonPropertyName("coreDuties")]
        public List<string> CoreDuties { get; set; }

        [JsonPropertyName("integratedApprenticeship")]
        public bool IntegratedApprenticeship { get; set; }

        [JsonPropertyName("options")]
        public List<string> Options { get; set; }

        [JsonPropertyName("sectorCode")]
        public int SectorCode { get; set; }

        [JsonPropertyName("epaChanged")]
        public bool EpaChanged { get; set; }

        [JsonPropertyName("versionMinor")]
        public int VersionMinor { get; set; }

        [JsonPropertyName("versionMajor")]
        public int VersionMajor { get; set; }
    }

    public class AprenticeshipFunding
    {
        public int MaxEmployerLevyCap { get; set; }
        public DateTime EffectiveTo { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public int Duration { get; set; }
    }

    public class StandardDate
    {
        public string LastDateStarts { get; set; }
        public string EffectiveTo { get; set; }
        public string EffectiveFrom { get; set; }
    }

    public class VersionDetail
    {
        public DateTime EarliestStartDate { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public DateTime? LatestEndDate { get; set; }
        public DateTime ApprovedForDelivery { get; set; }
        public int ProposedTypicalDuration { get; set; }
        public int ProposedMaxFunding { get; set; }
    }

    public class EqaProvider
    {
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string WebLink { get; set; }
    }
}
