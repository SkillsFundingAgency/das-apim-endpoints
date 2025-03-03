using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class ProviderCourseBase
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string TradingName { get; set; }
        public Guid? ShortlistId { get; set; }
        public List<GetDeliveryType> DeliveryModes { get; set; }
        public int? OverallCohort { get; set; }
        public decimal? OverallAchievementRate { get; set; }
        public GetEmployerFeedbackResponse EmployerFeedback { get; set; }
        public GetApprenticeFeedbackResponse ApprenticeFeedback { get; set; }
        public bool HasLocation { get; set; }
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

        protected GetAchievementRateItem GetAchievementRateItem(IEnumerable<GetAchievementRateItem> list, int level)
        {
            if (!list.Any()) return null;

            return list.FirstOrDefault(c => c.Level.Equals(MapLevel(level))) ?? list.FirstOrDefault(c => c.Level.Equals("AllLevels"));
        }

        protected GetEmployerFeedbackResponse EmployerFeedbackResponse(InnerApi.Responses.GetEmployerFeedbackResponse employerFeedback)
        {
            if (employerFeedback == null || employerFeedback.ReviewCount == 0)
            {
                return new GetEmployerFeedbackResponse
                {
                    TotalEmployerResponses = 0,
                    TotalFeedbackRating = 0,
                    FeedbackAttributes = new List<GetEmployerFeedbackAttributeItem>()
                };
            }

            IEnumerable<GetEmployerFeedbackAttributeItem> feedbackAttrItems;

            if (employerFeedback?.ProviderAttribute == null)
            {
                feedbackAttrItems = new List<GetEmployerFeedbackAttributeItem>();
            }
            else
            {
                feedbackAttrItems = employerFeedback.ProviderAttribute
                    .Where(c => c.Strength + c.Weakness != 0)
                    .Select(c => (GetEmployerFeedbackAttributeItem)c).ToList();
            }

            return new GetEmployerFeedbackResponse
            {
                TotalFeedbackRating = employerFeedback.Stars,
                TotalEmployerResponses = employerFeedback.ReviewCount,
                FeedbackAttributes = feedbackAttrItems,
            };
        }

        protected GetApprenticeFeedbackResponse ApprenticeFeedbackResponse(
            InnerApi.Responses.GetApprenticeFeedbackResponse apprenticeFeedback)
        {
            if (apprenticeFeedback == null || apprenticeFeedback.ReviewCount == 0)
            {
                return new GetApprenticeFeedbackResponse
                {
                    TotalApprenticeResponses = 0,
                    TotalFeedbackRating = 0,
                    FeedbackAttributes = new List<GetApprenticeFeedbackAttributeItem>(),
                };
            }

            IEnumerable<GetApprenticeFeedbackAttributeItem> feedbackAttrItems;
            if (apprenticeFeedback?.ProviderAttribute == null)
            {
                feedbackAttrItems = new List<GetApprenticeFeedbackAttributeItem>();
            }
            else
            {
                feedbackAttrItems = apprenticeFeedback.ProviderAttribute
                    .Where(c => c.Agree + c.Disagree != 0)
                    .Select(c => (GetApprenticeFeedbackAttributeItem)c).ToList();
            }

            return new GetApprenticeFeedbackResponse
            {
                TotalFeedbackRating = apprenticeFeedback.Stars,
                TotalApprenticeResponses = apprenticeFeedback.ReviewCount,
                FeedbackAttributes = feedbackAttrItems,
            };
        }

        protected List<GetDeliveryType> FilterDeliveryModes(IEnumerable<DeliveryModel> deliveryModels)
        {
            var deliveryTypes = new List<GetDeliveryType>();
            var hasNational = false;
            var hasRegional = false;
            var hasDayRelease = false;
            var hasBlockRelease = false;

            if (deliveryModels == null || deliveryModels.Any() == false)
            {
                deliveryTypes.Add(
                    new GetDeliveryType
                    {
                        DeliveryModeType = DeliveryModeType.NotFound
                    });

                return deliveryTypes;
            }

            foreach (var deliveryModel in deliveryModels.OrderBy(c => c.DistanceInMiles))
            {
                var deliveryType = new GetDeliveryType
                {
                    Address1 = deliveryModel.Address1,
                    Address2 = deliveryModel.Address2,
                    Town = deliveryModel.Town,
                    County = deliveryModel.County,
                    Postcode = deliveryModel.Postcode,
                    DistanceInMiles = 0
                };

                if (deliveryModel.DistanceInMiles.HasValue)
                    deliveryType.DistanceInMiles = deliveryModel.DistanceInMiles.Value;

                if (!hasNational)
                {
                    if (deliveryModel.LocationType == LocationType.National)
                    {
                        deliveryType.National = true;
                        deliveryType.DeliveryModeType = DeliveryModeType.Workplace;
                        deliveryType.DistanceInMiles = 0m;
                        hasNational = true;
                        deliveryTypes.Add(deliveryType);
                    }
                }

                if (!hasRegional)
                {
                    if (deliveryModel.LocationType == LocationType.Regional)
                    {
                        deliveryType.National = false;
                        deliveryType.DeliveryModeType = DeliveryModeType.Workplace;
                        deliveryType.DistanceInMiles = 0m;
                        hasRegional = true;
                        deliveryTypes.Add(deliveryType);
                    }
                }

                switch (deliveryModel.LocationType)
                {
                    case LocationType.Provider:
                        {
                            deliveryType.National = false;
                            if (deliveryModel.DayRelease is true && !hasDayRelease)
                            {
                                deliveryType.DeliveryModeType = DeliveryModeType.DayRelease;
                                hasDayRelease = true;
                                deliveryTypes.Add(deliveryType);
                            }

                            if (deliveryModel.BlockRelease is true && !hasBlockRelease)
                            {
                                deliveryType.DeliveryModeType = DeliveryModeType.BlockRelease;
                                hasBlockRelease = true;
                                deliveryTypes.Add(deliveryType);
                            }

                            break;
                        }
                }
            }

            return deliveryTypes;

        }
    }
}