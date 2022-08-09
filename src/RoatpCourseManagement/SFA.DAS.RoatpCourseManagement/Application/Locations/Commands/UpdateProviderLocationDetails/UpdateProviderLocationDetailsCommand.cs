using MediatR;
using System;
using System.Net;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.UpdateProviderLocationDetails
{
    public class UpdateProviderLocationDetailsCommand : IRequest<HttpStatusCode>
    {
        public int Ukprn{ get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string LocationName { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
