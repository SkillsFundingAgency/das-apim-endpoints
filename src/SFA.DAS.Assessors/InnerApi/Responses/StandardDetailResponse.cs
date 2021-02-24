using SFA.DAS.SharedOuterApi.InnerApi.Responses;
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
        public float? SearchScore { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public decimal Version { get; set; }
        public string OverviewOfRole { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public string TypicalJobTitles { get; set; }
        public string CoreSkillsCount { get; set; }
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
        public string CoreDuties { get; set; }
        public bool IntegratedApprenticeship { get; set; }

        public StandardVersionDetail VersionDetail { get; set; }

        public EqaProvider EqaProvider { get; set; }

        protected override int GetFundingDetails(string prop)
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

            return base.GetFundingDetails(prop);
        }

    }
}
