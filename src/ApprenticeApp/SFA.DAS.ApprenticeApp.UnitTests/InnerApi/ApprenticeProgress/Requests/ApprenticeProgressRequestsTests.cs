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
            var apprenticeId = new Guid("8e5482b2-1c77-4143-80a5-ee3ddc751075");
            var taskId = 1;
            ApprenticeTaskData data = new ApprenticeTaskData();

            var instance = new PatchApprenticeTaskRequest(apprenticeId, 1, data);

            instance.PostUrl.Should().Be($"/apprenticeships/8e5482b2-1c77-4143-80a5-ee3ddc751075/tasks/1");
        }


        [Test, AutoData]
        public void GetTaskCategoriesRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeId = new Guid("8e5482b2-1c77-4143-80a5-ee3ddc751075");

            var instance = new GetTaskCategoriesRequest(apprenticeId);

            instance.GetUrl.Should().Be($"apprenticeships/8e5482b2-1c77-4143-80a5-ee3ddc751075/taskCategories");
        }

        [Test, AutoData]
        public void GetApprenticeTaskRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeId = new Guid("8e5482b2-1c77-4143-80a5-ee3ddc751075");
            var taskId = 1;

            var instance = new GetApprenticeTaskRequest(apprenticeId, taskId);

            instance.GetUrl.Should().Be($"apprenticeships/8e5482b2-1c77-4143-80a5-ee3ddc751075/tasks/1/");
        }

        [Test, AutoData]
        public void GetApprenticeTasksRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeId = new Guid("8e5482b2-1c77-4143-80a5-ee3ddc751075");
            var statusId = 1;
            var fromDate = "1-1-2020";
            var toDate = "1-1-2030";
            var instance = new GetApprenticeTasksRequest(apprenticeId, statusId, fromDate, toDate);
            instance.GetUrl.Should().Be($"apprenticeships/8e5482b2-1c77-4143-80a5-ee3ddc751075/fromDate/1-1-2020/toDate/1-1-2030/status/1");
        }


        [Test, AutoData]
        public void DeleteApprenticeTaskRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeId = new Guid("8e5482b2-1c77-4143-80a5-ee3ddc751075");
            var taskId = 1;
            var instance = new DeleteApprenticeTaskRequest(apprenticeId, taskId);
            instance.DeleteUrl.Should().Be($"apprenticeships/8e5482b2-1c77-4143-80a5-ee3ddc751075/tasks/1/");
        }


        [Test, AutoData]
        public void DeleteTaskToKsbProgressRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeId = new Guid("8e5482b2-1c77-4143-80a5-ee3ddc751075");
            var taskId = 1;
            var ksbProgressId = 2;
            var instance = new DeleteTaskToKsbProgressRequest(apprenticeId, taskId, ksbProgressId);
            instance.DeleteUrl.Should().Be($"apprenticeships/8e5482b2-1c77-4143-80a5-ee3ddc751075/ksbs/2/taskid/1");
        }

        [Test, AutoData]
        public void PostApprenticeTaskRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeshipId = new Guid("8e5482b2-1c77-4143-80a5-ee3ddc751075");
            var instance = new PostApprenticeTaskRequest(apprenticeshipId, new ApprenticeTaskData());
            instance.PostUrl.Should().Be($"/apprenticeships/8e5482b2-1c77-4143-80a5-ee3ddc751075/tasks");
        }

        [Test, AutoData]
        public void PostKsbProgressRequestTestUrlIsCorrectlyBuilt()
        {
            var apprenticeshipId = new Guid("8e5482b2-1c77-4143-80a5-ee3ddc751075");
            var instance = new PostKsbProgressRequest(apprenticeshipId, new ApprenticeKsbProgressData());
            instance.PostUrl.Should().Be($"/apprenticeships/8e5482b2-1c77-4143-80a5-ee3ddc751075/ksbs");
        }
    }
}