using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Responses
{
    public record GetApplicationsByVacancyReferenceApiResponse
    {
        public List<Domain.Application> Applications { get; set; } = [];
    }
}