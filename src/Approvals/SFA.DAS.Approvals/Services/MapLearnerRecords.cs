using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Learners.Queries;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Services;

public interface IMapLearnerRecords
{
    Task<List<LearnerSummary>> Map(IEnumerable<LearnerDataRecord> data, List<GetAllStandardsResponse.TrainingProgramme> trainingProgrammes);
}

public class MapLearnerRecords(ILogger<IMapLearnerRecords> logger) : IMapLearnerRecords
{
    public async Task<List<LearnerSummary>> Map(IEnumerable<LearnerDataRecord> learners, List<GetAllStandardsResponse.TrainingProgramme> list)
    {
        logger.LogInformation("Getting all Courses, to match with Learner Records");
        var learnerSummaries = new List<LearnerSummary>();

        logger.LogInformation("Mapping Learner record to summary");
        foreach (var learner in learners)
        {
            learnerSummaries.Add(new LearnerSummary
            {
                Id = learner.Id,
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                Uln = learner.Uln,
                Course = list.FirstOrDefault(x => x.CourseCode == learner.StandardCode.ToString())?.Name,
                StartDate = learner.StartDate
            });
        }

        return learnerSummaries;
    }

}