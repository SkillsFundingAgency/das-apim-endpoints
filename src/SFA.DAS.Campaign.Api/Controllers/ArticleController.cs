using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Articles;
using SFA.DAS.Campaign.Application.Queries.PreviewArticles;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ArticleController : Controller
    {
        private readonly IMediator _mediator;


        public ArticleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{hub}/{slug}")]
        public async Task<IActionResult> GetArticleAsync(string hub, string slug,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetArticleByHubAndSlugQuery
            {
                Hub = hub,
                Slug = slug
            }, cancellationToken);

            if (result.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Article not found for {hub}/{slug}"
                });
            }    

            return Ok(new GetArticleResponse
            {
                Article = result.PageModel
            });

        }

        [HttpGet("preview/{hub}/{slug}")]
        public async Task<IActionResult> GetPreviewArticle(string hub, string slug)
        {
            var result = await _mediator.Send(new GetPreviewArticleByHubAndSlugQuery
            {
                Hub = hub,
                Slug = slug
            });
            if (result.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Preview article not found for {hub}/{slug}"
                });
            }
            return Ok(new GetArticleResponse
            {
                Article = result.PageModel
            });
        }
    }
}