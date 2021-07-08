using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.SiteMap
{
    public class GetSiteMapQuery : IRequest<GetSiteMapQueryResult>
    {
        public string ContentType { get; set; }
    }
}
