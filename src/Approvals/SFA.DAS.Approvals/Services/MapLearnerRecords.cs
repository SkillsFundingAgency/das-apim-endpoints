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
    Task<List<LearnerSummary>> Map(List<LearnerDataRecord> data, List<GetAllStandardsResponse.TrainingProgramme> trainingProgrammes);
}

public class MapLearnerRecords(ILogger<IMapLearnerRecords> logger) : IMapLearnerRecords
{
    public async Task<List<LearnerSummary>> Map(List<LearnerDataRecord> learners, List<GetAllStandardsResponse.TrainingProgramme> trainingProgrammes)
    {
        logger.LogInformation("Mapping Learner record to summary");

        return learners.ConvertAll(learner =>
            {
                var courseName = learner.TrainingName;

                if (string.IsNullOrEmpty(courseName))
                {
                    var matchingProgramme = trainingProgrammes.FirstOrDefault(p => p.CourseCode == learner.StandardCode.ToString());

                    courseName = matchingProgramme?.Name;
                }

                return new LearnerSummary
                {
                    Id = learner.Id,
                    FirstName = learner.FirstName,
                    LastName = learner.LastName,
                    Uln = learner.Uln,
                    Course = courseName,
                    StartDate = learner.StartDate
                };
            });
    }

}