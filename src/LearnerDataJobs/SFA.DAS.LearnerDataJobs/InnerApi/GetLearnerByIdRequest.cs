using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.LearnerDataJobs.InnerApi
{
    public class GetLearnerByIdRequest(long providerId, long learnerDataId) : IGetApiRequest
    {
        public string GetUrl => $"providers/{providerId}/learners/{learnerDataId}";
    }
}
