using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts;
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
        [HttpGet("/apprentices/{id}/progress/taskCategories")]
        public async Task<IActionResult> GetTaskCategories(Guid id)
        {
            var result = await _mediator.Send(new GetTaskCategoriesQuery { ApprenticeshipId = id });
            if (result.TaskCategories == null)
                return NotFound();
            return Ok(result);
        }

        // add a new tasks
        [HttpPost("/apprentices/{id}/progress/tasks")]
        public async Task<IActionResult> AddTask(Guid id, ApprenticeTaskData data)
        {
            await _mediator.Send(new AddApprenticeTaskCommand
            {
                ApprenticeshipId = id,
                Data = data
            });

            return Ok();
        }

        // gets the tasks based on dates and status
        [HttpGet("/apprentices/{id}/progress/tasks")]
        public async Task<IActionResult> GetTasks(Guid apprenticeshipId, int status, DateTime? fromDate, DateTime? toDate)
        {
            var result = await _mediator.Send(new GetTasksByApprenticeshipIdQuery { ApprenticeshipId = apprenticeshipId, Status = status, FromDate = fromDate, ToDate = toDate });
            if (result.Tasks == null)
                return NotFound();
            return Ok(result);
        }

        // get a task by id
        [HttpGet("/apprentices/{id}/progress/tasks/{taskId}")]
        public async Task<IActionResult> GetTaskById(Guid id, int taskId)
        {
            var result = await _mediator.Send(new GetTaskByTaskIdQuery { ApprenticeshipId = id, TaskId = taskId });
            if (result.Tasks == null)
                return NotFound();
            return Ok(result);
        }

        // update a task by id
        [HttpPut("/apprentices/{id}/progress/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTaskById(Guid id, int taskId, ApprenticeTaskData data)
        {
            await _mediator.Send(new UpdateApprenticeTaskCommand
            {
                ApprenticeshipId = id,
                TaskId = taskId,
                Data = data
            });

            return Ok();
        }

        // Delete a task by id
        [HttpDelete("/apprentices/{id}/progress/tasks/{taskId}")]
        public async Task<IActionResult> DeleteTaskById(Guid id, int taskId)
        {
            await _mediator.Send(new DeleteApprenticeTaskCommand
            {
                ApprenticeshipId = id,
                TaskId = taskId
            });

            return Ok();
        }
    }
}