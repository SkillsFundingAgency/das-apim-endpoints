using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class PatchApprenticeTaskStatusRequest : IPostApiRequest
    {
        private readonly long _apprenticeshipId;
        public int _taskId;
        public string PostUrl => $"/apprenticeships/{_apprenticeshipId}/tasks/{_taskId}/changestatus/{_statusId}/";
        public int _statusId { get; set; }
        public object Data { get; set; }

        public PatchApprenticeTaskStatusRequest(long apprenticeshipId, int taskId, int statusId, ApprenticeTaskData data)
        {
            _apprenticeshipId = apprenticeshipId;
            _taskId = taskId;
            _statusId = statusId;
            Data = data;
        }
    }
}