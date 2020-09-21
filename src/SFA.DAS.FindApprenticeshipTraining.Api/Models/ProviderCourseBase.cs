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
        public GetProviderFeedbackResponse FeedbackResponse { get ; set ; }
        
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

        protected GetProviderFeedbackResponse ProviderFeedbackResponse(IEnumerable<GetFeedbackRatingItem> getFeedbackRatingItems)
        {
            if (getFeedbackRatingItems == null)
            {
                return new GetProviderFeedbackResponse
                {
                    TotalEmployerResponses = 0,
                    TotalFeedbackRating = 0
                };
            }

            var feedbackRatingItems = getFeedbackRatingItems.ToList();
            var totalRatings = feedbackRatingItems.Sum(c => c.FeedbackCount);

            var ratings = 0;

            foreach (var feedbackRatingItem in feedbackRatingItems)
            {
                switch (feedbackRatingItem.FeedbackName.ToLower())
                {
                    case "very poor":
                        ratings += feedbackRatingItem.FeedbackCount * 1;
                        break;
                    case "poor":
                        ratings += feedbackRatingItem.FeedbackCount * 2;
                        break;
                    case "good":
                        ratings += feedbackRatingItem.FeedbackCount * 3;
                        break;
                    case "excellent":
                        ratings += feedbackRatingItem.FeedbackCount * 4;
                        break;
                }
            }

            var ratingAverage = Math.Round((double)ratings / totalRatings,1 );

            var ratingResponse = 0;
            if (ratingAverage >= 1 && ratingAverage < 1.3)
            {
                ratingResponse = 1;
            } else if (ratingAverage >= 1.3 && ratingAverage < 2.3)
            {
                ratingResponse = 2;
            }else if (ratingAverage >= 2.3 && ratingAverage < 3.3)
            {
                ratingResponse = 3;
            }else if (ratingAverage >= 3.3 && ratingAverage < 4)
            {
                ratingResponse = 4;
            }
            
            return new GetProviderFeedbackResponse
            {
                TotalFeedbackRating = ratingResponse,
                TotalEmployerResponses = totalRatings
            };
        }

        protected List<GetDeliveryType> FilterDeliveryModes(IEnumerable<GetDeliveryTypeItem> getDeliveryTypeItems)
        {
            var hasWorkPlace = false;
            var hasDayRelease = false;
            var hasBlockRelease = false;
            var filterDeliveryModes = new List<GetDeliveryType>();

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
                    }
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
                _ => default
            };
        }

        private GetDeliveryType CreateDeliveryTypeItem(GetDeliveryTypeItem deliveryTypeItem)
        {
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