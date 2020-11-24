using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos;

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
        public async Task<IActionResult> GetList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetEpaosQuery());
                
                var model = new GetEpaosApiModel
                {
                    Epaos = queryResult.Epaos.Select(item => (EpaoListItem)item)
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of Epaos");
                return BadRequest();
            }
        }
    }
}
