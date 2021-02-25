using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests
{
    public class GetCourseProvidersRequest
    {
        [FromQuery]
        public string Location { get; set; }
        [FromQuery]
        public double Lat { get; set; } = 0;
        [FromQuery]
        public double Lon { get; set; } = 0;
        [FromQuery]
        public List<DeliveryModeType> DeliveryModes { get; set; } = null;
        [FromQuery]
        public List<FeedbackRatingType> ProviderRatings { get; set; } = null;
        [FromQuery] 
        public ProviderCourseSortOrder.SortOrder SortOrder { get; set; } = ProviderCourseSortOrder.SortOrder.Distance;
        [FromQuery]
        public Guid? ShortlistUserId { get; set; }

    }
}