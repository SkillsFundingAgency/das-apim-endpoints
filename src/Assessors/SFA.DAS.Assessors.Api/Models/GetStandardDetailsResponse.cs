using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Assessors.Api.Models
{
    public class GetStandardDetailsResponse
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public bool CoronationEmblem { get; set; }
        public bool EpaoMustBeApprovedByRegulatorBody { get; set; }
        public string Version { get; set; }
        public string OverviewOfRole { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public string TrailBlazerContact { get; set; }
        public string TypicalJobTitles { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Knowledge { get; set; }
        public List<string> Behaviours { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }

        public List<ApprenticeshipFundingResponse> ApprenticeshipFunding { get; set; }

        public StandardDatesResponse StandardDates { get; set; }

        public StandardVersionDetailResponse VersionDetail { get; set; }

        public EqaProviderResponse EqaProvider { get; set; }

        public bool OtherBodyApprovalRequired { get; set; }
        public string ApprovalBody { get; set; }
        public List<string> Duties { get; set; }
        public bool CoreAndOptions { get; set; }
        public List<string> CoreDuties { get; set; }
        public bool IntegratedApprenticeship { get; set; }
        public List<string> Options { get; set; }
        public int MaxFunding { get; set; }
        public int TypicalDuration { get; set; }
        public bool IsActive { get; set; }
        public bool EPAChanged { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }

        public static implicit operator GetStandardDetailsResponse(StandardDetailResponse source)
        {
            return new GetStandardDetailsResponse
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode,
                Status = source.Status,
                Title = source.Title,
                Level = source.Level,
                CoronationEmblem = source.CoronationEmblem,
                EpaoMustBeApprovedByRegulatorBody = source.EpaoMustBeApprovedByRegulatorBody,
                Version = source.Version,
                OverviewOfRole = source.OverviewOfRole,
                Keywords = source.Keywords,
                Route = source.Route,
                AssessmentPlanUrl = source.AssessmentPlanUrl,
                TrailBlazerContact = source.TrailBlazerContact,
                TypicalJobTitles = source.TypicalJobTitles,
                Skills = source.Skills,
                Knowledge = source.Knowledge,
                Behaviours = source.Behaviours,
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                SectorSubjectAreaTier2Description = source.SectorSubjectAreaTier2Description,
                ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c => (ApprenticeshipFundingResponse)c).ToList(),
                StandardDates = source.StandardDates,
                VersionDetail = source.VersionDetail,
                EqaProvider = source.EqaProvider,
                OtherBodyApprovalRequired = source.OtherBodyApprovalRequired,
                ApprovalBody = source.ApprovalBody,
                Duties = source.Duties,
                CoreAndOptions = source.CoreAndOptions,
                CoreDuties = source.CoreDuties,
                IntegratedApprenticeship = source.IntegratedApprenticeship,
                Options = source.Options,
                MaxFunding = source.MaxFunding,
                TypicalDuration = source.TypicalDuration,
                IsActive = source.IsActive,
                EPAChanged = source.EPAChanged,
                VersionMajor = source.VersionMajor,
                VersionMinor = source.VersionMinor
            };
        }
    }
    public class ApprenticeshipFundingResponse
    {
        public int MaxEmployerLevyCap { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public int Duration { get; set; }

        public static implicit operator ApprenticeshipFundingResponse(ApprenticeshipFunding source)
            => new ApprenticeshipFundingResponse
            {
                MaxEmployerLevyCap = source.MaxEmployerLevyCap,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                Duration = source.Duration
            };
    }

    public class StandardDatesResponse
    {
        public DateTime? LastDateStarts { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime EffectiveFrom { get; set; }

        public static implicit operator StandardDatesResponse(StandardDate source)
        {
            if (source == null) return null;
            return new StandardDatesResponse
            {
                LastDateStarts = source.LastDateStarts,
                EffectiveTo = source.EffectiveTo,
                EffectiveFrom = source.EffectiveFrom
            };
        }
    }

    public class StandardVersionDetailResponse
    {
        public DateTime? EarliestStartDate { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public DateTime? LatestEndDate { get; set; }
        public DateTime? ApprovedForDelivery { get; set; }
        public int ProposedTypicalDuration { get; set; }
        public int ProposedMaxFunding { get; set; }

        public static implicit operator StandardVersionDetailResponse(StandardVersionDetail source)
            => new StandardVersionDetailResponse
            {
                EarliestStartDate = source.EarliestStartDate,
                LatestStartDate = source.LatestStartDate,
                LatestEndDate = source.LatestEndDate,
                ApprovedForDelivery = source.ApprovedForDelivery,
                ProposedTypicalDuration = source.ProposedTypicalDuration,
                ProposedMaxFunding = source.ProposedMaxFunding
            };
    }

    public class EqaProviderResponse
    {
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string WebLink { get; set; }

        public static implicit operator EqaProviderResponse(EqaProvider source)
            => new EqaProviderResponse
            {
                Name = source.Name,
                ContactName = source.ContactName,
                ContactEmail = source.ContactEmail,
                WebLink = source.WebLink
            };
    }
}
