using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using System;

namespace SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;

public class UpsertVacancyReviewCommand : IRequest
{
    public Guid Id { get; set; }
    public VacancyReviewDto VacancyReview { get; set; }
}