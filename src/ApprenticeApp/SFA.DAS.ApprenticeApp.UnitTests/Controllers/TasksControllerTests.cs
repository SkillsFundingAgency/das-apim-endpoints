using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.ApprenticeApp.Application.Queries.Tasks;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;
using Apprentice = SFA.DAS.ApprenticeApp.Models.Apprentice;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class TasksControllerTests
    {

        [Test, MoqAutoData]
        public async Task Get_Task_NoCategories_Tests(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            var result = await controller.GetTaskCategories(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_Task_Categories_Tests(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTaskCategoriesQueryResult categoriesResult,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            mediator.Setup(x => x.Send(It.IsAny<GetTaskCategoriesQuery>(), default)).ReturnsAsync(categoriesResult);
            var result = await controller.GetTaskCategories(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_Task_Categories_NoApprenticeship_Test(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTaskCategoriesQueryResult categoriesResult,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });
            mediator.Setup(x => x.Send(It.IsAny<GetTaskCategoriesQuery>(), default)).ReturnsAsync(categoriesResult);
            var result = await controller.GetTaskCategories(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Get_Tasks_NoTasks_Tests(
            [Frozen] Mock<IMediator> mediator,
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
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            var result = await controller.GetTasks(apprenticeId, status, dateFrom, dateTo);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }


        [Test, MoqAutoData]
        public async Task Get_Tasks_Tests(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTasksByApprenticeshipIdQueryResult tasksResult,
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
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            mediator.Setup(x => x.Send(It.IsAny<GetTasksByApprenticeshipIdQuery>(), default)).ReturnsAsync(tasksResult);
            var result = await controller.GetTasks(apprenticeId, status, dateFrom, dateTo);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_Tasks_NoApprenticeship_Test(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTasksByApprenticeshipIdQueryResult tasksResult,
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
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });
            mediator.Setup(x => x.Send(It.IsAny<GetTasksByApprenticeshipIdQuery>(), default)).ReturnsAsync(tasksResult);
            var result = await controller.GetTasks(apprenticeId, status, dateFrom, dateTo);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Get_TaskBy_Id_No_Task(
            [Frozen] Mock<IMediator> mediator,
             [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            int taskId = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            var result = await controller.GetTaskById(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_TaskBy_Id_Test(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTaskByTaskIdQueryResult taskResult,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            int taskId = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            mediator.Setup(x => x.Send(It.IsAny<GetTaskByTaskIdQuery>(), default)).ReturnsAsync(taskResult);
            taskResult.Tasks = new ApprenticeTasksCollection() { Tasks = new System.Collections.Generic.List<ApprenticeTask>() };
            var result = await controller.GetTaskById(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_TaskBy_Id_NoApprenticeship_Test(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTaskByTaskIdQueryResult taskResult,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            int taskId = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });
            mediator.Setup(x => x.Send(It.IsAny<GetTaskByTaskIdQuery>(), default)).ReturnsAsync(taskResult);
            taskResult.Tasks = new ApprenticeTasksCollection() { Tasks = new System.Collections.Generic.List<ApprenticeTask>() };
            var result = await controller.GetTaskById(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Update_Task_By_Id_Test(
            [Frozen] Mock<IMediator> mediator,
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
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            var result = await controller.UpdateTaskById(apprenticeId, taskId, ApprenticeTaskData);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Update_Task_By_Id_NoApprenticeship_Test(
           [Frozen] Mock<IMediator> mediator,
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
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });
            var result = await controller.UpdateTaskById(apprenticeId, taskId, ApprenticeTaskData);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Delete_TaskBy_Id_Tests(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            var result = await controller.DeleteTaskById(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Delete_TaskBy_Id_NoApprenticeship_Test(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });
            var result = await controller.DeleteTaskById(apprenticeId, taskId);
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
            [Frozen] Mock<IMediator> mediator,
             [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var ApprenticeTaskData = new ApprenticeTaskData();
            var taskId = 1;
            var statusId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            var result = await controller.UpdateTaskStatus(apprenticeId, taskId, statusId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Update_task_status_NoApprenticeship_Test(
           [Frozen] Mock<IMediator> mediator,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var ApprenticeTaskData = new ApprenticeTaskData();
            var taskId = 1;
            var statusId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });
            var result = await controller.UpdateTaskStatus(apprenticeId, taskId, statusId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Get_task_viewdata_no_task(
            [Frozen] Mock<IMediator> mediator,
             [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            var result = await controller.GetTaskViewData(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_task_viewdata_noapprenticeship_test(
           [Frozen] Mock<IMediator> mediator,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });
            var result = await controller.GetTaskViewData(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }


        [Test, MoqAutoData]
        public async Task Get_task_viewdata_no_categories(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTaskByTaskIdQueryResult taskResult,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var taskId = 1;
            string standardUid = "1";
            string option = "test";
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });
            mediator.Setup(x => x.Send(It.IsAny<GetTaskByTaskIdQuery>(), default)).ReturnsAsync(taskResult);
            taskResult.Tasks = new ApprenticeTasksCollection() { Tasks = new System.Collections.Generic.List<ApprenticeTask>() };
            var result = await controller.GetTaskViewData(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task Get_task_viewdata_no_ksbs(
           [Frozen] Mock<IMediator> mediator,
           [Frozen] GetTaskByTaskIdQueryResult taskResult,
           [Frozen] GetTaskCategoriesQueryResult categoriesResult,
           [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var taskId = 1;
            string standardUid = "1";
            string option = "test";
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            mediator.Setup(x => x.Send(It.IsAny<GetTaskByTaskIdQuery>(), default)).ReturnsAsync(taskResult);
            taskResult.Tasks = new ApprenticeTasksCollection() { Tasks = new System.Collections.Generic.List<ApprenticeTask>() };
            mediator.Setup(x => x.Send(It.IsAny<GetTaskCategoriesQuery>(), default)).ReturnsAsync(categoriesResult);
            categoriesResult.TaskCategories = new ApprenticeTaskCategoriesCollection() { TaskCategories = new System.Collections.Generic.List<ApprenticeTaskCategories>() };
            var result = await controller.GetTaskViewData(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult));
        }

        [Test, MoqAutoData]
        public async Task Get_task_viewdata(
           [Frozen] Mock<IMediator> mediator,
           [Frozen] GetTaskByTaskIdQueryResult taskResult,
           [Frozen] GetTaskCategoriesQueryResult categoriesResult,
           [Frozen] GetStandardOptionKsbsQueryResult ksbResult,
           [Frozen] GetKsbProgressForTaskQueryResult ksbProgressResult,
           [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var taskId = 1;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            string ksbId = "00000000-0000-0000-0000-000000000000";
            var ksb = new Ksb() { Id = new Guid(ksbId), Key = "test", Detail = "test" };
            var ksbProgress = new ApprenticeKsbProgressData() { ApprenticeshipId = 1, KSBId = new Guid(ksbId), KsbProgressId = 1 };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });
            mediator.Setup(x => x.Send(It.IsAny<GetTaskByTaskIdQuery>(), default)).ReturnsAsync(taskResult);
            taskResult.Tasks = new ApprenticeTasksCollection() { Tasks = new System.Collections.Generic.List<ApprenticeTask>() };
            mediator.Setup(x => x.Send(It.IsAny<GetTaskCategoriesQuery>(), default)).ReturnsAsync(categoriesResult);
            categoriesResult.TaskCategories = new ApprenticeTaskCategoriesCollection() { TaskCategories = new System.Collections.Generic.List<ApprenticeTaskCategories>() };
            mediator.Setup(x => x.Send(It.IsAny<GetStandardOptionKsbsQuery>(), default)).ReturnsAsync(ksbResult);
            ksbResult.KsbsResult = new GetStandardOptionKsbsResult() { Ksbs = new System.Collections.Generic.List<Ksb>() { ksb } };
            mediator.Setup(x => x.Send(It.IsAny<GetKsbProgressForTaskQuery>(), default)).ReturnsAsync(ksbProgressResult);
            ksbProgressResult.KSBProgress = new System.Collections.Generic.List<ApprenticeKsbProgressData>() { ksbProgress };
            var result = await controller.GetTaskViewData(apprenticeId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_TaskReminders(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] GetTaskRemindersByApprenticeshipIdQueryResult taskRemindersResult,
            [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = new MyApprenticeship { ApprenticeshipId = 1 }
                }
            });

            var result = await controller.GetTaskReminders(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_TaskReminders_NoApprenticeshipId(
           [Frozen] Mock<IMediator> mediator,
           [Greedy] TasksController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticeDetailsQuery>(), default)).ReturnsAsync(new GetApprenticeDetailsQueryResult
            {
                ApprenticeDetails = new ApprenticeDetails
                {
                    Apprentice = new Apprentice { ApprenticeId = apprenticeId },
                    MyApprenticeship = null
                }
            });

            var result = await controller.GetTaskReminders(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }
    }
}