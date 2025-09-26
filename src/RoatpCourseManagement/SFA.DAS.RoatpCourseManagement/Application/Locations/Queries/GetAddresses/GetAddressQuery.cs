using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAddresses;
public class GetAddressesQuery : IGetApiRequest, IRequest<GetAddressesQueryResult>
{
    public GetAddressesQuery(string query) => Query = query;
    public string Query { get; }
    public double MinMatch { get; set; }

    public string GetUrl => $"api/addresses?query={Query}&MinMatch={MinMatch}";
}