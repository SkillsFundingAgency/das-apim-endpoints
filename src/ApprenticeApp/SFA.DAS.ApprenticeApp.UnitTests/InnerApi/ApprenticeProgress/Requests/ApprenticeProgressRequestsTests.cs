using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.UnitTests.InnerApi.ApprenticeAccounts.Requests
{
    public class ApprenticeProgressRequestsTests
    {
        [Test, AutoData]
        public void PatchApprenticeTaskRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeshipId = 1;
            var taskId = 1;
            ApprenticeTaskData data = new ApprenticeTaskData();

            var instance = new PatchApprenticeTaskRequest(apprenticeshipId, 1, data);

            instance.PostUrl.Should().Be($"/apprenticeships/1/tasks/1");
        }


        [Test, AutoData]
        public void GetTaskCategoriesRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeshipId = 1;

            var instance = new GetTaskCategoriesRequest(apprenticeshipId);

            instance.GetUrl.Should().Be($"apprenticeships/1/taskCategories");
        }

        [Test, AutoData]
        public void GetApprenticeTaskRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeshipId = 1;
            var taskId = 1;

            var instance = new GetApprenticeTaskRequest(apprenticeshipId, taskId);

            instance.GetUrl.Should().Be($"apprenticeships/1/tasks/1/");
        }

        [Test, AutoData]
        public void GetApprenticeTasksRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeshipId = 1;
            var statusId = 1;
            var fromDate = "1-1-2020";
            var toDate = "1-1-2030";
            var instance = new GetApprenticeTasksRequest(apprenticeshipId, statusId, fromDate, toDate);
            instance.GetUrl.Should().Be($"apprenticeships/1/fromDate/1-1-2020/toDate/1-1-2030/status/1");
        }


        [Test, AutoData]
        public void DeleteApprenticeTaskRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeshipId = 1;
            var taskId = 1;
            var instance = new DeleteApprenticeTaskRequest(apprenticeshipId, taskId);
            instance.DeleteUrl.Should().Be($"apprenticeships/1/tasks/1/");
        }


        [Test, AutoData]
        public void DeleteTaskToKsbProgressRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeshipId = 1;
            var taskId = 1;
            var ksbProgressId = 2;
            var instance = new DeleteTaskToKsbProgressRequest(apprenticeshipId, taskId, ksbProgressId);
            instance.DeleteUrl.Should().Be($"apprenticeships/1/ksbs/2/taskid/1");
        }

        [Test, AutoData]
        public void PostApprenticeTaskRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeshipId = 1;
            var instance = new PostApprenticeTaskRequest(apprenticeshipId, new ApprenticeTaskData());
            instance.PostUrl.Should().Be($"/apprenticeships/1/tasks");
        }

        [Test, AutoData]
        public void PostKsbProgressRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeshipId = 1;
            var instance = new PostKsbProgressRequest(apprenticeshipId, new ApprenticeKsbProgressData());
            instance.PostUrl.Should().Be($"/apprenticeships/1/ksbs");
        }
    }
}