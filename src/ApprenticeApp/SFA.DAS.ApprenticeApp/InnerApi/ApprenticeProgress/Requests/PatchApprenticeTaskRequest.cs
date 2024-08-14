using System;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class PatchApprenticeTaskRequest : IPostApiRequest
    {
        private readonly long _apprenticeshipId;
        public int _taskId;
        public string PostUrl => $"/apprenticeships/{_apprenticeshipId}/tasks/{_taskId}";
        public object Data { get; set; }

        public PatchApprenticeTaskRequest(long apprenticeshipId, int taskId, ApprenticeTaskData data)
        {
            _apprenticeshipId = apprenticeshipId;
            _taskId = taskId;
            Data = data;
        }
    }
}
