using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReview;

public class GetVacancyReviewQuery : IRequest<GetVacancyReviewQueryResult>
{
    public Guid Id { get; set; }
}