using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes
{
    public class GetRoutingInformationForFormQuery : IRequest<BaseMediatrResponse<GetRoutingInformationForFormQueryResponse>>
    {
        public Guid FormVersionId { get; set; }
    }
}