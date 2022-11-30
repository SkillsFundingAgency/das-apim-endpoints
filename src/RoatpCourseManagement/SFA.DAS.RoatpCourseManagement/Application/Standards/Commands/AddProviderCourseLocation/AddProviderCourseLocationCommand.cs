using MediatR;
using System;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation
{
    public class AddProviderCourseLocationCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public Guid LocationNavigationId { get; set; }
        public bool? HasDayReleaseDeliveryOption { get; set; }
        public bool? HasBlockReleaseDeliveryOption { get; set; }
    }
}
