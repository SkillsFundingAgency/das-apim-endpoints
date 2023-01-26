using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using GetApprenticeFeedbackResponse = SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses.GetApprenticeFeedbackResponse;
using StandardDate = SFA.DAS.SharedOuterApi.InnerApi.Responses.StandardDate;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenCallingGetTrainingCourseProvidersList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Course_And_Providers_From_Mediator(
            int id,
            GetCourseProvidersRequest request,
            GetTrainingCourseProvidersResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            request.DeliveryModes = new List<DeliveryModeType>();
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProvidersQuery>(c =>
                        c.Id.Equals(id)
                        && c.Location.Equals(request.Location)
                        && c.Lat.Equals(request.Lat)
                        && c.Lon.Equals(request.Lon)
                        && c.ShortlistUserId.Equals(request.ShortlistUserId)
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            mediatorResult.Providers.ToList().ForEach(s =>
            {
                s.ApprenticeFeedback.ReviewCount = 0;
                s.ApprenticeFeedback.Stars = 0;
                s.EmployerFeedback.ReviewCount = 0;
                s.EmployerFeedback.Stars = 0;
            });

            var controllerResult = await controller.GetProviders(id, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseProvidersResponse;
            Assert.IsNotNull(model);
            model.TrainingCourse.Should().BeEquivalentTo((GetTrainingCourseListItem)mediatorResult.Course);
            model.TrainingCourseProviders.Should()
                .BeEquivalentTo(mediatorResult.Providers,
                    options => options.Excluding(c => c.Ukprn)
                        .Excluding(c => c.AchievementRates)
                        .Excluding(c => c.DeliveryTypes)
                        .Excluding(c => c.DeliveryModels)
                        .Excluding(c => c.EmployerFeedback)
                        .Excluding(c => c.ApprenticeFeedback)
                        .Excluding(c => c.DeliveryModelsShortestDistance)
                        .Excluding(c=>c.IsApprovedByRegulator)
                );
            model.Total.Should().Be(mediatorResult.Total);
            model.Location.Location.GeoPoint.Should().BeEquivalentTo(mediatorResult.Location.GeoPoint);
            model.Location.Name.Should().Be(mediatorResult.Location.Name);
            model.ShortlistItemCount.Should().Be(mediatorResult.ShortlistItemCount);
        }

        [Test, MoqAutoData]
        public async Task Then_Nulls_Are_Filtered_Out_From_The_Providers_List_And_The_Filtered_Count_Returned(
            int id,
            GetCourseProvidersRequest request,
            GetProvidersListItem provider1,
            GetProvidersListItem provider2,
            GetTrainingCourseProvidersResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            request.DeliveryModes = new List<DeliveryModeType>
            {
                DeliveryModeType.Workplace
            };

            provider1.DeliveryTypes = null;
            provider1.DeliveryModels = provider1.DeliveryModels.Select(c =>
            {
                c.LocationType = LocationType.Regional;
                c.BlockRelease = false;
                c.DayRelease = false;
                return c;
            }).ToList();
            provider1.ApprenticeFeedback.ReviewCount = 0;
            provider1.ApprenticeFeedback.Stars = 0;
            provider1.EmployerFeedback.ReviewCount = 0;
            provider1.EmployerFeedback.Stars = 0;

            provider2.DeliveryTypes = null;
            provider2.DeliveryModels = provider2.DeliveryModels.Select(c =>
            {
                c.LocationType = LocationType.Provider;
                c.DayRelease = true;
                c.BlockRelease = false;
                return c;
            }).ToList();
            provider2.ApprenticeFeedback.ReviewCount = 0;
            provider2.ApprenticeFeedback.Stars = 0;
            provider2.EmployerFeedback.ReviewCount = 0;
            provider2.EmployerFeedback.Stars = 0;
            mediatorResult.Providers = new List<GetProvidersListItem> { provider1, provider2 };
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProvidersQuery>(c => c.Id.Equals(id)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviders(id, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseProvidersResponse;
            Assert.IsNotNull(model);
            model.TrainingCourseProviders.Count().Should().Be(1);
            model.Total.Should().Be(mediatorResult.Total);
            model.TotalFiltered.Should().Be(model.TrainingCourseProviders.Count());
        }

        [Test, MoqAutoData]
        public async Task Then_EmployerFeedbackRating_Filter_Is_Applied(
            int id,
            GetCourseProvidersRequest request,
            string location,
            GetProvidersListItem provider1,
            GetProvidersListItem provider2,
            GetProvidersListItem provider3,
            GetTrainingCourseProvidersResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            request.EmployerProviderRatings = new List<FeedbackRatingType>
            {
                FeedbackRatingType.Excellent,
                FeedbackRatingType.Good
            };
            request.ApprenticeProviderRatings = new List<FeedbackRatingType>();
            request.DeliveryModes = null;
            provider1.EmployerFeedback.ReviewCount = 3;
            provider1.EmployerFeedback.Stars = 3;

            provider2.EmployerFeedback.ReviewCount = 1;
            provider2.EmployerFeedback.Stars = 1;

            provider3.EmployerFeedback.ReviewCount = 1;
            provider3.EmployerFeedback.Stars = 3;

            mediatorResult.Providers = new List<GetProvidersListItem> { provider1, provider2, provider3 };
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProvidersQuery>(c => c.Id.Equals(id)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviders(id, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseProvidersResponse;
            Assert.IsNotNull(model);
            model.TrainingCourseProviders.Count().Should().Be(2);
            model.Total.Should().Be(mediatorResult.Total);
            model.TotalFiltered.Should().Be(model.TrainingCourseProviders.Count());
        }

        [Test, MoqAutoData]
        public async Task Then_ApprenticeFeedbackRating_Filter_Is_Applied(
            int id,
            GetCourseProvidersRequest request,
            string location,
            GetProvidersListItem provider1,
            GetProvidersListItem provider2,
            GetProvidersListItem provider3,
            GetTrainingCourseProvidersResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            request.ApprenticeProviderRatings = new List<FeedbackRatingType>
            {
                FeedbackRatingType.Excellent,
                FeedbackRatingType.Good
            };
            request.EmployerProviderRatings = new List<FeedbackRatingType>();

            request.DeliveryModes = null;
            provider1.ApprenticeFeedback.Stars = 4;
            provider1.ApprenticeFeedback.ReviewCount = 1;

            provider2.ApprenticeFeedback.Stars = 2;
            provider2.ApprenticeFeedback.ReviewCount = 1;

            provider3.ApprenticeFeedback.Stars = 3;
            provider3.ApprenticeFeedback.ReviewCount = 1;

            mediatorResult.Providers = new List<GetProvidersListItem> { provider1, provider2, provider3 };
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProvidersQuery>(c => c.Id.Equals(id)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviders(id, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseProvidersResponse;
            Assert.IsNotNull(model);
            model.TrainingCourseProviders.Count().Should().Be(2);
            model.Total.Should().Be(mediatorResult.Total);
            model.TotalFiltered.Should().Be(model.TrainingCourseProviders.Count());
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int id,
            GetCourseProvidersRequest request,
            List<DeliveryModeType> deliveryModes,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCourseProvidersQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProviders(id, request) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [TestCase(ProviderCourseSortOrder.SortOrder.Name,"NameB","NameA",2,1,56, LocationType.Provider, LocationType.Provider, "NameA",1,false)]
        [TestCase(ProviderCourseSortOrder.SortOrder.Name, "NameA", "NameB", 2, 1, 56, LocationType.Provider, LocationType.Provider, "NameA",2,false)]
        [TestCase(ProviderCourseSortOrder.SortOrder.Distance, "NameA", "NameB", 2, 1, 56,LocationType.Provider, LocationType.Provider, "NameA", 2, false)]
        [TestCase(ProviderCourseSortOrder.SortOrder.Distance, "NameB", "NameB", 2, 1, 56, LocationType.Provider, LocationType.Provider, "NameB", 1, false)]
        [TestCase(ProviderCourseSortOrder.SortOrder.Name, "NameA", "NameA", 2, 3, 56, LocationType.Provider, LocationType.National, "NameA", 0, true)]
        [TestCase(ProviderCourseSortOrder.SortOrder.Distance, "NameA", "NameA", 2, 3, 0, LocationType.Provider, LocationType.National, "NameA", 0, true)]
        public async Task Then_Expected_Sort_Order_Is_Applied(ProviderCourseSortOrder.SortOrder sortOrder, string name1, string name2, decimal distance1, decimal distance2, double latLong, LocationType locationType1,LocationType locationType2, string expectedFirstName, decimal expectedDistanceInMiles, bool expectedNational)
        {
            const int id = 1;
            var deliveryModes = new List<DeliveryModeType> { DeliveryModeType.DayRelease };

            if (locationType1==LocationType.National || locationType2 == LocationType.National)
            {
                deliveryModes = new List<DeliveryModeType> { DeliveryModeType.DayRelease, DeliveryModeType.Workplace};
            }

            var request = new GetCourseProvidersRequest
            {
                DeliveryModes = deliveryModes,
                Lat = latLong,
                Lon=latLong
            };

            var provider1 = new GetProvidersListItem();
            var provider2 = new GetProvidersListItem();
            var mediatorResult = new GetTrainingCourseProvidersResult();
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<TrainingCoursesController>>();
            
            var controller = new TrainingCoursesController(mockLogger.Object,mockMediator.Object);

            request.SortOrder = sortOrder;

            request.ApprenticeProviderRatings = new List<FeedbackRatingType>();
          
            request.EmployerProviderRatings = new List<FeedbackRatingType>();

            provider1.Name = name1;
            provider1.Ukprn = 1;
            provider1.DeliveryModels = new List<DeliveryModel>
            {
                new()
                {
                    DistanceInMiles = distance1,
                    LocationType = locationType1,
                    DayRelease = (locationType1!=LocationType.National),
                }
            };

            provider2.Name = name2;
            provider2.Ukprn = 2;
            provider2.DeliveryModels = new List<DeliveryModel>
            {
                new()
                {
                    DistanceInMiles = distance2,
                    LocationType = locationType2,
                    DayRelease = (locationType2!=LocationType.National)
                }
            };

            provider1.ApprenticeFeedback = new GetApprenticeFeedbackResponse
            {
                Stars = 4,
                ReviewCount = 1
            };

            provider2.ApprenticeFeedback = new GetApprenticeFeedbackResponse
            {
                Stars = 4,
                ReviewCount = 1
            };

            mediatorResult.Providers = new List<GetProvidersListItem> { provider1, provider2 };
            mediatorResult.Course = new GetStandardsListItem {TypicalJobTitles = string.Empty, StandardDates = new StandardDate()};
            
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProvidersQuery>(c => c.Id.Equals(id)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviders(id, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseProvidersResponse;
            Assert.IsNotNull(model);
            model.TrainingCourseProviders.First().Name.Should().Be(expectedFirstName);
            model.TrainingCourseProviders.First().DeliveryModes.First().DistanceInMiles.Should()
                .Be(expectedDistanceInMiles);
            model.TrainingCourseProviders.First().DeliveryModes.First().National.Should().Be(expectedNational);

        }
    }
}
