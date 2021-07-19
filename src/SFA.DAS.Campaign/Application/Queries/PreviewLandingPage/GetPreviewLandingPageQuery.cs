using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.PreviewLandingPage
{
    public class GetPreviewLandingPageQuery : IRequest<GetPreviewLandingPageQueryResult>
    {
        public string Hub { get; set; }
        public string Slug { get; set; }
    }
}