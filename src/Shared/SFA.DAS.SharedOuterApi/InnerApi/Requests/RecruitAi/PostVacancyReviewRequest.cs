using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RecruitAi;

public class PostVacancyReviewRequest(Guid vacancyReviewId, PostVacancyReviewDto data) : IPostApiRequest
{
    public string PostUrl => $"api/llm/vacancyReview/{vacancyReviewId}/review";
    public object Data { get; set; } = data;
}

public sealed record PostVacancyReviewDto(
    Guid VacancyId,
    string Title,
    string ShortDescription,
    string Description,
    string EmployerDescription,
    string Skills,
    string Qualifications,
    string ThingsToConsider,
    string TrainingDescription,
    string AdditionalTrainingDescription,
    string TrainingProgrammeTitle,
    string TrainingProgrammeLevel
);