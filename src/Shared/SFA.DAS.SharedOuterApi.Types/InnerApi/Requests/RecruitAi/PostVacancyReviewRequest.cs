using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RecruitAi;

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
    string ThingsToConsider,
    string TrainingDescription,
    string AdditionalTrainingDescription,
    string TrainingProgrammeTitle,
    string TrainingProgrammeLevel,
    string OutcomeDescription,
    string ApplicationInstructions,
    string AdditionalQuestion1,
    string AdditionalQuestion2,
    string WageAdditionalInformation,
    string WageCompanyBenefitsInformation,
    string WageWorkingWeekDescription
);