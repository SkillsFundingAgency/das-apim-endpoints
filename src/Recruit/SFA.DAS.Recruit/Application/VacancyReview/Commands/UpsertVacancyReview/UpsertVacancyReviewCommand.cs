using System;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;

public class UpsertVacancyReviewCommand : IRequest
{
    public Guid Id { get; set; }
    public VacancyReviewDto VacancyReview { get; set; }
}