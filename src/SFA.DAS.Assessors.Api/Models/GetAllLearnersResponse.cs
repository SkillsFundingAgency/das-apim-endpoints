using SFA.DAS.Assessors.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Assessors.Api.Models
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
