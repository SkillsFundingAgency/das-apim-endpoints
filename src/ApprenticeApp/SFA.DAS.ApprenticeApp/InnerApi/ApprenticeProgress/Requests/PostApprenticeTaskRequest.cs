using System;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class PostApprenticeTaskRequest : IPostApiRequest<MyApprenticeshipData>
    {
        private readonly Guid _apprenticeId;
        public string PostUrl => $"/apprentices/{_apprenticeId}/myapprenticeship";
        public MyApprenticeshipData Data { get; set; }

        public PostApprenticeTaskRequest(Guid apprenticeId)
        {
            _apprenticeId = apprenticeId;
        }
    }
}
