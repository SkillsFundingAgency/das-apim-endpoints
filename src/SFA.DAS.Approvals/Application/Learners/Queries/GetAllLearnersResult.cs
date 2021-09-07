using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Learners.Queries
{
    public class GetAllLearnersResult
    {
        public List<Learner> Learners { get; }

        public int BatchNumber { get; }

        public int BatchSize { get; }

        public int TotalNumberOfBatches { get; }

        public GetAllLearnersResult(List<Learner> learners, int batchNumber, int batchSize, int totalNumberOfBatches)
        {
            Learners = learners;
            BatchNumber = batchNumber;
            BatchSize = batchSize;
            TotalNumberOfBatches = totalNumberOfBatches;
        }
    }
}