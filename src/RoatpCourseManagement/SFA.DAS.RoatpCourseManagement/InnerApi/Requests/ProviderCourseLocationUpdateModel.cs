using System;
using System.Collections.Generic;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class ProviderCourseLocationUpdateModel
    {
        public int Ukprn { get; set; }
        public int LarsCode { get; set; }
        public string UserId { get; set; }
        public List<int> SelectedSubregionIds { get; set; }
    }
}
