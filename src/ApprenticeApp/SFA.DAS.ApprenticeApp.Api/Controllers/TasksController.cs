using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using System;
using System.Threading.Tasks;

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
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();            
            return Ok(result.ApprenticeDetails);
        }

        // add a new task category
        [HttpPost("/apprentices/{id}/progress/taskCategories")]
        public async Task<IActionResult> AddTaskCategories(Guid id)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            return Ok(result.ApprenticeDetails);
        }


        // deletes a new task category
        [HttpDelete("/apprentices/{id}/progress/taskCategories")]
        public async Task<IActionResult> DeleteTaskCategories(Guid id)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            return Ok(result.ApprenticeDetails);
        }

        // gets the tasks
        [HttpGet("/apprentices/{id}/progress/tasks")]
        public async Task<IActionResult> GetTasks(Guid apprenticeshipId, int status, DateTime? fromDate, DateTime? toDate)
        {

            var result = await _mediator.Send(new GetTasksByApprenticeshipIdQuery { ApprenticeshipId = apprenticeshipId, Status = status, FromDate = fromDate, ToDate = toDate });

            //if (result.ApprenticeDetails?.Apprentice == null)
          //      return NotFound();
            return Ok();
        }

        // add a new tasks
        [HttpPost("/apprentices/{id}/progress/tasks")]
        public async Task<IActionResult> AddTasks(Guid id)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            return Ok(result.ApprenticeDetails);
        }


        // get a task by id
        [HttpGet("/apprentices/{id}/progress/tasks/{taskId}")]
        public async Task<IActionResult> GetTaskById(Guid id, int taskId)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            return Ok(result.ApprenticeDetails);
        }

        // update a task by id
        [HttpPut("/apprentices/{id}/progress/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTaskById(Guid id, int taskId)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            return Ok(result.ApprenticeDetails);
        }

        // Delete a task by id
        [HttpDelete("/apprentices/{id}/progress/tasks/{taskId}")]
        public async Task<IActionResult> DeleteTaskById(Guid id, int taskId)
        {
            var result = await _mediator.Send(new GetApprenticeDetailsQuery { ApprenticeId = id });
            if (result.ApprenticeDetails?.Apprentice == null)
                return NotFound();
            return Ok(result.ApprenticeDetails);
        }



    }
}
