using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands;
using SFA.DAS.ApprenticeApp.Application.Commands.Tasks;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.ApprenticeApp.Application.Queries.Tasks;
using SFA.DAS.ApprenticeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("/apprentices/{apprenticeId}/progress/taskCategories")]
        public async Task<IActionResult> GetTaskCategories(Guid apprenticeId)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId });
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            var result = await _mediator.Send(new GetTaskCategoriesQuery { ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId });
            if (result.TaskCategories == null)
                return NotFound();
            return Ok(result.TaskCategories);
        }

        // add a new tasks
        [HttpPost("/apprentices/{apprenticeshipId}/progress/tasks")]
        public async Task<IActionResult> AddTask(long apprenticeshipId, ApprenticeTaskData data)
        {
            await _mediator.Send(new AddApprenticeTaskCommand
            {
                ApprenticeshipId = apprenticeshipId,
                Data = data
            });

            return Ok();
        }

        // gets the tasks based on dates and status
        [HttpGet("/apprentices/{apprenticeId}/progress/tasks")]
        public async Task<IActionResult> GetTasks(Guid apprenticeId, int status, DateTime? fromDate, DateTime? toDate)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId }); ;
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            var result = await _mediator.Send(new GetTasksByApprenticeshipIdQuery { ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId, Status = status, FromDate = fromDate, ToDate = toDate });
            if (result.Tasks == null)
                return NotFound();
            return Ok(result.Tasks);
        }

        // get a task by id
        [HttpGet("/apprentices/{apprenticeId}/progress/tasks/{taskId}")]
        public async Task<IActionResult> GetTaskById(Guid apprenticeId, int taskId)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId }); ;
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            var result = await _mediator.Send(new GetTaskByTaskIdQuery { ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId, TaskId = taskId });
            if (result.Tasks == null)
                return NotFound();
            return Ok(result.Tasks);
        }

        // update a task by id
        [HttpPut("/apprentices/{apprenticeId}/progress/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTaskById(Guid apprenticeId, int taskId, ApprenticeTaskData data)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId }); ;
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            await _mediator.Send(new UpdateApprenticeTaskCommand
            {
                ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId,
                TaskId = taskId,
                Data = data
            });

            return Ok();
        }

        // Delete a task by id
        [HttpDelete("/apprentices/{apprenticeId}/progress/tasks/{taskId}")]
        public async Task<IActionResult> DeleteTaskById(Guid apprenticeId, int taskId)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId }); ;
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            await _mediator.Send(new DeleteApprenticeTaskCommand
            {
                ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId,
                TaskId = taskId
            });

            return Ok();
        }

        // update a task status
        [HttpPost("/apprentices/{apprenticeId}/progress/tasks/{taskId}/status/{statusId}")]
        public async Task<IActionResult> UpdateTaskStatus(Guid apprenticeId, int taskId, int statusId)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId }); ;
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();
            await _mediator.Send(new UpdateApprenticeTaskStatusCommand
            {
                ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId,
                TaskId = taskId,
                StatusId = statusId
            });

            return Ok();
        }
        
        //Build viewmodel data for pwa
        [HttpGet("/apprentices/{apprenticeId}/progress/taskCategories/tasks/{taskId}/ksbs")]
        public async Task<IActionResult> GetTaskViewData(Guid apprenticeId, int taskId)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId }); ;
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();
            var apprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId;

            var taskResult = await _mediator.Send(new GetTaskByTaskIdQuery { ApprenticeshipId = apprenticeshipId, TaskId = taskId });
            if (taskResult.Tasks == null)
                return NotFound();

            var categoriesResult = await _mediator.Send(new GetTaskCategoriesQuery { ApprenticeshipId = apprenticeshipId });

            if (categoriesResult.TaskCategories == null)
                return NotFound();

            var ksbResult = await _mediator.Send(new GetStandardOptionKsbsQuery { Id = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.StandardUId, Option = "core" });

            if (ksbResult.KsbsResult == null)
                return NotFound();

            var ksbProgressResult = await _mediator.Send(new GetKsbProgressForTaskQuery
                {
                    ApprenticeshipId = apprenticeshipId,
                    TaskId = taskId
            });

            var ksbData = new List<ApprenticeKsbData>();
            if (ksbProgressResult.KSBProgress != null)
            { 
                foreach(var ksbProgress in ksbProgressResult.KSBProgress)
                {
                    var ksb = ksbResult.KsbsResult.Ksbs.First(x => x.Id == ksbProgress.KSBId);
                    if (ksb != null)
                    {
                        var apprenticeKsb = new ApprenticeKsbData()
                        {
                            ApprenticeshipId = ksbProgress.ApprenticeshipId,
                            KsbProgressId = ksbProgress.KsbProgressId,
                            KSBId = ksbProgress.KSBId,
                            KsbKey = ksb.Key,
                            CurrentStatus = ksbProgress.CurrentStatus,
                            Detail = ksb.Detail
                        };
                        ksbData.Add(apprenticeKsb);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            var getTaskViewDataResult = new ApprenticeTaskModelData()
            {
                Task = taskResult.Tasks.Tasks.FirstOrDefault(),
                KSBProgress = ksbData,
                TaskCategories = categoriesResult.TaskCategories
            };

            return Ok(getTaskViewDataResult);
        }

        [HttpGet("/apprentices/{apprenticeId}/progress/tasks/taskReminders")]
        public async Task<IActionResult> GetTaskReminders(Guid apprenticeId)
        {
            var apprenticeDetailsResult = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = apprenticeId }); ;
            if (apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship == null)
                return Ok();

            var taskRemindersResult = await _mediator.Send(new GetTaskRemindersByApprenticeshipIdQuery
            {
                ApprenticeshipId = apprenticeDetailsResult.ApprenticeDetails.MyApprenticeship.ApprenticeshipId
            });

            return Ok(taskRemindersResult.TaskReminders);
        }

        [HttpPost("/apprentices/{apprenticeId}/progress/tasks/taskReminders/{taskId}/{statusId}")]
        public async Task<IActionResult> UpdateTaskReminder(Guid apprenticeId, int taskId, int statusId)
        {
            await _mediator.Send(new UpdateApprenticeTaskReminderCommand
            {
                TaskId = taskId,
                StatusId = statusId
            });

            return Ok();
        }
    }
}