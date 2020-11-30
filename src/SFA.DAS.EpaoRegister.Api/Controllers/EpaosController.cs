using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpao;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Models;

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
        public async Task<IActionResult> GetEpaos() //todo: filter by status
        {
            try
            {
                var queryResult = await _mediator.Send(new GetEpaosQuery());
                
                var model = (GetEpaosApiModel)queryResult;

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of Epaos");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{epaoId}")]
        public async Task<IActionResult> GetEpao(string epaoId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetEpaoQuery {EpaoId = epaoId});

                var model = (GetEpaoApiModel) queryResult?.Epao;

                return Ok(model);
            }
            catch (EntityNotFoundException<SearchEpaosListItem> ex)
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
        [Route("{epaoId}/courses")]
        public async Task<IActionResult> GetEpaoCourses(string epaoId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetEpaoCoursesQuery {EpaoId = epaoId});

                var model = (GetEpaoCoursesApiModel) queryResult;

                return Ok(model);
            }
            catch (EntityNotFoundException<SearchEpaosListItem> ex)
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
