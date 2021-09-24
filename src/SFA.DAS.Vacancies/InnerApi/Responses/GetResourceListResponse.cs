using System.Collections.Generic;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class GetResourceListResponse
    {
        public IEnumerable<Resource> Resources { get; set; }
    }
}