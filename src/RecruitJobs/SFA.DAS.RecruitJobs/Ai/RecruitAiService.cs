using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RecruitAi;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

namespace SFA.DAS.RecruitJobs.Ai;

public interface IRecruitAiService
{
    Task<bool> ReviewVacancyAsync(Guid vacancyReviewId, Vacancy vacancy, CancellationToken cancellationToken);
}

public class RecruitAiService(
    ILogger<RecruitAiService> logger,
    ICourseService courseService,
    IRecruitAiApiClient<RecruitAiApiConfiguration> recruitAiApiClient) : IRecruitAiService
{
    internal class TrainingProgrammeSummary
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
            Id = item.LarsCode.ToString(),
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
        
        var payload = new PostVacancyReviewDto(
            vacancy.Id,
            vacancy.Title,
            vacancy.ShortDescription,
            vacancy.Description,
            vacancy.EmployerDescription,
            vacancy.ThingsToConsider,
            vacancy.TrainingDescription,
            vacancy.AdditionalTrainingDescription,
            programme.Title,
            $"Level {programme.Level}",
            vacancy.OutcomeDescription,
            vacancy.ApplicationInstructions,
            vacancy.AdditionalQuestion1,
            vacancy.AdditionalQuestion2,
            vacancy.Wage!.WageAdditionalInformation,
            vacancy.Wage.CompanyBenefitsInformation,
            vacancy.Wage.WorkingWeekDescription
        );

        var reviewResponse = await recruitAiApiClient.PostWithResponseCode<NullResponse>(new PostVacancyReviewRequest(vacancyReviewId, payload), false);
        return reviewResponse.StatusCode.IsSuccessStatusCode();
    }
}