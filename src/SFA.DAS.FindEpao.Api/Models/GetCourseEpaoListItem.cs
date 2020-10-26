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

        public IEnumerable<EpaoDeliveryArea> DeliveryAreas { get; set; }

        public static implicit operator GetCourseEpaoListItem(InnerApi.Responses.GetCourseEpaoListItem source)
        {
            return new GetCourseEpaoListItem
            {
                EpaoId = source.EpaoId,
                Name = source.Name,
                City = source.City,
                Postcode = source.Postcode,
                DeliveryAreas = source.DeliveryAreas.Select(area => (EpaoDeliveryArea)area)
            };
        }
    }

    public class EpaoDeliveryArea
    {
        public int DeliveryAreaId { get; set; }

        public static implicit operator EpaoDeliveryArea(InnerApi.Responses.EpaoDeliveryArea source)
        {
            return new EpaoDeliveryArea{DeliveryAreaId = source.DeliveryAreaId};
        }
    }
}