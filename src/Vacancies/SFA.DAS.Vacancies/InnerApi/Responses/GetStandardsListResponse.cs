using System.Collections.Generic;
using SFA.DAS.Apim.Shared.Common;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.Vacancies.InnerApi.Responses
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
        public LearningType ApprenticeshipType { get; set; }
    }
}