using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class PatchApprenticeTaskReminderStatusRequest : IPostApiRequest
    {
        private readonly long _apprenticeshipId;
        public int _taskId;
        public string PostUrl => $"/apprenticeships/{_apprenticeshipId}/updatetaskreminders/{_taskId}/reminderstatus/{_statusId}/";
        public int _statusId { get; set; }
        public object Data { get; set; }

        public PatchApprenticeTaskReminderStatusRequest(long apprenticeshipId, int taskId, int statusId)
        {
            _apprenticeshipId = apprenticeshipId;
            _taskId = taskId;
            _statusId = statusId;
        }
    }
}
