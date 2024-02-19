using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetStandardsListItem : StandardApiResponseBase
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string LevelEquivalent { get; set; }
        public string Version { get; set; }


        public string OverviewOfRole { get; set; }

        public string Keywords { get; set; }


        public string Route { get; set; }

        public string TypicalJobTitles { get; set; }
        public List<string> CoreSkills => GetCoreSkillsCount();

        public string StandardPageUrl { get; set; }

        public string IntegratedDegree { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public int SectorSubjectAreaTier1 { get; set; }
        public bool OtherBodyApprovalRequired { get; set; }
        public string ApprovalBody { get; set; }
        public List<string> Skills { get; set; }
        public bool CoreAndOptions { get; set; }
        public List<string> CoreDuties { get; set; }



        private List<string> GetCoreSkillsCount()
        {
            if (CoreAndOptions)
            {
                return CoreDuties;
            }
            return Skills;
        }
    }


}
