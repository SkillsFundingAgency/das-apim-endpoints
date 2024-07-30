using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticeTask
    {
        public int TaskId { get; set; }
        public Guid ApprenticeshipId { get; set; }
        public DateTime? DueDate { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int? ApprenticeshipCategoryId { get; set; } = null!;
        public string Note { get; set; } = null!;
        public DateTime? CompletionDateTime { get; set; } = null!;
        public DateTime? CreatedDateTime { get; set; } = null!;
        public TaskStatus? Status { get; set; } = null!;

        public List<TaskFile>? TaskFiles { get; set; } = null!;
        public List<TaskReminder>? TaskReminders { get; set; } = null!;
        public List<TaskKSBs>? TaskLinkedKsbs { get; set; } = null!;
        public List<ApprenticeshipCategory>? ApprenticeshipCategory { get; set; } = null!;
    }

    public class ApprenticeshipCategory
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
    }

    public class TaskKSBs
    {
        public int TaskId { get; set; }
        public int? KSBProgressId { get; set; }
    }

    public class TaskFile
    {
        public int? TaskId { get; set; }
        public int? TaskFileId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileContents { get; set; }
    }

    public class TaskReminder
    {
        public int TaskId { get; set; }
        public int? ReminderId { get; set; }
        public int? ReminderValue { get; set; }
        public ReminderUnit? ReminderUnit { get; set; }
        public ReminderStatus? Status { get; set; }
    }

    [Flags]
    public enum ReminderUnit
    {
        Minutes = 0,
        Hours = 1,
        Days = 2
    }

    [Flags]
    public enum ReminderStatus
    {
        NotSent = 0,
        Sent = 1,
        Dismissed = 2
    }


    [Flags]
    public enum TaskStatus
    {
        Todo = 0,
        Done = 1
    }

    public class ApprenticeTasksCollection
    {
        public List<ApprenticeTask> Tasks { get; set; }
    }
}