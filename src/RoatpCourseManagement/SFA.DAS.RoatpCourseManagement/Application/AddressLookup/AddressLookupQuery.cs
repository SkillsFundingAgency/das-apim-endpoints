using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.Application.AddressLookup
{
    public class AddresssLookupQuery : IRequest<AddresssLookupQueryResult>
    {
        public string Postcode { get; }
        public AddresssLookupQuery(string postcode)
        {
            Postcode = postcode;
        }
    }
}
