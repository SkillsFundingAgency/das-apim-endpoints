using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;

namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    public class SupportAndGuidanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupportAndGuidanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/supportguidance/categories/{contentType}")]
        public async Task<IActionResult> GetCategories(string contentType)
        {
            var queryResult = await _mediator.Send(new GetCategoriesByContentTypeQuery
            {
                ContentType = contentType
            });

            return Ok(queryResult.CategoryPages);
        }

        [HttpGet]
        [Route("/supportguidance/category/{categoryIdentifier}/articles/")]
        [Route("/supportguidance/category/{categoryIdentifier}/articles/{id}")]
        public async Task<IActionResult> GetArticlesForCategory(string categoryIdentifier, Guid? id = null)
        {
            var queryResult = await _mediator.Send(new GetCategoryArticlesByIdentifierQuery
            {
                Slug = categoryIdentifier,
                Id = id
            });

            return Ok(queryResult);
        }

        [HttpGet]
        [Route("/supportguidance/articles/{entryId}")]
        public async Task<IActionResult> GetOneByEntryId(string entryId)
        {
            var queryResult = await _mediator.Send(new GetContentQuery
            {
                EntryId = entryId
            });

            return Ok();
        }

        [HttpGet]
        [Route("/supportguidance/savedarticles/{id}")]
        public async Task<IActionResult> GetSavedArticles(Guid id)
        {
            var queryResult = await _mediator.Send(new GetUserSavedArticlesQuery
            {
                ApprenticeId = id
            });

            return Ok(queryResult);
        }

        [HttpPost("/apprentices/{id}/articles/{articleIdentifier}")]
        public async Task<IActionResult> AddUpdateApprenticeArticle(Guid id, string articleIdentifier, [FromBody] ApprenticeArticleRequest request)
        {
            await _mediator.Send(new AddUpdateApprenticeArticleCommand
            {
                Id = id,
                EntryId = articleIdentifier,
                IsSaved = request.IsSaved,
                LikeStatus = request.LikeStatus
            });

            return Ok();
        }

        public class ApprenticeArticleRequest
        {
            public bool? IsSaved { get; set; }
            public bool? LikeStatus { get; set; }
        }
    }
}