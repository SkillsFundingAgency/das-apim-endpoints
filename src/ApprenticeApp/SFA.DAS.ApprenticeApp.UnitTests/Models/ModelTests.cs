using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
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

            Assert.Equals(1, sut.TaskId);
            Assert.Equals(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), sut.ApprenticeshipId);
            Assert.Equals(new DateTime(2019, 05, 09), sut.DueDate);
            Assert.Equals("title", sut.Title);
            Assert.Equals(0, sut.ApprenticeshipCategoryId);
            Assert.Equals("note", sut.Note);
            Assert.Equals(new DateTime(2019, 05, 09), sut.CompletionDateTime);
            Assert.Equals(new DateTime(2019, 05, 09), sut.CreatedDateTime);
            Assert.Equals(TaskStatus.Done, sut.Status);

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

            Assert.Equals(1, sut.TaskId);
            Assert.Equals(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), sut.ApprenticeshipId);
            Assert.Equals(new DateTime(2019, 05, 09), sut.DueDate);
            Assert.Equals("title", sut.Title);
            Assert.Equals(0, sut.ApprenticeshipCategoryId);
            Assert.Equals("note", sut.Note);
            Assert.Equals(new DateTime(2019, 05, 09), sut.CompletionDateTime);
            Assert.Equals(new DateTime(2019, 05, 09), sut.CreatedDateTime);
            Assert.Equals(1, sut.Status);
            Assert.Equals(1, sut.CategoryId);

        }

        [Test]
        public void ApprenticeTaskCategories_model_test()
        {
            var sut = new ApprenticeTaskCategories
            {
                CategoryId = 1,
                Title = "title"
            };

            Assert.Equals(1, sut.CategoryId);
            Assert.Equals("title", sut.Title);

        }
    }
}
