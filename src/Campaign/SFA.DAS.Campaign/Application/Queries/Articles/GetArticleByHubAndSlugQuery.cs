using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.Articles
{
    public class GetArticleByHubAndSlugQuery : IRequest<GetArticleByHubAndSlugQueryResult>
    {
        public string Hub { get; set; }
        public string Slug { get; set; }
    }
}
