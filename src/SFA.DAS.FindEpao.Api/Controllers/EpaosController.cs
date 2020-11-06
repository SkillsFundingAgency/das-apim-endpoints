using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEpao.Api.Models;
using SFA.DAS.FindEpao.Application.Epaos.Queries.GetDeliveryAreaList;

namespace SFA.DAS.FindEpao.Api.Controllers
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
        [Route("delivery-areas")]
        public async Task<IActionResult> GetDeliveryAreas()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetDeliveryAreaListQuery());
                
                var model = new GetDeliveryAreaListResponse
                {
                    DeliveryAreas = queryResult.DeliveryAreas.Select(c=>(GetDeliveryAreaListItem)c).ToList()
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of delivery areas");
                return BadRequest();
            }
        }
    }
}