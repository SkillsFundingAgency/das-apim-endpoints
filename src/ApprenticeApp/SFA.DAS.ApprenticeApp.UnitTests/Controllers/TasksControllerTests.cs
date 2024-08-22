using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class TasksControllerTests
    {

        [Test, MoqAutoData]
        public async Task Get_Task_Categories_Tests(
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetTaskCategories(apprenticeshipId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_Tasks_Tests(
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            var dateFrom = new DateTime();
            var dateTo = new DateTime();
            int status = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetTasks(apprenticeshipId, status, dateFrom, dateTo);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_TaskBy_Id_Test(
             [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            int taskId = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetTaskById(apprenticeshipId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Update_Task_By_Id_Test(
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            var ApprenticeTaskData = new ApprenticeTaskData();
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.UpdateTaskById(apprenticeshipId, taskId, ApprenticeTaskData);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Delete_TaskBy_Id_Tests(
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            var ApprenticeTaskData = new ApprenticeTaskData();
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.DeleteTaskById(apprenticeshipId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }


        [Test, MoqAutoData]
        public async Task Add_task_test(
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            var ApprenticeTaskData = new ApprenticeTaskData();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.AddTask(apprenticeshipId, ApprenticeTaskData);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }


        [Test, MoqAutoData]
        public async Task Update_task_status(
             [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            var ApprenticeTaskData = new ApprenticeTaskData();
            var taskId = 1;
            var statusId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.UpdateTaskStatus(apprenticeshipId, taskId, statusId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }
    }
}