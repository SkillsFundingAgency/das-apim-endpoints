using System;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class ProviderLocationUpdateModel
    {
        public int Ukprn { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string LocationName { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
