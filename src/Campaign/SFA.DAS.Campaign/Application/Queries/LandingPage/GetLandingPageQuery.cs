using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.LandingPage
{
    public class GetLandingPageQuery : IRequest<GetLandingPageQueryResult>
    {
        public string Hub { get; set; }
        public string Slug { get; set; }
    }
}
