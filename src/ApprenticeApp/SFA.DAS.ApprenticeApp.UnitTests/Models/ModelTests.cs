using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SFA.DAS.ApprenticeApp.Models;

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
                ApprenticeshipId = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),
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
            ClassicAssert.AreEqual(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), sut.ApprenticeshipId);
            ClassicAssert.AreEqual(new DateTime(2019, 05, 09), sut.DueDate);
            ClassicAssert.AreEqual("title", sut.Title);
            ClassicAssert.AreEqual(0, sut.ApprenticeshipCategoryId);
            ClassicAssert.AreEqual("note", sut.Note);
            ClassicAssert.AreEqual(new DateTime(2019, 05, 09), sut.CompletionDateTime);
            ClassicAssert.AreEqual(new DateTime(2019, 05, 09), sut.CreatedDateTime);
            ClassicAssert.AreEqual(TaskStatus.Done, sut.Status);

        }

        [Test]
        public void ApprenticeTaskData_model_test()
        {
            var sut = new ApprenticeTaskData
            {
                TaskId = 1,
                ApprenticeshipId = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),
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
            ClassicAssert.AreEqual(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), sut.ApprenticeshipId);
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
    }
}
