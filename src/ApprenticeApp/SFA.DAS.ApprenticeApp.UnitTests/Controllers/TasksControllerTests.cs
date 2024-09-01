using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.Testing.AutoFixture;
using StackExchange.Redis;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class TasksControllerTests
    {

        [Test, MoqAutoData]
        public async Task Get_Task_NoCategories_Tests(
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
        public async Task Get_Task_Categories_Tests(
              [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTaskCategoriesQueryResult categoriesResult,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(x => x.Send(It.IsAny<GetTaskCategoriesQuery>(), default)).ReturnsAsync(categoriesResult);
            var result = await controller.GetTaskCategories(apprenticeshipId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_Tasks_NoTasks_Tests(
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
        public async Task Get_Tasks_Tests(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTasksByApprenticeshipIdQueryResult tasksResult,
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

            mediator.Setup(x => x.Send(It.IsAny<GetTasksByApprenticeshipIdQuery>(), default)).ReturnsAsync(tasksResult);
            var result = await controller.GetTasks(apprenticeshipId, status, dateFrom, dateTo);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_TaskBy_Id_No_Task(
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
        public async Task Get_TaskBy_Id_Test(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTaskByTaskIdQueryResult taskResult,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            int taskId = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(x => x.Send(It.IsAny<GetTaskByTaskIdQuery>(), default)).ReturnsAsync(taskResult);
            taskResult.Tasks = new ApprenticeTasksCollection() { Tasks = new System.Collections.Generic.List<ApprenticeTask>() };
            var result = await controller.GetTaskById(apprenticeshipId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
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

        [Test, MoqAutoData]
        public async Task Get_task_viewdata_no_task(
             [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetTaskViewData(apprenticeshipId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        
        [Test, MoqAutoData]
        public async Task Get_task_viewdata_no_categories(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTaskByTaskIdQueryResult taskResult,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(x => x.Send(It.IsAny<GetTaskByTaskIdQuery>(), default)).ReturnsAsync(taskResult);
            taskResult.Tasks = new ApprenticeTasksCollection() { Tasks = new System.Collections.Generic.List<ApprenticeTask>() };
            var result = await controller.GetTaskViewData(apprenticeshipId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_task_viewdata(
           [Frozen] Mock<IMediator> mediator,
           [Frozen] GetTaskByTaskIdQueryResult taskResult,
           [Frozen] GetTaskCategoriesQueryResult categoriesResult,
           [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(x => x.Send(It.IsAny<GetTaskByTaskIdQuery>(), default)).ReturnsAsync(taskResult);
            taskResult.Tasks = new ApprenticeTasksCollection() { Tasks = new System.Collections.Generic.List<ApprenticeTask>() };
            mediator.Setup(x => x.Send(It.IsAny<GetTaskCategoriesQuery>(), default)).ReturnsAsync(categoriesResult);
            categoriesResult.TaskCategories = new ApprenticeTaskCategoriesCollection() { TaskCategories = new System.Collections.Generic.List<ApprenticeTaskCategories>() };
            var result = await controller.GetTaskViewData(apprenticeshipId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }


    }
}