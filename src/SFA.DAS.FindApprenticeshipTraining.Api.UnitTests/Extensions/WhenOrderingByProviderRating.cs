using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Extensions;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Extensions
{
    public class WhenOrderingByProviderRating
    {
        [Test]
        public void Then_The_FeedbackRating_Is_Used_To_Order()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 0
                    } 
                        
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 2
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 5,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 1
                    } 
                }
            };
            
            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(4);
            list.Skip(1).Take(1).First().ProviderId.Should().Be(3);
            list.Skip(2).Take(1).First().ProviderId.Should().Be(1);
            list.Skip(3).Take(1).First().ProviderId.Should().Be(2);
            list.Last().ProviderId.Should().Be(5);
        }

        [Test]
        public void Then_If_Only_One_Has_Percentile_Score_Then_That_Scores_Highest()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    OverallAchievementRate = 84.8m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                }
            };

            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(3);
        }
        
        [Test]
        public void Then_The_Percentile_Is_Calculated_And_Rank_Assigned_For_AchievementRates()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    OverallAchievementRate = 72.6m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    OverallAchievementRate = 81.6m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    OverallAchievementRate = 84.8m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    OverallAchievementRate = 81.8m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 6,
                    OverallAchievementRate = 81.8m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 5,
                    OverallAchievementRate = null,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                }
            };

            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(3);
            list.Last().ProviderId.Should().Be(1);
        }
        
        
        [Test]
        public void Then_The_Percentile_Is_Calculated_And_Rank_Assigned_For_AchievementRates_And_If_Same_Ordered_By_PassRate()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    OverallAchievementRate = 52.6m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    OverallAchievementRate = 81.6m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    OverallAchievementRate = 84.8m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    OverallAchievementRate = 81.8m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 7,
                    OverallAchievementRate = 81.8m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 6,
                    OverallAchievementRate = 82.8m,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 5,
                    OverallAchievementRate = null,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        
                    } 
                }
            };

            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(3);
            list.Last().ProviderId.Should().Be(1);
        }
        
        [Test]
        public void Then_The_Percentile_Is_Calculated_And_Rank_Assigned_For_AchievementRates_And_If_Same_Ordered_By_PassRate_With_Distance()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    OverallAchievementRate = 52.6m,
                    Feedback = new GetProviderFeedbackResponse(),
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 1
                        }
                    }
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    OverallAchievementRate = 81.6m,
                    Feedback = new GetProviderFeedbackResponse(),
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 1
                        }
                    }
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    OverallAchievementRate = 84.8m,
                    Feedback = new GetProviderFeedbackResponse(),
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 1
                        }
                    }
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    OverallAchievementRate = 81.8m,
                    Feedback = new GetProviderFeedbackResponse(),
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 1
                        }
                    }
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 7,
                    OverallAchievementRate = 81.8m,
                    Feedback = new GetProviderFeedbackResponse(),
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 1
                        }
                    }
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 6,
                    OverallAchievementRate = 82.8m,
                    Feedback = new GetProviderFeedbackResponse(),
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 1
                        }
                    }
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 5,
                    OverallAchievementRate = null,
                    Feedback = new GetProviderFeedbackResponse(),
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 1
                        }
                    }
                }
            };

            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(3);
            list.Last().ProviderId.Should().Be(1);
        }
        
        [Test]
        public void Then_If_The_Scores_Are_The_Same_Ordered_By_Cohort_Size()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    OverallCohort = 100,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    } 
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    OverallCohort = 1000,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    }  
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    OverallCohort = 10,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    }  
                }
            };
            
            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(4);
            list.Skip(1).Take(1).First().ProviderId.Should().Be(1);
            list.Last().ProviderId.Should().Be(3);
        }
        
        [Test]
        public void Then_If_The_Feedback_Scores_And_Cohorts_Are_The_Same_Ordered_By_Total_Employer_Responses_Size()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Name = "test 1",
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3,
                        TotalEmployerResponses = 100
                    },
                    OverallCohort = 100
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    Name = "test 2",
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3,
                        TotalEmployerResponses = 1000
                    },
                    OverallCohort = 100
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Name = "test 3",
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3,
                        TotalEmployerResponses = 10
                    },
                    OverallCohort = 100
                }
            };
            
            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(4);
            list.Skip(1).Take(1).First().ProviderId.Should().Be(1);
            list.Last().ProviderId.Should().Be(3);
        }
        
        [Test]
        public void Then_If_All_Score_Same_With_No_Location_Then_Order_By_Name()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Name = "Third",
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3,
                        TotalEmployerResponses = 100
                    },
                    OverallCohort = 100
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    Name = "Second",
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3,
                        TotalEmployerResponses = 100
                    },
                    OverallCohort = 100
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Name = "First",
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3,
                        TotalEmployerResponses = 100
                    },
                    OverallCohort = 100
                }
            };
            
            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(3);
            list.Skip(1).Take(1).First().ProviderId.Should().Be(4);
            list.Last().ProviderId.Should().Be(1);
        }
        
        
    }
}