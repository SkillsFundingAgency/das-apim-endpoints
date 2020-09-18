using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class ProviderCourseBase
    {
        public int ProviderId { get ; set ; }

        public string Name { get ; set ; }
        public List<GetDeliveryType> DeliveryModes { get ; set ; }
        public int? OverallCohort { get; set; }
        public decimal? OverallAchievementRate { get ; set ; }
        
        private string MapLevel(int level)
        {
            if (level == 2)
            {
                return "Two";
            }
            if (level == 3)
            {
                return "Three";
            }
            if (level > 3)
            {
                return "AllLevels";
            }
            return "";
        }

        protected GetAchievementRateItem GetAchievementRateItem(IEnumerable<GetAchievementRateItem> list, string subjectArea, int level)
        {
            if (list == null)
                return null;
            
            var result = list.Where(c =>
                c.SectorSubjectArea.Equals(subjectArea, StringComparison.CurrentCultureIgnoreCase)).ToList();

            if (result.Count == 0)
                return null;
            
            var item = result.FirstOrDefault(c => c.Level.Equals(MapLevel(level))) 
                       ?? result.FirstOrDefault(c => c.Level.Equals("AllLevels"));

            return item;
        }

        protected List<GetDeliveryType> FilterDeliveryModes(IEnumerable<GetDeliveryTypeItem> getDeliveryTypeItems)
        {
            var hasWorkPlace = false;
            var hasDayRelease = false;
            var hasBlockRelease = false;
            var isNotFound = false;
            var filterDeliveryModes = new List<GetDeliveryType>();

            if (getDeliveryTypeItems.Any(x => x.DeliveryModes == DeliveryModeType.NotFound.ToString()))
            {
                var item = CreateDeliveryTypeItem(getDeliveryTypeItems.First());
                filterDeliveryModes.Add(item);
                return filterDeliveryModes;
            }

            foreach (var deliveryTypeItem in getDeliveryTypeItems)
            {
                var deliveryTypeItemSplit = deliveryTypeItem.DeliveryModes.Split("|").ToList();

                foreach (var mappedType in deliveryTypeItemSplit.Select(MapDeliveryType))
                {
                    var item = CreateDeliveryTypeItem(deliveryTypeItem);
                    switch (mappedType)
                    {
                        case DeliveryModeType.Workplace when !hasWorkPlace:
                            item.DeliveryModeType = DeliveryModeType.Workplace;
                            filterDeliveryModes.Add(item);
                            hasWorkPlace = true;
                            break;
                        case DeliveryModeType.BlockRelease when !hasBlockRelease:
                            item.DeliveryModeType = DeliveryModeType.BlockRelease;
                            filterDeliveryModes.Add(item);
                            hasBlockRelease = true;
                            break;
                        case DeliveryModeType.DayRelease when !hasDayRelease:
                            item.DeliveryModeType = DeliveryModeType.DayRelease;
                            filterDeliveryModes.Add(item);
                            hasDayRelease = true;
                            break;
                        case DeliveryModeType.NotFound when !isNotFound:
                            item.DeliveryModeType = DeliveryModeType.NotFound;
                            filterDeliveryModes.Add(item);
                            isNotFound = true;
                            break;
                    }
                }

                if (isNotFound)
                {
                    break;
                }

                if (hasBlockRelease && hasDayRelease && hasWorkPlace)
                {
                    break;
                }
            }
            return filterDeliveryModes; 
                
        }
        private DeliveryModeType MapDeliveryType(string deliveryType)
        {
            return deliveryType switch
            {
                "100PercentEmployer" => DeliveryModeType.Workplace,
                "DayRelease" => DeliveryModeType.DayRelease,
                "BlockRelease" => DeliveryModeType.BlockRelease,
                "NotFound" => DeliveryModeType.NotFound,
                _ => default
            };
        }

        private GetDeliveryType CreateDeliveryTypeItem(GetDeliveryTypeItem deliveryTypeItem)
        {
            if (deliveryTypeItem.DeliveryModes == DeliveryModeType.NotFound.ToString())
            {
                return new GetDeliveryType
                {
                    DeliveryModeType = DeliveryModeType.NotFound
                };
            }
            return new GetDeliveryType
            {
                DistanceInMiles = deliveryTypeItem.DistanceInMiles,
                Address1 = deliveryTypeItem.Address1,
                Address2 = deliveryTypeItem.Address2,
                County = deliveryTypeItem.County,
                Postcode = deliveryTypeItem.Postcode,
                Town = deliveryTypeItem.Town
            };
        }
    }
}