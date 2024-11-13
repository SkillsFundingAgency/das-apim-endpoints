using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Tasks
{
    public class GetTaskRemindersByApprenticeshipIdQueryResult
    {
        public ApprenticeTaskRemindersCollection TaskReminders { get; set; }
    }
}
