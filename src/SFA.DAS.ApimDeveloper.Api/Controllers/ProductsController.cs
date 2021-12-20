using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.ApiProducts.Queries.GetApiProduct;

namespace SFA.DAS.ApimDeveloper.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController (IMediator mediator, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetApiProduct(string id)
        {
            try
            {
                var result = await _mediator.Send(new GetApiProductQuery
                {
                    ProductName = id
                });

                var model = (ProductApiResponseItem)result.Product;

                if (model == null)
                {
                    return new NotFoundResult();
                }

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to get Api product for {id}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}