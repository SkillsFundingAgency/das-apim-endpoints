using SFA.DAS.FindEpao.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetCourseEpaoStandardsListItem
    {
        public string Version { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public static implicit operator GetCourseEpaoStandardsListItem(InnerApi.Responses.GetStandardsExtendedListItem source)
        {
            return new GetCourseEpaoStandardsListItem
            {
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                Version = source.Version
            };       
        }
    }
}