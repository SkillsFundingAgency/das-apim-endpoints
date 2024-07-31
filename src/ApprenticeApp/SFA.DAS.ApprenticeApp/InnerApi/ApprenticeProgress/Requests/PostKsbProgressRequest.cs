using System;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class PostKsbProgressRequest : IPostApiRequest
    {
        private readonly Guid _apprenticeshipId;
        public string PostUrl => $"/apprenticeships/{_apprenticeshipId}/ksbs";
        public object Data { get; set; }

        public PostKsbProgressRequest(Guid apprenticeshipId, ApprenticeKsbProgressData data)
        {
            _apprenticeshipId = apprenticeshipId;
            Data = data;
        }
    }
}
