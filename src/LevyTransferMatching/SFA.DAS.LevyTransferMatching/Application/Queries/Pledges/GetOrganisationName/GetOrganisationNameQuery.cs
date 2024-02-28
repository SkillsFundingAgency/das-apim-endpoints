using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetOrganisationName
{
    public class GetOrganisationNameQuery : IRequest<GetOrganisationNameQueryResult>
    {
        public string EncodedAccountId { get; set; }
    }
}
