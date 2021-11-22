﻿using System;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetEpaoCourseListItem
    {
        public int StandardCode { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? DateStandardApprovedOnRegister { get; set; }
        public GetStandardsExtendedListResponse StandardVersions { get; set; }
    }
}