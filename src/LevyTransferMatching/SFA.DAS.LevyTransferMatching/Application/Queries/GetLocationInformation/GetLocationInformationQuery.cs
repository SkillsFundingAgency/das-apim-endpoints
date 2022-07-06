using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLocationInformation
{
    public class GetLocationInformationQuery : IRequest<GetLocationInformationResult>
    {
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
