using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetAddresses
{
    public class GetAddressesQuery : IRequest<GetAddressesQueryResult>
    {
        public string Postcode { get; }
        
        public GetAddressesQuery(string query)
        {
            Postcode = query;
        }
    }
}
