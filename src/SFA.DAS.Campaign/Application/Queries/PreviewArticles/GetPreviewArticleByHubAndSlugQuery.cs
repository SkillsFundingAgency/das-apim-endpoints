using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.PreviewArticles
{
    public class GetPreviewArticleByHubAndSlugQuery : IRequest<GetPreviewArticleByHubAndSlugQueryResult>
    {
        public string Hub { get; set; }
        public string Slug { get; set; }
    }
}