using System;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Assessors.InnerApi.Responses
{
    public class StandardDetailResponse : StandardApiResponseBase
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
        public bool OtherBodyApprovalRequired { get; set; }
        public string ApprovalBody { get; set; }
        public List<string> Duties { get; set; }
        public bool CoreAndOptions { get; set; }
        public List<string> CoreDuties { get; set; }
        public bool IntegratedApprenticeship { get; set; }
        public List<string> Options { get; set; }

        public StandardVersionDetail VersionDetail { get; set; }

        public EqaProvider EqaProvider { get; set; }

        public bool EPAChanged { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }

        protected override int GetFundingDetails(string prop, DateTime? effectiveDate = null)
        {
            if (ApprenticeshipFunding == null || ApprenticeshipFunding.Any() == false)
            {
                switch (prop)
                {
                    case nameof(MaxFunding):
                        return VersionDetail?.ProposedMaxFunding ?? 0;
                    case nameof(TypicalDuration):
                        return VersionDetail?.ProposedTypicalDuration ?? 0;
                }
            }

            return base.GetFundingDetails(prop, effectiveDate);
        }
    }
}
