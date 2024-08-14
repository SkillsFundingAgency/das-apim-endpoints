using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
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
            return Ok(result);
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
            return Ok(result);
        }

        // get a task by id
        [HttpGet("/apprentices/{apprenticeshipId}/progress/tasks/{taskId}")]
        public async Task<IActionResult> GetTaskById(long apprenticeshipId, int taskId)
        {
            var result = await _mediator.Send(new GetTaskByTaskIdQuery { ApprenticeshipId = apprenticeshipId, TaskId = taskId });
            if (result.Tasks == null)
                return NotFound();
            return Ok(result);
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
    }
}