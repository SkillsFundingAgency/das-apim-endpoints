using System;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.CreateProviderCourse
{
    public class ProviderCourseLocationCommandModel
    {
        public Guid ProviderLocationId { get; set; }
        public bool HasDayReleaseDeliveryOption { get; set; }
        public bool HasBlockReleaseDeliveryOption { get; set; }
    }
}
