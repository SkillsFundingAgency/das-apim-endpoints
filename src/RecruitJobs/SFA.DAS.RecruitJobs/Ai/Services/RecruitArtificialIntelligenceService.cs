using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.RecruitJobs.Ai.Clients;
using SFA.DAS.RecruitJobs.Configuration;
using SFA.DAS.SharedOuterApi.Domain.Recruit;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.Ai.Services;

public interface IRecruitArtificialIntelligenceService
{
    Task<AiReviewResultV1> ReviewVacancyAsync(Vacancy vacancy, CancellationToken cancellationToken);
}

public class RecruitArtificialIntelligenceService(
    ILogger<RecruitArtificialIntelligenceService> logger,
    VacancyAiConfiguration configuration,
    IAzureAiClient azureAiClient,
    ICourseService courseService,
    JsonSerializerOptions jsonOptions) : IRecruitArtificialIntelligenceService
{
    private class TrainingProgrammeSummary
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int? Level { get; set; }
    }
    
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
    
    public class GetStandardsListItem : StandardApiResponseBase
    {
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
    }
    
    private async Task<TrainingProgrammeSummary?> GetTrainingProgrammeById(string programmeId)
    {
        var standards = await courseService.GetActiveStandards<GetStandardsListResponse>("ActiveStandards");
        var allTrainingProgrammes = standards.Standards?.Select(item => new TrainingProgrammeSummary
        {
            Id = item. LarsCode.ToString(),
            Title = item.Title,
            Level = item.Level
        }).ToList() ?? [];
    
        return allTrainingProgrammes.FirstOrDefault(c => c.Id.Equals(programmeId, StringComparison.CurrentCultureIgnoreCase));
    }

    public async Task<AiReviewResultV1> ReviewVacancyAsync(Vacancy vacancy, CancellationToken cancellationToken)
    {
        var programme = await GetTrainingProgrammeById(vacancy.ProgrammeId);
        if (programme is null)
        {
            logger.LogError("Failed to locate programme '{ProgrammeId}' for vacancy '{VacancyId}' when trying to perform AI review", vacancy.ProgrammeId, vacancy.Id);
            return new AiReviewResultV1(); // will force manual review
        }
        
        var skills = JsonSerializer.Serialize(vacancy.Skills ?? [], jsonOptions);
        var qualifications = JsonSerializer.Serialize(vacancy.Qualifications ?? [], jsonOptions);

        var spellcheckFields = new Dictionary<string, string>
        {
            ["AdditionalTrainingDescription"] = vacancy.AdditionalTrainingDescription,
            ["Description"] = vacancy.Description,
            ["EmployerDescription"] = vacancy.EmployerDescription,
            ["Qualifications"] = qualifications,
            ["ShortDescription"] = vacancy.ShortDescription,
            ["Skills"] = skills,
            ["Title"] = vacancy.Title,
            ["TrainingDescription"] = vacancy.TrainingDescription,
        };

        var fieldsToCheck = new Dictionary<string, string>(spellcheckFields)
        {
            ["TrainingProgrammeTitle"] = programme.Title,
            ["TrainingProgrammeLevel"] = $"Level {programme.Level}",
            ["ThingsToConsider"] = vacancy.ThingsToConsider,
        };

        var spellcheckPrompt = new AzureAiClientPrompt(configuration.SpellingCheckPrompt.SystemPrompt, configuration.SpellingCheckPrompt.UserHeader, configuration.SpellingCheckPrompt.UserInstruction);
        var discriminationPrompt = new AzureAiClientPrompt(configuration.DiscriminationPrompt.SystemPrompt, configuration.DiscriminationPrompt.UserHeader, configuration.DiscriminationPrompt.UserInstruction);
        var contentEvaluationPrompt = new AzureAiClientPrompt(configuration.MissingContentPrompt.SystemPrompt, configuration.MissingContentPrompt.UserHeader, configuration.MissingContentPrompt.UserInstruction);
        
        var spellcheckTask = azureAiClient.PerformCheckAsync<Dictionary<string, int>>(spellcheckPrompt, spellcheckFields, cancellationToken);
        var discriminationTask = azureAiClient.PerformCheckAsync<Dictionary<string, string>>(discriminationPrompt, fieldsToCheck, cancellationToken);
        var contentEvaluationTask = azureAiClient.PerformCheckAsync<Dictionary<string, string>>(contentEvaluationPrompt, fieldsToCheck, cancellationToken);
        
        await Task.WhenAll(spellcheckTask, discriminationTask, contentEvaluationTask);

        return new AiReviewResultV1
        {
            SpellcheckResult = spellcheckTask.Result,
            DiscriminationResult = discriminationTask.Result,
            ContentEvaluationResult = contentEvaluationTask.Result,
        };
    }
}