using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticeTask
    {
        public int TaskId { get; set; }
        public Guid ApprenticeshipId { get; set; }
        public DateTime? DueDate { get; set; }
        public string Title { get; set; }
        public int? ApprenticeshipCategoryId { get; set; }
        public string Note { get; set; }
        public DateTime? CompletionDateTime { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public TaskStatus? Status { get; set; }

        public List<TaskFile>? TaskFiles { get; set; }
        public List<TaskReminder>? TaskReminders { get; set; }
        public List<TaskKSBs>? TaskLinkedKsbs { get; set; }
        public List<ApprenticeshipCategory>? ApprenticeshipCategory { get; set; }


    }

    public class ApprenticeshipCategory
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
    }

    public class TaskFile
    {
        public int TaskId { get; set; }
        public int? TaskFileId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public byte[] FileContents { get; set; }
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

    public class TaskKSBs
    {
        public int TaskId { get; set; }
        public int? KSBProgressId { get; set; }
    }


    public class ApprenticeTasksCollection
    {
        public List<ApprenticeTask> Tasks { get; set; }
    }
}