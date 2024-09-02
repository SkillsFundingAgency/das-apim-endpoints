using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
            => _mediator = mediator;

        // gets the task categories
        [HttpGet("/apprentices/{apprenticeshipId}/progress/taskCategories")]
        public async Task<IActionResult> GetTaskCategories(long apprenticeshipId)
        {
            var result = await _mediator.Send(new GetTaskCategoriesQuery { ApprenticeshipId = apprenticeshipId });
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
        [HttpGet("/apprentices/{apprenticeshipId}/progress/tasks")]
        public async Task<IActionResult> GetTasks(long apprenticeshipId, int status, DateTime? fromDate, DateTime? toDate)
        {
            var result = await _mediator.Send(new GetTasksByApprenticeshipIdQuery { ApprenticeshipId = apprenticeshipId, Status = status, FromDate = fromDate, ToDate = toDate });
            if (result.Tasks == null)
                return NotFound();
            return Ok(result.Tasks);
        }

        // get a task by id
        [HttpGet("/apprentices/{apprenticeshipId}/progress/tasks/{taskId}")]
        public async Task<IActionResult> GetTaskById(long apprenticeshipId, int taskId)
        {
            var result = await _mediator.Send(new GetTaskByTaskIdQuery { ApprenticeshipId = apprenticeshipId, TaskId = taskId });
            if (result.Tasks == null)
                return NotFound();
            return Ok(result.Tasks);
        }

        // update a task by id
        [HttpPut("/apprentices/{apprenticeshipId}/progress/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTaskById(long apprenticeshipId, int taskId, ApprenticeTaskData data)
        {
            await _mediator.Send(new UpdateApprenticeTaskCommand
            {
                ApprenticeshipId = apprenticeshipId,
                TaskId = taskId,
                Data = data
            });

            return Ok();
        }

        // Delete a task by id
        [HttpDelete("/apprentices/{apprenticeshipId}/progress/tasks/{taskId}")]
        public async Task<IActionResult> DeleteTaskById(long apprenticeshipId, int taskId)
        {
            await _mediator.Send(new DeleteApprenticeTaskCommand
            {
                ApprenticeshipId = apprenticeshipId,
                TaskId = taskId
            });

            return Ok();
        }

        // update a task status
        [HttpPost("/apprentices/{apprenticeshipId}/progress/tasks/{taskId}/status/{statusId}")]
        public async Task<IActionResult> UpdateTaskStatus(long apprenticeshipId, int taskId, int statusId)
        {
            await _mediator.Send(new UpdateApprenticeTaskStatusCommand
            {
                ApprenticeshipId = apprenticeshipId,
                TaskId = taskId,
                StatusId = statusId
            });

            return Ok();
        }
        
        //Build viewmodel data for pwa
        [HttpGet("/apprentices/{apprenticeshipId}/progress/taskCategories/tasks/{taskId}/courses/{standardUid}/options/{option}/ksbs")]
        public async Task<IActionResult> GetTaskViewData(long apprenticeshipId, int taskId, string standardUid, string option)
        {
            var taskResult = await _mediator.Send(new GetTaskByTaskIdQuery { ApprenticeshipId = apprenticeshipId, TaskId = taskId });
            if (taskResult.Tasks == null)
                return NotFound();

            var categoriesResult = await _mediator.Send(new GetTaskCategoriesQuery { ApprenticeshipId = apprenticeshipId });

            if (categoriesResult.TaskCategories == null)
                return NotFound();

            var ksbResult = await _mediator.Send(new GetStandardOptionKsbsQuery { Id = standardUid, Option = option });

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
    }
}