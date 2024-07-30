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
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetTaskCategories(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_Tasks_Tests(
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var dateFrom = new DateTime();
            var dateTo = new DateTime();
            int status = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetTasks(apprenticeId, status, dateFrom, dateTo);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_TaskBy_Id_Test(
             [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            int taskId = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetTaskById(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Update_Task_By_Id_Test(
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var ApprenticeTaskData = new ApprenticeTaskData();
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.UpdateTaskById(apprenticeId, taskId, ApprenticeTaskData);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Delete_TaskBy_Id_Tests(
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var ApprenticeTaskData = new ApprenticeTaskData();
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.DeleteTaskById(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }
    }
}