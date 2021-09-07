using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetAllLearnersResponse
    {
        public List<Learner> Learners { get; }

        public int BatchNumber { get; }

        public int BatchSize { get; }

        public int TotalNumberOfBatches { get; }

        public GetAllLearnersResponse(List<Learner> learners, int batchNumber, int batchSize, int totalNumberOfBatches)
        {
            Learners = learners;
            BatchNumber = batchNumber;
            BatchSize = batchSize;
            TotalNumberOfBatches = totalNumberOfBatches;
        }
    }
}
