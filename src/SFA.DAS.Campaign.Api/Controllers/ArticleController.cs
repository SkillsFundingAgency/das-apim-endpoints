using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Articles;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.Interfaces;

namespace SFA.DAS.Campaign.Api.Controllers
{
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
                Hub = hub.ToTitleCase(),
                Slug = slug
            }, cancellationToken);

            if (!result.Article.Items.Any())
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"couldn't find an article for {hub}/{slug}"
                });
            }

            return Ok(new GetArticleResponse
            {
                Article = result.Article
            });

        }
    }
}