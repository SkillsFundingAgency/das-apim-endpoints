using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Commands.UpsertVacancyReview;

public class UpsertVacancyReviewCommand : IRequest
{
    public Guid Id { get; set; }
    public VacancyReviewDto VacancyReview { get; set; }
}