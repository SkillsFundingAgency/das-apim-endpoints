using System.Collections.Generic;

namespace SFA.DAS.Assessors.InnerApi.Responses
{
    public class GetAllLearnersResponse
    {
        public List<Learner> Learners { get; set; }

        public int BatchNumber { get; set; }

        public int BatchSize { get; set; }

        public int TotalNumberOfBatches { get; set; }
    }
}
