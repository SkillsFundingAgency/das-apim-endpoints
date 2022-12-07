using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

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

        protected GetApprenticeFeedbackResponse ApprenticeFeedbackResponse(InnerApi.Responses.GetApprenticeFeedbackResponse apprenticeFeedback)
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

        //TODO This function should be removed in story CSP-282
        protected List<GetDeliveryType> FilterDeliveryModes(IEnumerable<GetDeliveryTypeItem> getDeliveryTypeItems)
        {
            var hasWorkPlace = false;
            var hasDayRelease = false;
            var hasBlockRelease = false;
            var isNotFound = false;
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
                            item.DistanceInMiles = 0m;
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
                            filterDeliveryModes.Add(item);
                            isNotFound = true;
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


        protected List<GetDeliveryType> FilterDeliveryModes(IEnumerable<DeliveryModel> deliveryModels)
        {
            var deliveryTypes = new List<GetDeliveryType>();

            if (deliveryModels == null || deliveryModels.Any() == false)
            {
                deliveryTypes.Add(
                    new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.NotFound
                        });

                return deliveryTypes;
            }

            foreach (var deliveryModel in deliveryModels)
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

                if (deliveryModel.LocationType == LocationType.National)
                {
                    deliveryType.National = true;
                    deliveryType.DeliveryModeType = DeliveryModeType.Workplace;
                    deliveryTypes.Add(deliveryType);
                }


                if (deliveryModel.LocationType == LocationType.Regional)
                {
                    deliveryType.National = false;
                    deliveryType.DeliveryModeType = DeliveryModeType.Workplace;
                    deliveryTypes.Add(deliveryType);
                }


                if (deliveryModel.LocationType == LocationType.Provider)
                {
                    deliveryType.National = false;
                    if (deliveryModel.DayRelease is true)
                        deliveryType.DeliveryModeType = DeliveryModeType.DayRelease;

                    if (deliveryModel.BlockRelease is true)
                        deliveryType.DeliveryModeType = DeliveryModeType.BlockRelease;
                    deliveryTypes.Add(deliveryType);
                }
            }

            return deliveryTypes;

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
                Town = deliveryTypeItem.Town,
                National = deliveryTypeItem.National
            };
        }
    }
}