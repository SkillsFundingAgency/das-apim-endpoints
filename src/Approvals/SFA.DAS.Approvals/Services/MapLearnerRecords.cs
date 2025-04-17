using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Learners.Queries;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services;

public interface IMapLearnerRecords
{
    Task<List<LearnerSummary>> Map(IEnumerable<LearnerDataRecord> learner);
}

public class MapLearnerRecords(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ILogger<IMapLearnerRecords> logger) : IMapLearnerRecords
{
    public async Task<List<LearnerSummary>> Map(IEnumerable<LearnerDataRecord> learners)
    {
        logger.LogInformation("Getting all Courses, to match with Learner Records");
        var learnerSummaries = new List<LearnerSummary>();
        var standards = await coursesApiClient.Get<GetStandardsListResponse>(new GetStandardsExportRequest());

        var list = standards.Standards.ToList();

        logger.LogInformation("Mapping Learner record to summary");
        foreach (var learner in learners)
        {
            learnerSummaries.Add(new LearnerSummary
            {
                Id = learner.Id,
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                Uln = learner.Uln,
                Course = list.FirstOrDefault(x => x.LarsCode == learner.StandardCode)?.Title
            });
        }

        return learnerSummaries;
    }

}