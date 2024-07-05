using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetClosestRegion
{
    public class GetClosestRegionQuery : IRequest<GetClosestRegionResult>
    {
        public double Longitude { get; set;  }
        public double Latitude { get; set; }
    }
}