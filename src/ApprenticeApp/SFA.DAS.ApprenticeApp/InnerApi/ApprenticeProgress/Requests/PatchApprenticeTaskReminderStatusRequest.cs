using SFA.DAS.ApprenticeApp.Application.Commands.Tasks;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class PatchApprenticeTaskReminderRequest : IPostApiRequest
    {
        public int _taskId { get; set; }
        public int _statusId { get; set; }

        public string PostUrl => $"updatetaskreminders/tasks/{_taskId}/status/{_statusId}";
        public object Data { get; set; }

        public PatchApprenticeTaskReminderRequest(int taskId, int statusId)
        {
            _taskId = taskId;
            _statusId = statusId;
        }
    }
}
