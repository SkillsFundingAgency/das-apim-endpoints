using System.Collections.Generic;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByUkprn
{
    public record GetApplicationReviewsCountByUkprnQuery(int Ukprn, List<long> VacancyReferences)
        : IRequest<List<ApplicationReviewStats>>;
}