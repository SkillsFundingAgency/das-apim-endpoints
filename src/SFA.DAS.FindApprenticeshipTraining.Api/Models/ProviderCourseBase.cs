using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class ProviderCourseBase
    {
        private string MapLevel(int level)
        {
            if (level == 2)
            {
                return "Two";
            }
            if (level == 3)
            {
                return "Three";
            }
            if (level > 3)
            {
                return "AllLevels";
            }
            return "";
        }

        protected GetAchievementRateItem GetAchievementRateItem(IEnumerable<GetAchievementRateItem> list, string subjectArea, int level)
        {
            if (list == null)
                return null;
            
            var result = list.Where(c =>
                c.SectorSubjectArea.Equals(subjectArea, StringComparison.CurrentCultureIgnoreCase)).ToList();

            if (result.Count == 0)
                return null;
            
            var item = result.FirstOrDefault(c => c.Level.Equals(MapLevel(level))) 
                       ?? result.FirstOrDefault(c => c.Level.Equals("AllLevels"));

            return item;
        }
    }
}