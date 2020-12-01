using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EpaoRegister.Api.Infrastructure;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpao;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos;
using SFA.DAS.SharedOuterApi.Exceptions;

namespace SFA.DAS.EpaoRegister.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class EpaosController : ControllerBase
    {
        private readonly ILogger<EpaosController> _logger;
        private readonly IMediator _mediator;

        public EpaosController(ILogger<EpaosController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("", Name = RouteNames.GetEpaos)]
        public async Task<IActionResult> GetEpaos()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetEpaosQuery());
                
                var model = (GetEpaosApiModel)queryResult;
                model?.BuildLinks(Url);

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of Epaos");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{epaoId}", Name = RouteNames.GetEpao)]
        public async Task<IActionResult> GetEpao(string epaoId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetEpaoQuery {EpaoId = epaoId});

                var model = (GetEpaoApiModel) queryResult?.Epao;
                model?.BuildLinks(Url);

                return Ok(model);
            }
            catch (NotFoundException<GetEpaoResult> ex)
            {
                _logger.LogInformation(ex, $"Epao not found for EpaoId:[{epaoId}]");
                return NotFound();
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation(ex, $"Invalid EpaoId, EpaoId:[{epaoId}]");
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Epao, EpaoId:[{epaoId}]");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{epaoId}/courses", Name = RouteNames.GetEpaoCourses)]
        public async Task<IActionResult> GetEpaoCourses(string epaoId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetEpaoCoursesQuery {EpaoId = epaoId});

                var model = (GetEpaoCoursesApiModel) queryResult;
                model?.BuildLinks(Url);

                return Ok(model);
            }
            catch (NotFoundException<GetEpaoCoursesResult> ex)
            {
                _logger.LogInformation(ex, $"Epao courses not found for EpaoId:[{epaoId}]");
                return NotFound();
            }
            catch (ValidationException ex)
            {
                _logger.LogInformation(ex, $"Invalid EpaoId, EpaoId:[{epaoId}]");
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Epao courses, EpaoId:[{epaoId}]");
                return BadRequest();
            }
        }
    }
}
