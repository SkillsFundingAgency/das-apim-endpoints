﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Asp.Versioning;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Api.Mappers;
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLogData;
using SFA.DAS.EarlyConnect.Application.Commands.DeliveryUpdateData;
using Newtonsoft.Json;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLogData;
using Microsoft.Azure.Amqp.Framing;
using Azure;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/early-connect/delivery-update/")]
    public class DeliveryUpdateController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DeliveryUpdateController> _logger;
        private readonly string DataSource = "UCAS";

        public DeliveryUpdateController(IMediator mediator, ILogger<DeliveryUpdateController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeliveryUpdate([FromBody] DeliveryUpdatePostRequest request)
        {
            try
            {
                var response = await _mediator.Send(new DeliveryUpdateCommand
                {
                    DeliveryUpdate = request.MapFromDeliveryUpdateRequest()
                });
                
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting delivery update data");

                var apiException = (e as SharedOuterApi.Exceptions.ApiResponseException);
                var status = apiException?.Status;
                var errorMessage = apiException?.Error;

                if (status.Equals(HttpStatusCode.InternalServerError))
                {
                    return Problem();
                }
                return BadRequest(errorMessage);
            }
        }
    }
}
