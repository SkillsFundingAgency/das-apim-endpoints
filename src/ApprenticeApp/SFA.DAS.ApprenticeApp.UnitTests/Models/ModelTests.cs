using NUnit.Framework;
using NUnit.Framework.Legacy;
using SFA.DAS.ApprenticeApp.Models;
using System;

namespace SFA.DAS.ApprenticeApp.UnitTests.InnerApi.ApprenticeAccounts.Requests
{
    public class ModelTests
    {
        [Test]
        public void ApprenticeTask_model_test()
        {
            var sut = new ApprenticeTask
            {
                TaskId = 1,
                ApprenticeshipId = 1,
                ApprenticeAccountId = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),
                DueDate = new DateTime(2019, 05, 09),
                Title = "title",
                ApprenticeshipCategoryId = 0,
                Note = "note",
                CompletionDateTime = new DateTime(2019, 05, 09),
                CreatedDateTime = new DateTime(2019, 05, 09),
                ApprenticeshipCategory = new System.Collections.Generic.List<ApprenticeshipCategory>(),
                Status = TaskStatus.Done,
            };

            ClassicAssert.AreEqual(1, sut.TaskId);
            ClassicAssert.AreEqual(1, sut.ApprenticeshipId);
            ClassicAssert.AreEqual(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), sut.ApprenticeAccountId);
            ClassicAssert.AreEqual(new DateTime(2019, 05, 09), sut.DueDate);
            ClassicAssert.AreEqual("title", sut.Title);
            ClassicAssert.AreEqual(0, sut.ApprenticeshipCategoryId);
            ClassicAssert.AreEqual("note", sut.Note);
            ClassicAssert.AreEqual(new DateTime(2019, 05, 09), sut.CompletionDateTime);
            ClassicAssert.AreEqual(new DateTime(2019, 05, 09), sut.CreatedDateTime);
            ClassicAssert.AreEqual(TaskStatus.Done, sut.Status);
        }

        [Test]
        public void ApprenticeshipCategory_model_test()
        {
            var sut = new ApprenticeshipCategory
            {
                CategoryId = 1,
                Title = "title"
            };

            ClassicAssert.AreEqual(1, sut.CategoryId);
            ClassicAssert.AreEqual("title", sut.Title);
        }

        [Test]
        public void TaskKSBs_model_test()
        {
            var sut = new TaskKSBs
            {
                TaskId = 1,
                KSBProgressId = 2
            };

            ClassicAssert.AreEqual(1, sut.TaskId);
            ClassicAssert.AreEqual(2, sut.KSBProgressId);
        }

        [Test]
        public void TaskFile_model_test()
        {
            var sut = new TaskFile
            {
                TaskId = 1,
                TaskFileId = 2,
                FileType = "type",
                FileName = "name",
                FileContents = "contents"
            };

            ClassicAssert.AreEqual(1, sut.TaskId);
            ClassicAssert.AreEqual(2, sut.TaskFileId);
            ClassicAssert.AreEqual("type", sut.FileType);
            ClassicAssert.AreEqual("name", sut.FileName);
            ClassicAssert.AreEqual("contents", sut.FileContents);
        }


        [Test]
        public void TaskReminder_model_test()
        {
            var sut = new TaskReminder
            {
                TaskId = 1,
                ReminderId = 2,
                ReminderValue = 1,
                ReminderUnit = ReminderUnit.Days,
                Status = ReminderStatus.Dismissed
            };

            ClassicAssert.AreEqual(1, sut.TaskId);
            ClassicAssert.AreEqual(2, sut.ReminderId);
            ClassicAssert.AreEqual(1, sut.ReminderValue);
            ClassicAssert.AreEqual(ReminderUnit.Days, sut.ReminderUnit);
            ClassicAssert.AreEqual(ReminderStatus.Dismissed, sut.Status);
        }

        [Test]
        public void ApprenticeTaskData_model_test()
        {
            var sut = new ApprenticeTaskData
            {
                TaskId = 1,
                ApprenticeshipId = 1,
                DueDate = new DateTime(2019, 05, 09),
                Title = "title",
                ApprenticeshipCategoryId = 0,
                Note = "note",
                CompletionDateTime = new DateTime(2019, 05, 09),
                CreatedDateTime = new DateTime(2019, 05, 09),
                Status = 1,
                CategoryId = 1
            };

            ClassicAssert.AreEqual(1, sut.TaskId);
            ClassicAssert.AreEqual(1, sut.ApprenticeshipId);
            ClassicAssert.AreEqual(new DateTime(2019, 05, 09), sut.DueDate);
            ClassicAssert.AreEqual("title", sut.Title);
            ClassicAssert.AreEqual(0, sut.ApprenticeshipCategoryId);
            ClassicAssert.AreEqual("note", sut.Note);
            ClassicAssert.AreEqual(new DateTime(2019, 05, 09), sut.CompletionDateTime);
            ClassicAssert.AreEqual(new DateTime(2019, 05, 09), sut.CreatedDateTime);
            ClassicAssert.AreEqual(1, sut.Status);
            ClassicAssert.AreEqual(1, sut.CategoryId);

        }

        [Test]
        public void ApprenticeTaskCategories_model_test()
        {
            var sut = new ApprenticeTaskCategories
            {
                CategoryId = 1,
                Title = "title"
            };

            ClassicAssert.AreEqual(1, sut.CategoryId);
            ClassicAssert.AreEqual("title", sut.Title);

        }

        [Test]
        public void Ksb_model_test()
        {
            var sut = new Ksb
            {
                Type = KsbType.Behaviour,
                Id = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),
                Key = "key",
                Detail = "detail"
            };

            ClassicAssert.AreEqual(KsbType.Behaviour, sut.Type);
            ClassicAssert.AreEqual(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), sut.Id);
            ClassicAssert.AreEqual("key", sut.Key);
            ClassicAssert.AreEqual("detail", sut.Detail);
        }
    }
}
