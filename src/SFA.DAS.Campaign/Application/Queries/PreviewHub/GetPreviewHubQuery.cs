using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.PreviewHub
{
    public class GetPreviewHubQuery : IRequest<GetPreviewHubQueryResult>
    {
        public string Hub { get; set; }
    }
}
