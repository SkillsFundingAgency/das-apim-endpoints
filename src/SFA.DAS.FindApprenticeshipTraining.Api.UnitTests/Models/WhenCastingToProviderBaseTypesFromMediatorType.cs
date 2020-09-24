using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingToProviderBaseTypesFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Matching_AchievementRate(string sectorSubjectArea,
            GetProvidersListItem source, GetAchievementRateItem item, GetAchievementRateItem item2)
        {
            item.SectorSubjectArea = sectorSubjectArea;
            item.Level = "Two";
            source.AchievementRates = new List<GetAchievementRateItem>
            {
               item,
               item2
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,2);

            response.Name.Should().Be(source.Name);
            response.ProviderId.Should().Be(source.Ukprn);
            response.OverallCohort.Should().Be(item.OverallCohort);
            response.OverallAchievementRate.Should().Be(item.OverallAchievementRate);

        }
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Returning_Null_For_AchievementRate_Data_If_No_Matching_No_AchievementRate(string sectorSubjectArea,
            GetProvidersListItem source, GetAchievementRateItem item, GetAchievementRateItem item2)
        {
            source.AchievementRates = new List<GetAchievementRateItem>
            {
                item,
                item2
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Name.Should().Be(source.Name);
            response.ProviderId.Should().Be(source.Ukprn);
            response.OverallCohort.Should().BeNull();
            response.OverallAchievementRate.Should().BeNull();

        }
        
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately_Returning_Null_For_AchievementRate_Data_If_No_AchievementRates_And_Empty_List_For_DeliveryModes_If_No_Delivery_Modes(string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>();
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Name.Should().Be(source.Name);
            response.ProviderId.Should().Be(source.Ukprn);
            response.OverallCohort.Should().BeNull();
            response.OverallAchievementRate.Should().BeNull();
            response.DeliveryModes.Should().BeEmpty();

        }

        [Test, AutoData]
        public void Then_Maps_Delivery_Types_Returning_The_Smallest_Distance_Only(string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>
            {
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "100PercentEmployer|DayRelease",
                    DistanceInMiles = 2.5m
                },
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "100PercentEmployer|DayRelease|BlockRelease",
                    DistanceInMiles = 3.1m
                },
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "BlockRelease",
                    DistanceInMiles = 5.5m
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.DeliveryModes.Count.Should().Be(3);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == DeliveryModeType.BlockRelease)?.DistanceInMiles.Should().Be(3.1m);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == DeliveryModeType.DayRelease)?.DistanceInMiles.Should().Be(2.5m);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == DeliveryModeType.Workplace)?.DistanceInMiles.Should().Be(2.5m);
        }
        
        [Test]
        [InlineAutoData("100PercentEmployer",DeliveryModeType.Workplace)]
        [InlineAutoData("BlockRelease",DeliveryModeType.BlockRelease)]
        [InlineAutoData("DayRelease",DeliveryModeType.DayRelease)]
        public void Then_Maps_Delivery_Types_Returning_The_Smallest_Distance_Only_For_One_Type(string deliveryModeString, DeliveryModeType deliveryModeType, string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>
            {
                new GetDeliveryTypeItem
                {
                    DeliveryModes = deliveryModeString,
                    DistanceInMiles = 2.5m
                },
                new GetDeliveryTypeItem
                {
                    DeliveryModes = deliveryModeString,
                    DistanceInMiles = 3.1m
                },
                new GetDeliveryTypeItem
                {
                    DeliveryModes = deliveryModeString,
                    DistanceInMiles = 5.5m
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.DeliveryModes.Count.Should().Be(1);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == deliveryModeType)?.DistanceInMiles.Should().Be(2.5m);
        }

        [Test, AutoData]
        public void Then_Maps_All_DeliveryType_Fields(string sectorSubjectArea, GetProvidersListItem source, GetDeliveryTypeItem item)
        {
            source.AchievementRates = null;
            item.DeliveryModes = "100PercentEmployer";
            source.DeliveryTypes = new List<GetDeliveryTypeItem>{item};
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);
            
            response.DeliveryModes.First().Should().BeEquivalentTo(item, options => options.Excluding(c=>c.DeliveryModes));
        }

        [Test, AutoData]
        public void Then_Maps_Zero_If_No_Feedback(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackRatings = null;
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Feedback.TotalEmployerResponses.Should().Be(0);
            response.Feedback.TotalFeedbackRating.Should().Be(0);
        }

        [Test, AutoData]
        public void Then_Maps_Feedback_Rating_To_A_Score(GetProvidersListItem source, string sectorSubjectArea )
        {
            source.FeedbackRatings = new List<GetFeedbackRatingItem>
            {
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Good",
                    FeedbackCount = 92,
                },
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Excellent",
                    FeedbackCount = 29,
                },
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Poor",
                    FeedbackCount = 7,
                },
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Very Poor",
                    FeedbackCount = 1,
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Feedback.TotalEmployerResponses.Should().Be(129);
            response.Feedback.TotalFeedbackRating.Should().Be(3);
        }

        [Test, AutoData]
        public void Then_Returns_Feedback_Of_One_If_Between_Boundary(GetProvidersListItem source, string sectorSubjectArea )
        {
            source.FeedbackRatings = new List<GetFeedbackRatingItem>
            {
                
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Very Poor",
                    FeedbackCount = 7,
                },
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Poor",
                    FeedbackCount = 2,
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Feedback.TotalEmployerResponses.Should().Be(9);
            response.Feedback.TotalFeedbackRating.Should().Be(1);
        }
        
        [Test, AutoData]
        public void Then_Returns_Feedback_Of_Two_If_Between_Boundary(GetProvidersListItem source, string sectorSubjectArea )
        {
            source.FeedbackRatings = new List<GetFeedbackRatingItem>
            {
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Poor",
                    FeedbackCount = 4,
                },
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Good",
                    FeedbackCount = 1,
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Feedback.TotalEmployerResponses.Should().Be(5);
            response.Feedback.TotalFeedbackRating.Should().Be(2);
        }
        
        [Test, AutoData]
        public void Then_Returns_Feedback_Of_Three_If_Between_Boundary(GetProvidersListItem source, string sectorSubjectArea )
        {
            source.FeedbackRatings = new List<GetFeedbackRatingItem>
            {
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Poor",
                    FeedbackCount = 4,
                },
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Good",
                    FeedbackCount = 2,
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Feedback.TotalEmployerResponses.Should().Be(6);
            response.Feedback.TotalFeedbackRating.Should().Be(3);
        }
        [Test, AutoData]
        public void Then_Returns_Feedback_Of_Four_If_Between_Boundary(GetProvidersListItem source, string sectorSubjectArea )
        {
            source.FeedbackRatings = new List<GetFeedbackRatingItem>
            {
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Good",
                    FeedbackCount = 1,
                },
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Excellent",
                    FeedbackCount = 1,
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Feedback.TotalEmployerResponses.Should().Be(2);
            response.Feedback.TotalFeedbackRating.Should().Be(4);
        }
        
        [Test, AutoData]
        public void Then_Returns_Feedback_Of_Four_If_Max(GetProvidersListItem source, string sectorSubjectArea )
        {
            source.FeedbackRatings = new List<GetFeedbackRatingItem>
            {
                new GetFeedbackRatingItem
                {
                    FeedbackName = "Excellent",
                    FeedbackCount = 6,
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1);

            response.Feedback.TotalEmployerResponses.Should().Be(6);
            response.Feedback.TotalFeedbackRating.Should().Be(4);
        }
    }
}