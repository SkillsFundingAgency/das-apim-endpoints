using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsResult>
    {
        public string Query { get; set; }
    }
}
