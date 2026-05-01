using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReviewsByFilter;

public class GetVacancyReviewsByFilterQuery : IRequest<GetVacancyReviewsByFilterQueryResult>
{
    public List<string>? Status { get; set; }
    public DateTime? ExpiredAssignationDateTime { get; set; }
}