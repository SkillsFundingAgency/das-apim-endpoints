using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation
{
    public class CreateProviderLocationCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public string UserId { get; set; }
        public string LocationName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string County { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
    }
}
