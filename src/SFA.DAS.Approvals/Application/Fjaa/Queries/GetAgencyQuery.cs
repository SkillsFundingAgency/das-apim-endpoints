using MediatR;

namespace SFA.DAS.Approvals.Application.Fjaa.Queries
{
    public class GetAgencyQuery : IRequest<GetAgencyResult>
    {
        public int LegalEntityId { get; set; }
    }
}