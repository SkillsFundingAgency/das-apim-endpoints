using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Learners.Queries;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Services;

public interface IMapLearnerRecords
{
    Task<List<LearnerSummary>> Map(IEnumerable<LearnerDataRecord> learner, List<GetStandardsListItem> list);
}

public class MapLearnerRecords(ILogger<IMapLearnerRecords> logger) : IMapLearnerRecords
{
    public async Task<List<LearnerSummary>> Map(IEnumerable<LearnerDataRecord> learners, List<GetStandardsListItem> list)
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
                Course = list.FirstOrDefault(x => x.LarsCode == learner.StandardCode)?.Title
            });
        }

        return learnerSummaries;
    }

}