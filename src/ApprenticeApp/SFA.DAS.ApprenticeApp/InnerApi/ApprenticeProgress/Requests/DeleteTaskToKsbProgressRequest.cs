using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class DeleteTaskToKsbProgressRequest : IDeleteApiRequest
    {
        public long ApprenticeshipId;
        public int KsbProgressId;
        public int TaskId;

        public DeleteTaskToKsbProgressRequest(long apprenticeshipId, int taskId, int ksbProgressId)
        {
            KsbProgressId = ksbProgressId;
            TaskId = taskId;
            ApprenticeshipId = apprenticeshipId;
        }

        public string DeleteUrl => $"apprenticeships/{ApprenticeshipId}/ksbs/{KsbProgressId}/taskid/{TaskId}";
    }
}