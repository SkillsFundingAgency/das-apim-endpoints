using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Tasks
{
    public class GetTaskRemindersByApprenticeshipIdQueryResult
    {
        public ApprenticeTaskReminderCollection TaskReminders { get; set; }
    }
}
