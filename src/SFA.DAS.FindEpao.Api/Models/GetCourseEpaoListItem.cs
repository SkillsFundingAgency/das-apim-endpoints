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
                standardVersions = source.CourseEpaoDetails.standardVersions.ConvertAll(x => 
                                        new StandardVersions { standardUId = x.standardUId, dateVersionApproved = x.dateVersionApproved, 
                                        effectiveFrom = x.effectiveFrom, effectiveTo = x.effectiveTo, larsCode = x.larsCode, 
                                        status = x.status, title = x.title, version = x.version})
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
            public string standardUId { get; set; }
            public string title { get; set; }
            public int larsCode { get; set; }
            public string version { get; set; }
            public DateTime? effectiveFrom { get; set; }
            public DateTime? effectiveTo { get; set; }
            public DateTime? dateVersionApproved { get; set; }
            public string status { get; set; }
    }
            
}