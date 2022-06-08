﻿using System.Collections.Generic;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Models.RegisteredProvider
{
    public class Feedback
    {
        public int Total { get; set; }
        public List<FeedbackRating> FeedbackRating { get; set; }
        public List<ProviderAttribute> ProviderAttributes { get; set; }
    }
}