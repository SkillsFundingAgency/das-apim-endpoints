using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Recruit.Application.Services;

public interface IRecruitArtificialIntelligenceService
{
    Task SendVacancyReviewAsync(Vacancy vacancy, CancellationToken cancellationToken);
}
    
public class RecruitArtificialIntelligenceService(
    ILogger<RecruitArtificialIntelligenceService> logger,
    IRecruitArtificialIntelligenceClient aiClient,
    ICourseService courseService) : IRecruitArtificialIntelligenceService
{
    private async Task<TrainingProgramme> GetTrainingProgrammeById(string programmeId)
    {
        var standards = await courseService.GetActiveStandards<GetStandardsListResponse>("ActiveStandards");
        var allTrainingProgrammes = standards.Standards?
            .Select(item => (TrainingProgramme)item)
            .ToList() ?? [];

        return allTrainingProgrammes.FirstOrDefault(c => c.Id.Equals(programmeId, StringComparison.CurrentCultureIgnoreCase));
    }

    public async Task SendVacancyReviewAsync(Vacancy vacancy, CancellationToken cancellationToken)
    {
        var programme = await GetTrainingProgrammeById(vacancy.ProgrammeId);
        if (programme is null)
        {
            logger.LogWarning("Failed to locate programme '{ProgrammeId}' for vacancy '{VacancyId}' when trying to send VacancyReview details to AI service", vacancy.ProgrammeId, vacancy.Id);
            return;
        }

        var skills = JsonSerializer.Serialize(vacancy.Skills ?? [], Global.JsonSerializerOptionsCaseInsensitive);
        var qualifications = JsonSerializer.Serialize(vacancy.Qualifications ?? [], Global.JsonSerializerOptionsCaseInsensitive);

        var payload = new AiVacancyReviewData(
            vacancy.Id,
            vacancy.Title,
            vacancy.ShortDescription,
            vacancy.Description,
            vacancy.EmployerDescription,
            skills,
            qualifications,
            vacancy.ThingsToConsider,
            vacancy.TrainingDescription,
            vacancy.AdditionalTrainingDescription,
            programme.Title,
            $"Level {programme.EducationLevelNumber}");

        await aiClient.SendPayloadAsync(payload, cancellationToken);
    }
}

public sealed record AiVacancyReviewData(
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