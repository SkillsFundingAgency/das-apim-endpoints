using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public class GetAllApplicationsByIdApiResponse
    {
        public List<Domain.Application> Applications { get; set; }
    }
}
