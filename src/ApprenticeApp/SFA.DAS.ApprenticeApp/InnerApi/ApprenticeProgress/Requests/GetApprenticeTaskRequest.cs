using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class GetApprenticeTaskRequest : IGetApiRequest
    {
        public Guid ApprenticeshipId;
        public int TaskId;        

        public GetApprenticeTaskRequest(Guid apprenticeshipId, int taskId)
        {
            ApprenticeshipId = apprenticeshipId;
            TaskId = taskId;
        }

        public string GetUrl => $"apprenticeships/{ApprenticeshipId}/tasks/{TaskId}/";
    }
}
