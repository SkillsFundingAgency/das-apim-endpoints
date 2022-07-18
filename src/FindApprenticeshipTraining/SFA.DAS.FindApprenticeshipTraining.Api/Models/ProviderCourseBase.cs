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

        protected GetEmployerFeedbackResponse EmployerFeedbackResponse(InnerApi.Responses.GetEmployerFeedbackItem employerFeedback)
        {
            if (employerFeedback?.FeedbackRatings == null)
            {
                return new GetEmployerFeedbackResponse
                {
                    TotalEmployerResponses = 0,
                    TotalFeedbackRating = 0,
                    FeedbackAttributes = new List<GetEmployerFeedbackAttributeItem>(),
                    FeedbackDetail = new List<GetEmployerFeedbackItem>()
                };
            }

            var feedbackRatingItems = employerFeedback.FeedbackRatings.ToList();
            var totalRatings = feedbackRatingItems.Sum(c => c.FeedbackCount);

            var ratingScore = GetRatingScore(feedbackRatingItems.Select(s => (Rating: s.FeedbackName, Score: s.FeedbackCount)));

            var ratingAverage = Math.Round((double)ratingScore / totalRatings, 1);

            var ratingResponse = GetOverallEmployerRatingResponse(ratingAverage);

            var feedbackAttrItems = employerFeedback.FeedbackAttributes
                .Where(c => c.Strength + c.Weakness != 0)
                .Select(c => (GetEmployerFeedbackAttributeItem)c).ToList();

            return new GetEmployerFeedbackResponse
            {
                TotalFeedbackRating = ratingResponse,
                TotalEmployerResponses = totalRatings,
                FeedbackDetail = feedbackRatingItems.Select(c => (GetEmployerFeedbackItem)c).ToList(),
                FeedbackAttributes = feedbackAttrItems,
            };
        }

        protected GetApprenticeFeedbackResponse ApprenticeFeedbackResponse(InnerApi.Responses.GetApprenticeFeedbackResponse apprenticeFeedback)
        {
            if (apprenticeFeedback?.ReviewCount == 0)
            {
                return new GetApprenticeFeedbackResponse
                {
                    TotalApprenticeResponses = 0,
                    TotalFeedbackRating = 0,
                    FeedbackAttributes = new List<GetApprenticeFeedbackAttributeItem>(),
                };
            }
                        
            var feedbackAttrItems = apprenticeFeedback.ProviderAttribute
                .Where(c => c.Agree + c.Disagree != 0)
                .Select(c => (GetApprenticeFeedbackAttributeItem)c).ToList();

            return new GetApprenticeFeedbackResponse
            {
                TotalFeedbackRating = apprenticeFeedback.Stars,
                TotalApprenticeResponses = apprenticeFeedback.ReviewCount,
                FeedbackAttributes = feedbackAttrItems,
            };
        }


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

        private static int GetRatingScore(IEnumerable<(string Rating, int Count)> feedbackRatingItems)
        {
            var ratingScore = 0;
            foreach (var feedbackRatingItem in feedbackRatingItems)
            {
                switch (feedbackRatingItem.Rating.ToLower())
                {
                    case "very poor":
                    case "verypoor":
                        ratingScore += feedbackRatingItem.Count * 1;
                        break;
                    case "poor":
                        ratingScore += feedbackRatingItem.Count * 2;
                        break;
                    case "good":
                        ratingScore += feedbackRatingItem.Count * 3;
                        break;
                    case "excellent":
                        ratingScore += feedbackRatingItem.Count * 4;
                        break;
                }
            }

            return ratingScore;
        }

        private static int GetOverallEmployerRatingResponse(double ratingAverage)
        {
            var ratingResponse = 0;
            if (ratingAverage >= 1 && ratingAverage < 1.3)
            {
                ratingResponse = 1;
            }
            else if (ratingAverage >= 1.3 && ratingAverage < 2.3)
            {
                ratingResponse = 2;
            }
            else if (ratingAverage >= 2.3 && ratingAverage < 3.3)
            {
                ratingResponse = 3;
            }
            else if (ratingAverage >= 3.3 && ratingAverage <= 4)
            {
                ratingResponse = 4;
            }

            return ratingResponse;
        }
    }
}