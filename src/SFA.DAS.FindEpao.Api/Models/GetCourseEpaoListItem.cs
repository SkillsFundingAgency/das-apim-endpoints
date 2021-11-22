using SFA.DAS.FindEpao.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetCourseEpaoListItem
    {
        public string EpaoId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public List<StandardVersions> standardVersions { get; set; }
        public IEnumerable<EpaoDeliveryArea> DeliveryAreas { get; set; }

        public static implicit operator GetCourseEpaoListItem(InnerApi.Responses.GetCourseEpaoListItem source)
        {
            return new GetCourseEpaoListItem
            {
                EpaoId = source.EpaoId,
                Name = source.Name,
                City = source.City,
                Postcode = source.Postcode,
                DeliveryAreas = source.DeliveryAreas.Select(area => (EpaoDeliveryArea)area),
                EffectiveFrom = (DateTime)source.CourseEpaoDetails.EffectiveFrom,
                standardVersions = source.CourseEpaoDetails.standardVersions.Select(x => (StandardVersions)x).ToList()
            };
        }
    }

    public class EpaoDeliveryArea
    {
        public int DeliveryAreaId { get; set; }

        public static implicit operator EpaoDeliveryArea(InnerApi.Responses.EpaoDeliveryArea source)
        {
            return new EpaoDeliveryArea { DeliveryAreaId = source.DeliveryAreaId };
        }
    }

    public class StandardVersions
    {
        public string StandardUId { get; set; }
        public string Title { get; set; }
        public int LarsCode { get; set; }
        public string Version { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? DateVersionApproved { get; set; }
        public string Status { get; set; }

        public static implicit operator StandardVersions(InnerApi.Responses.GetStandardsExtendedListItem source)
        {
            return new StandardVersions
            {
                DateVersionApproved = source.DateVersionApproved,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                LarsCode = source.LarsCode,
                StandardUId = source.StandardUId,
                Status = source.Status,
                Title = source.Title,
                Version = source.Version
            };
        }
    }
}