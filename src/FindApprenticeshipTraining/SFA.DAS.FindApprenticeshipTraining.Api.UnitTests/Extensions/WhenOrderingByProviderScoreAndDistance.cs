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
                CreateGetTrainingCourseProviderListItem(1, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 0},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 0}, 0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m } } ),

                CreateGetTrainingCourseProviderListItem(2, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 1},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 1},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m } } ),

                CreateGetTrainingCourseProviderListItem(3, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 5.1m } } ),

                CreateGetTrainingCourseProviderListItem(4, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 10.1m } } ),

                CreateGetTrainingCourseProviderListItem(5, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 15.1m } } ),

                CreateGetTrainingCourseProviderListItem(6, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 100.1m } } ),

                CreateGetTrainingCourseProviderListItem(7, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 20.1m } } ),

                CreateGetTrainingCourseProviderListItem(8, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 80.1m } } ),

                CreateGetTrainingCourseProviderListItem(9, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 25.1m } } ),

                CreateGetTrainingCourseProviderListItem(10, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 30.1m } } ),

                CreateGetTrainingCourseProviderListItem(11, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 40.1m } } ),

                CreateGetTrainingCourseProviderListItem(12, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 90.1m } } ),

                CreateGetTrainingCourseProviderListItem(13, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 50.1m } } ),

                CreateGetTrainingCourseProviderListItem(14, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 60.1m } } ),

                CreateGetTrainingCourseProviderListItem(15, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 70.1m } } )
            };

            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(1);
            list.Skip(1).Take(1).First().ProviderId.Should().Be(3);
            list.Skip(2).Take(1).First().ProviderId.Should().Be(2);
            list.Skip(3).Take(1).First().ProviderId.Should().Be(4);
            list.Skip(4).Take(1).First().ProviderId.Should().Be(5);
            list.Skip(5).Take(1).First().ProviderId.Should().Be(7);
            list.Skip(6).Take(1).First().ProviderId.Should().Be(9);
            list.Skip(7).Take(1).First().ProviderId.Should().Be(10);
            list.Skip(8).Take(1).First().ProviderId.Should().Be(11);
            list.Skip(9).Take(1).First().ProviderId.Should().Be(13);
            list.Skip(10).Take(1).First().ProviderId.Should().Be(14);
            list.Skip(11).Take(1).First().ProviderId.Should().Be(15);
            list.Skip(12).Take(1).First().ProviderId.Should().Be(8);
            list.Skip(13).Take(1).First().ProviderId.Should().Be(12);
            list.Last().ProviderId.Should().Be(6);
        }

        [Test]
        public void Then_Delivery_Type_Of_Workplace_Counts_As_Zero_Distance()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                CreateGetTrainingCourseProviderListItem(1, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 0},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 0},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m } } ),

                CreateGetTrainingCourseProviderListItem(2, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 1},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 1},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m } } ),

                CreateGetTrainingCourseProviderListItem(3, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 0m} } ),

                CreateGetTrainingCourseProviderListItem(4, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 1m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 0m} } ),

                CreateGetTrainingCourseProviderListItem(5, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 1},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 1},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 0m} } )
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
                CreateGetTrainingCourseProviderListItem(1, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 3.9m } } ),

                CreateGetTrainingCourseProviderListItem(3, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 2},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 2},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 0m } } ),

                CreateGetTrainingCourseProviderListItem(2, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 0m} } )
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
                CreateGetTrainingCourseProviderListItem(1, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 3.9m } } ),

                CreateGetTrainingCourseProviderListItem(3, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 0m } } ),

                CreateGetTrainingCourseProviderListItem(2, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 0m} } )
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
                CreateGetTrainingCourseProviderListItem(1, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 1.2m} } ),

                CreateGetTrainingCourseProviderListItem(3, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4 },0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 1.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 3m} } ),

                CreateGetTrainingCourseProviderListItem(2, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.8m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 0m} } )
            };

            list = list.OrderByProviderScore(new List<DeliveryModeType> { DeliveryModeType.DayRelease, DeliveryModeType.BlockRelease }).ToList();

            list.First().ProviderId.Should().Be(2);
            list.Skip(1).First().ProviderId.Should().Be(1);
            list.Last().ProviderId.Should().Be(3);
        }

        [Test]
        public void Then_If_There_Is_A_Delivery_Filter_For_DayRelease_Then_That_Mode_Is_Used_For_Distance()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                CreateGetTrainingCourseProviderListItem(1, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 1.2m} } ),

                CreateGetTrainingCourseProviderListItem(3, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 1.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 3m} } ),

                CreateGetTrainingCourseProviderListItem(2, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.8m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 0m} } )
            };

            list = list.OrderByProviderScore(new List<DeliveryModeType> { DeliveryModeType.DayRelease }).ToList();

            list.First().ProviderId.Should().Be(3);
            list.Skip(1).First().ProviderId.Should().Be(2);
            list.Last().ProviderId.Should().Be(1);
        }

        [Test]
        public void Then_If_There_Is_A_Delivery_Filter_For_Block_Release_Then_That_Mode_Is_Used_For_Distance()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                CreateGetTrainingCourseProviderListItem(1, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 1.2m} } ),

                CreateGetTrainingCourseProviderListItem(3, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 1.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 3m} } ),

                CreateGetTrainingCourseProviderListItem(2, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 0m} } )
            };

            list = list.OrderByProviderScore(new List<DeliveryModeType> { DeliveryModeType.BlockRelease }).ToList();

            list.First().ProviderId.Should().Be(2);
            list.Skip(1).First().ProviderId.Should().Be(1);
            list.Last().ProviderId.Should().Be(3);
        }

        [Test]
        public void Then_If_There_Is_A_Delivery_Filter_For_At_Workplace_With_National_Then_Distance_Is_Not_Used()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                CreateGetTrainingCourseProviderListItem(1, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 2},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 2},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 4.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 0m} } ),

                CreateGetTrainingCourseProviderListItem(3, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 3},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 1.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 0m} } ),

                CreateGetTrainingCourseProviderListItem(2, null, new GetEmployerFeedbackResponse{TotalFeedbackRating = 4},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 4},0, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.DayRelease, DistanceInMiles = 5.9m },
                        new GetDeliveryType{DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 0m} } )
            };

            list = list.OrderByProviderScore(new List<DeliveryModeType> { DeliveryModeType.Workplace, DeliveryModeType.National }).ToList();

            list.First().ProviderId.Should().Be(2);
            list.Skip(1).First().ProviderId.Should().Be(3);
            list.Last().ProviderId.Should().Be(1);
        }

        [Test]
        public void Then_If_All_Are_At_Workplace_Then_Not_Grouped_By_Distance()
        {
            var list = new List<GetTrainingCourseProviderListItem>
            {
                CreateGetTrainingCourseProviderListItem(
                    1, "Third", new GetEmployerFeedbackResponse{TotalFeedbackRating = 3, TotalEmployerResponses = 100},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3, TotalApprenticeResponses = 100}, 100, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 10m } }
                    ),

                CreateGetTrainingCourseProviderListItem(
                    4, "Second", new GetEmployerFeedbackResponse{TotalFeedbackRating = 3, TotalEmployerResponses = 100},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3, TotalApprenticeResponses = 100},100, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 15m } }
                    ),

                CreateGetTrainingCourseProviderListItem(
                    3, "First", new GetEmployerFeedbackResponse{TotalFeedbackRating = 3, TotalEmployerResponses = 100},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3, TotalApprenticeResponses = 100},100, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.Workplace, DistanceInMiles = 100m } }
                    )
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
                CreateGetTrainingCourseProviderListItem(
                    1, "Test", new GetEmployerFeedbackResponse{TotalFeedbackRating = 3, TotalEmployerResponses = 100},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3, TotalApprenticeResponses = 100 }, 10, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 11m } }
                ),

                CreateGetTrainingCourseProviderListItem(
                    4, "Test", new GetEmployerFeedbackResponse{TotalFeedbackRating = 3, TotalEmployerResponses = 100},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3, TotalApprenticeResponses = 100 },100, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 11m } }
                ),

                CreateGetTrainingCourseProviderListItem(
                    3, "Test", new GetEmployerFeedbackResponse{TotalFeedbackRating = 3, TotalEmployerResponses = 100},
                    new GetApprenticeFeedbackResponse{ TotalFeedbackRating = 3, TotalApprenticeResponses = 100 },1000, true,
                    new List<GetDeliveryType>{new GetDeliveryType{ DeliveryModeType = DeliveryModeType.BlockRelease, DistanceInMiles = 11m } }
                )
            };

            list = list.OrderByProviderScore().ToList();

            list.First().ProviderId.Should().Be(3);
            list.Skip(1).Take(1).First().ProviderId.Should().Be(4);
            list.Last().ProviderId.Should().Be(1);
        }

        private GetTrainingCourseProviderListItem CreateGetTrainingCourseProviderListItem(int providerId, string name,
            GetEmployerFeedbackResponse employerFeedback, GetApprenticeFeedbackResponse apprenticeFeedback, int overallCohort, bool hasLocation,
            List<GetDeliveryType> deliveryModes)
        {
            if (name != null && overallCohort != 0)
            {
                return new GetTrainingCourseProviderListItem
                {
                    ProviderId = providerId,
                    Name = name,
                    EmployerFeedback = employerFeedback,
                    ApprenticeFeedback = apprenticeFeedback,
                    OverallCohort = overallCohort,
                    HasLocation = hasLocation,
                    DeliveryModes = deliveryModes
                };
            }

            return new GetTrainingCourseProviderListItem
            {
                ProviderId = providerId,
                EmployerFeedback = employerFeedback,
                ApprenticeFeedback = apprenticeFeedback,
                HasLocation = hasLocation,
                DeliveryModes = deliveryModes
            };
        }
    }
}