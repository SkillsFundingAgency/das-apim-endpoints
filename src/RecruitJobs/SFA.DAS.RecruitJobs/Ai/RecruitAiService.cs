using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RecruitAi;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.Ai;

public interface IRecruitAiService
{
    Task<bool> ReviewVacancyAsync(Guid vacancyReviewId, Vacancy vacancy, CancellationToken cancellationToken);
}

public class RecruitAiService(
    ILogger<RecruitAiService> logger,
    ICourseService courseService,
    IRecruitAiApiClient<RecruitAiApiConfiguration> recruitAiApiClient,
    JsonSerializerOptions jsonOptions) : IRecruitAiService
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

    public async Task<bool> ReviewVacancyAsync(Guid vacancyReviewId, Vacancy vacancy, CancellationToken cancellationToken)
    {
        var programme = await GetTrainingProgrammeById(vacancy.ProgrammeId);
        if (programme is null)
        {
            logger.LogError("Failed to locate programme '{ProgrammeId}' for vacancy '{VacancyId}' when trying to perform AI review", vacancy.ProgrammeId, vacancy.Id);
            return false;
        }
        
        var skills = JsonSerializer.Serialize(vacancy.Skills ?? [], jsonOptions);
        var qualifications = JsonSerializer.Serialize(vacancy.Qualifications ?? [], jsonOptions);
        
        var payload = new PostVacancyReviewDto(
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
            $"Level {programme.Level}");

        var reviewResponse = await recruitAiApiClient.PostWithResponseCode<NullResponse>(new PostVacancyReviewRequest(vacancyReviewId, payload), false);
        return reviewResponse.StatusCode.IsSuccessStatusCode();
    }
}