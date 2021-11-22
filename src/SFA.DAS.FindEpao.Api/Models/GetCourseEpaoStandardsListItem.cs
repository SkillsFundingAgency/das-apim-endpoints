using SFA.DAS.FindEpao.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetCourseEpaoStandardsListItem
    {
        public string StandardUId { get; set; }
        public string Title { get; set; }
        public int LarsCode { get; set; }
        public int Level { get; set; }
        public string Version { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? DateVersionApproved { get; set; }
        public string Status { get; set; }


        public static implicit operator GetCourseEpaoStandardsListItem(InnerApi.Responses.GetStandardsExtendedListItem source)
        {
            return new GetCourseEpaoStandardsListItem
            {
                DateVersionApproved = source.DateVersionApproved,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                Level = source.Level,
                LarsCode = source.LarsCode,
                StandardUId = source.StandardUId,
                Status = source.Status,
                Title = source.Title,
                Version = source.Version
            };       
        }
    }
}