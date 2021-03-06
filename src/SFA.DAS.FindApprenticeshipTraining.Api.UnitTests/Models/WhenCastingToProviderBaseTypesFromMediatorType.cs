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
        public void Then_Maps_Fields_Appropriately_Matching_AchievementRate(
            string sectorSubjectArea,
            GetProvidersListItem source, GetAchievementRateItem item, GetAchievementRateItem item2)
        {
            item.SectorSubjectArea = sectorSubjectArea;
            item.Level = "Two";
            source.AchievementRates = new List<GetAchievementRateItem>
            {
               item,
               item2
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,2, null, null, true);

            response.Name.Should().Be(source.Name);
            response.ProviderId.Should().Be(source.Ukprn);
            response.OverallCohort.Should().Be(item.OverallCohort);
            response.HasLocation.Should().BeTrue();
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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, null, null, true);

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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

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
                    DistanceInMiles = 2.5m,
                    National = true
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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

            response.DeliveryModes.Count.Should().Be(3);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == DeliveryModeType.BlockRelease)?.DistanceInMiles.Should().Be(3.1m);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == DeliveryModeType.DayRelease)?.DistanceInMiles.Should().Be(2.5m);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == DeliveryModeType.Workplace)?.DistanceInMiles.Should().Be(0m);
            response.DeliveryModes.FirstOrDefault().National.Should().BeTrue();
        }
        
        [Test]
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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

            response.DeliveryModes.Count.Should().Be(1);
            response.DeliveryModes.FirstOrDefault(c => c.DeliveryModeType == deliveryModeType)?.DistanceInMiles.Should().Be(2.5m);
            response.DeliveryModes.FirstOrDefault().National.Should().BeFalse();
        }

        [Test, AutoData]
        public void Then_Maps_All_DeliveryType_Fields_And_Sets_At_WorkPlace_Distance_To_Zero(string sectorSubjectArea, GetProvidersListItem source, GetDeliveryTypeItem item)
        {
            source.AchievementRates = null;
            item.DeliveryModes = "100PercentEmployer";
            source.DeliveryTypes = new List<GetDeliveryTypeItem>{item};
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);
            
            response.DeliveryModes.First().Should().BeEquivalentTo(item, options => options.Excluding(c=>c.DeliveryModes).Excluding(c=>c.DistanceInMiles));
            response.DeliveryModes.First().DeliveryModeType.Should().Be(DeliveryModeType.Workplace);
            response.DeliveryModes.First().DistanceInMiles.Should().Be(0m);
        }

        [Test, AutoData]
        public void Then_Maps_Zero_If_No_Feedback(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackRatings = null;
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

            response.Feedback.TotalEmployerResponses.Should().Be(0);
            response.Feedback.TotalFeedbackRating.Should().Be(0);
        }

        [Test, AutoData]
        public void Then_Maps_FeedbackDetail(GetProvidersListItem source, string sectorSubjectArea)
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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

            response.Feedback.FeedbackDetail.Should().BeEquivalentTo(source.FeedbackRatings);
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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

            response.Feedback.TotalEmployerResponses.Should().Be(6);
            response.Feedback.TotalFeedbackRating.Should().Be(4);
        }

        [Test, AutoData]
        public void Then_Maps_Not_Found_Delivery_Mode(string sectorSubjectArea, GetProvidersListItem source)
        {
            var deliveryTypeItem = new GetDeliveryTypeItem{DeliveryModes = "NotFound"};
            source.DeliveryTypes = new List<GetDeliveryTypeItem>{deliveryTypeItem};
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

            response.DeliveryModes.First().DeliveryModeType.Should().Be(DeliveryModeType.NotFound);
            response.DeliveryModes.First().Address1.Should().BeNullOrEmpty();
            response.DeliveryModes.First().Address2.Should().BeNullOrEmpty();
            response.DeliveryModes.First().County.Should().BeNullOrEmpty();
            response.DeliveryModes.First().Postcode.Should().BeNullOrEmpty();
            response.DeliveryModes.First().Town.Should().BeNullOrEmpty();
            response.DeliveryModes.First().DistanceInMiles.Should().Be(0);
        }

        [Test, AutoData]
        public void Then_If_Delivery_Modes_Are_Passed_The_Results_Are_Filtered(string sectorSubjectArea, GetProvidersListItem source)
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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>
            {
                DeliveryModeType.DayRelease
            }, new List<FeedbackRatingType>(), true);

            response.DeliveryModes.Count.Should().Be(3);
            response.DeliveryModes.Should().Contain(c => c.DeliveryModeType == DeliveryModeType.DayRelease);
        }

        [Test, AutoData]
        public void Then_If_There_Are_Multiple_Delivery_Modes_Filtered_And_Not_Match_Then_It_Is_Returned_Correctly(string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>
            {
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "100PercentEmployer|DayRelease",
                    DistanceInMiles = 2.5m
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>
            {
                DeliveryModeType.DayRelease,
                DeliveryModeType.BlockRelease
            }, new List<FeedbackRatingType>(), true);

            response.Should().NotBeNull();
        }
        [Test, AutoData]
        public void Then_If_There_Are_Multiple_Delivery_Modes_Filtered_And_Match_Then_It_Is_Returned_Correctly(string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>
            {
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "100PercentEmployer|DayRelease",
                    DistanceInMiles = 2.5m
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>
            {
                DeliveryModeType.DayRelease,
                DeliveryModeType.Workplace
            }, new List<FeedbackRatingType>(), true);

            response.DeliveryModes.Count.Should().Be(2);
        }

        [Test, AutoData]
        public void The_If_There_Are_Delivery_Modes_To_Filter_And_Returns_No_Delivery_Modes_After_Filter_Then_Null_Returned(string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>
            {
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "BlockRelease",
                    DistanceInMiles = 5.5m
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>
            {
                DeliveryModeType.DayRelease
            }, new List<FeedbackRatingType>(), true);

            response.Should().BeNull();
        }

        [Test, AutoData]
        public void Then_If_National_At_Workplace_Is_Selected_As_Delivery_Mode_Filter_Then_Null_Returned_If_No_National_At_Workplace(string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>
            {
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "100PercentEmployer",
                    DistanceInMiles = 2.5m,
                    National = false
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>
            {
                DeliveryModeType.Workplace,
                DeliveryModeType.National
            }, new List<FeedbackRatingType>(), true);

            response.Should().BeNull();
        }
        
        [Test, AutoData]
        public void Then_If_National_At_Workplace_Is_Selected_As_Delivery_Mode_Filter_Then_Not_Null_Returned_If_National_At_Workplace(string sectorSubjectArea, GetProvidersListItem source)
        {
            source.AchievementRates = null;
            source.DeliveryTypes = new List<GetDeliveryTypeItem>
            {
                new GetDeliveryTypeItem
                {
                    DeliveryModes = "100PercentEmployer",
                    DistanceInMiles = 2.5m,
                    National = true
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1, new List<DeliveryModeType>
            {
                DeliveryModeType.Workplace,
                DeliveryModeType.National
            }, new List<FeedbackRatingType>(), true);

            response.Should().NotBeNull();
        }

        [Test, AutoData]
        public void Then_If_There_Are_Ratings_To_Filter_Then_Matches_On_Values(string sectorSubjectArea, GetProvidersListItem source)
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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>{FeedbackRatingType.Poor}, true);

            response.Should().BeNull();

        }
        
        
        [Test, AutoData]
        public void Then_If_There_Are_Multiple_Ratings_To_Filter_Then_Matches_On_Values(string sectorSubjectArea, GetProvidersListItem source)
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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>{FeedbackRatingType.Poor,FeedbackRatingType.Excellent}, true);

            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());

        }
    }
}