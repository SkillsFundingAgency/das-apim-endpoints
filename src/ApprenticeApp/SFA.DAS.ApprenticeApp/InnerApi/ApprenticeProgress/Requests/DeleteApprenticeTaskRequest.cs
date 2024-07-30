using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class DeleteApprenticeTaskRequest : IDeleteApiRequest
    {
        public Guid ApprenticeshipId;
        public int TaskId;

        public DeleteApprenticeTaskRequest(Guid apprenticeshipId, int taskId)
        {
            ApprenticeshipId = apprenticeshipId;
            TaskId = taskId;
        }

        public string DeleteUrl => $"apprenticeships/{ApprenticeshipId}/tasks/{TaskId}/";
    }
}