using System.Collections.Generic;
using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Application.Queries.GetAllLearners
{
    public class GetAllLearnersResult
    {
        public List<Learner> Learners { get; set;  }

        public int BatchNumber { get; set; }

        public int BatchSize { get; set; }

        public int TotalNumberOfBatches { get; set; }

        public GetAllLearnersResult(GetAllLearnersResponse response)
        {
            Learners = response.Learners;
            BatchNumber = response.BatchNumber;
            BatchSize = response.BatchSize;
            TotalNumberOfBatches = response.TotalNumberOfBatches;
        }
    }
}