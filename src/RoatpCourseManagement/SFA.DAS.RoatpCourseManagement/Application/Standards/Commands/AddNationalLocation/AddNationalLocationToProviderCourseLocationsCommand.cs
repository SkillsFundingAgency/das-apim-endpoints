using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.AddNationalLocation
{
    public class AddNationalLocationToProviderCourseLocationsCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public string LarsCode { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}
