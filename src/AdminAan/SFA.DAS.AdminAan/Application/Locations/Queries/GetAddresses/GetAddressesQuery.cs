using MediatR;

namespace SFA.DAS.AdminAan.Application.Locations.Queries.GetAddresses;
public class GetAddressesQuery : IRequest<GetAddressesQueryResult>
{
    public GetAddressesQuery(string query) => Query = query;

    public string Query { get; }
}