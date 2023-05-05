using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetPostcodes
{
    public class GetPostcodeQuery : IRequest<GetPostcodeQueryResult>
    {
        public GetPostcodeQuery(string postCode) => PostCode = postCode;

        public string PostCode { get; }
    }
}
