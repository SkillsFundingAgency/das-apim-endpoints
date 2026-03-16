using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.VacanciesManage.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
    
    public class GetStandardsListItem
    {
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Route { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public DateTime? LastDateStarts { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}