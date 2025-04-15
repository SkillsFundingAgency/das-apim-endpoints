﻿using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public class GetStandardsListItem : StandardApiResponseBase
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public int SectorCode { get;set; }
        public string OverviewOfRole { get; set; }


        public List<string> CoreSkills => GetCoreSkillsCount();

        public string StandardPageUrl { get; set; }

        public List<string> Skills { get; set; }
        public bool CoreAndOptions { get; set; }
        public List<string> CoreDuties { get; set; }
        private List<string> GetCoreSkillsCount()
        {
            if (CoreAndOptions)
            {
                return CoreDuties;
            }
            return  Skills;
        }
        public string ApprenticeshipType { get; set; }
    }
}