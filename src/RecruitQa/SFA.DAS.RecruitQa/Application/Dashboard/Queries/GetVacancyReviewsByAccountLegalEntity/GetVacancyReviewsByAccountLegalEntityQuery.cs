using MediatR;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByAccountLegalEntity;

public class GetVacancyReviewsByAccountLegalEntityQuery : IRequest<GetVacancyReviewsByAccountLegalEntityQueryResult>
{
    public long AccountLegalEntityId { get; set; }
}