using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByUkprn
{
    public record GetApplicationReviewsCountByUkprnQuery(int Ukprn, List<long> VacancyReferences)
        : IRequest<GetApplicationReviewsCountByUkprnResult>;
}