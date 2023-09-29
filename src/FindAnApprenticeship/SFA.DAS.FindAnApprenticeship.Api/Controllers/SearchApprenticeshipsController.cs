using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class SearchApprenticeshipsController : Controller
    {
        private readonly IMediator _mediator;

        public SearchApprenticeshipsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _mediator.Send(new SearchApprenticeshipsQuery());
                var viewModel = (SearchApprenticeshipsApiResponse)result;
                return Ok(viewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new  StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
            
        }
    }
}