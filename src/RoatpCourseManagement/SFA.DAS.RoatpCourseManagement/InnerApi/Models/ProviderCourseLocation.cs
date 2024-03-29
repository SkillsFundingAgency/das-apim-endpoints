﻿using System;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Models
{
    public class ProviderCourseLocation
    {
        public int Id { get; set; }
        public Guid NavigationId { get; set; }
        public int ProviderCourseId { get; set; }
        public int? ProviderLocationId { get; set; }
        public decimal Radius { get; set; }
        public bool? HasDayReleaseDeliveryOption { get; set; }
        public bool? HasBlockReleaseDeliveryOption { get; set; }
        public bool IsImported { get; set; }
    }
}
