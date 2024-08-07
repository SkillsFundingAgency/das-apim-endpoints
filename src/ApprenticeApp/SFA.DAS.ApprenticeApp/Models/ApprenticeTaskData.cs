using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticeTaskData
    {
        public Guid ApprenticeshipId { get; set; }
        public int? TaskId { get; set; }
        public DateTime? DueDate { get; set; }
        public string Title { get; set; }
        public int? ApprenticeshipCategoryId { get; set; }
        public string Note { get; set; }
        public DateTime? CompletionDateTime { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CategoryId { get; set; }
        public int? Status { get; set; }

        // files block
        public List<TaskFile> Files { get; set; }

        // reminder block
        public int? ReminderUnit { get; set; }
        public int? ReminderValue { get; set; }
        public int? ReminderStatus { get; set; }

        // ksbs linked
        public string[] KsbsLinked { get; set; }
    }
}
