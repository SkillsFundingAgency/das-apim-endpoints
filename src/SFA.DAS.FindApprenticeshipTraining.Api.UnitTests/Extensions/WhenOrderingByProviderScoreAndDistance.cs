using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Extensions;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Extensions
{
    public class WhenOrderingByProviderScoreAndDistance
    {
        [Test]
        public void Then_If_There_Is_A_Location_Then_Scored_And_Sorted_By_Distance()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 0
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 1
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 5.1m
                        }
                    } ,
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 15.1m
                        }
                    },
                    HasLocation = true
                }
            };
            
            list = list.OrderByProviderScore().ToList();
            
            list.First().ProviderId.Should().Be(1);
            list.Skip(1).Take(1).First().ProviderId.Should().Be(3);
            list.Skip(2).Take(1).First().ProviderId.Should().Be(2);
            list.Last().ProviderId.Should().Be(4);
        }
        
        [Test]
        public void Then_Delivery_Type_Of_Workplace_Counts_As_Zero_Distance()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 0
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 1
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 1.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 0m
                        }
                    } ,
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 1m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 5,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 1
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                }
            };
            
            list = list.OrderByProviderScore().ToList();
            
            list.First().ProviderId.Should().Be(3);
            list.Skip(1).Take(1).First().ProviderId.Should().Be(4);
            list.Skip(2).Take(1).First().ProviderId.Should().Be(1);
            list.Skip(3).Take(1).First().ProviderId.Should().Be(5);
            list.Last().ProviderId.Should().Be(2);
        }

        [Test]
        public void Then_Ordered_By_Score_In_Grouped_Distance()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 3.9m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 2
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                }
            };
            
            list = list.OrderByProviderScore().ToList();
            
            list.First().ProviderId.Should().Be(1);
            list.Skip(1).First().ProviderId.Should().Be(2);
            list.Last().ProviderId.Should().Be(3);
        }
        
        [Test]
        public void Then_If_Matched_In_Group_Then_Ordered_By_Closest_Excluding_Workplace_If_Other_Delivery_Options()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 3.9m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                }
            };
            
            list = list.OrderByProviderScore().ToList();
            
            list.First().ProviderId.Should().Be(3);
            list.Skip(1).First().ProviderId.Should().Be(1);
            list.Last().ProviderId.Should().Be(2);
        }

        [Test]
        public void Then_If_There_Are_Multiple_Filters_Then_Closest_Is_Used()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 1.2m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 1.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 3m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.8m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                }
            };
            
            list = list.OrderByProviderScore(new List<DeliveryModeType> {DeliveryModeType.DayRelease, DeliveryModeType.BlockRelease}).ToList();
            
            list.First().ProviderId.Should().Be(2);
            list.Skip(1).First().ProviderId.Should().Be(1);
            list.Last().ProviderId.Should().Be(3);
        }
        
        [Test]
        public void Then_If_There_Is_A_Delivery_Filter_For_DayRelease_Then_That_Mode_Is_Used_For_Distance()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 1.2m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 1.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 3m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.8m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                }
            };
            
            list = list.OrderByProviderScore(new List<DeliveryModeType> {DeliveryModeType.DayRelease}).ToList();
            
            list.First().ProviderId.Should().Be(3);
            list.Skip(1).First().ProviderId.Should().Be(2);
            list.Last().ProviderId.Should().Be(1);
        }
        
        [Test]
        public void Then_If_There_Is_A_Delivery_Filter_For_Block_Release_Then_That_Mode_Is_Used_For_Distance()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 1.2m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 1.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 3m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                }
            };
            
            list = list.OrderByProviderScore(new List<DeliveryModeType> {DeliveryModeType.BlockRelease}).ToList();
            
            list.First().ProviderId.Should().Be(2);
            list.Skip(1).First().ProviderId.Should().Be(1);
            list.Last().ProviderId.Should().Be(3);
        }

        [Test]
        public void Then_If_There_Is_A_Delivery_Filter_For_At_Workplace_With_National_Then_Distance_Is_Not_Used()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 2
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 4.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 1.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 2,
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 4
                    },
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.DayRelease,
                            DistanceInMiles = 5.9m
                        },
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 0m
                        }
                    },
                    HasLocation = true
                }
            };
            
            list = list.OrderByProviderScore(new List<DeliveryModeType> {DeliveryModeType.Workplace, DeliveryModeType.National}).ToList();
            
            list.First().ProviderId.Should().Be(2);
            list.Skip(1).First().ProviderId.Should().Be(3);
            list.Last().ProviderId.Should().Be(1);
        }
        
        [Test]
        public void Then_If_All_Are_At_Workplace_Then_Not_Grouped_By_Distance()
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
                    OverallCohort = 100,
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 10
                        }
                    }
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
                    OverallCohort = 100,
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 15
                        }
                    }
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
                    OverallCohort = 100,
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.Workplace,
                            DistanceInMiles = 100
                        }
                    }
                }
            };
            
            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(3);
            list.Skip(1).Take(1).First().ProviderId.Should().Be(4);
            list.Last().ProviderId.Should().Be(1);
        }
        
        [Test]
        public void Then_If_All_Are_Same_Score_And_Distance_Then_Ordered_By_OverAllCohort()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 1,
                    Name = "Test",
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3,
                        TotalEmployerResponses = 100
                    },
                    OverallCohort = 10,
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 11
                        }
                    }
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 4,
                    Name = "Test",
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3,
                        TotalEmployerResponses = 100
                    },
                    OverallCohort = 100,
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 11
                        }
                    }
                },
                new GetTrainingCourseProviderListItem
                {
                    ProviderId = 3,
                    Name = "Test",
                    Feedback = new GetProviderFeedbackResponse
                    {
                        TotalFeedbackRating = 3,
                        TotalEmployerResponses = 100
                    },
                    OverallCohort = 1000,
                    HasLocation = true,
                    DeliveryModes = new List<GetDeliveryType>
                    {
                        new GetDeliveryType
                        {
                            DeliveryModeType = DeliveryModeType.BlockRelease,
                            DistanceInMiles = 11
                        }
                    }
                }
            };
            
            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(3);
            list.Skip(1).Take(1).First().ProviderId.Should().Be(4);
            list.Last().ProviderId.Should().Be(1);
        }
    }
}