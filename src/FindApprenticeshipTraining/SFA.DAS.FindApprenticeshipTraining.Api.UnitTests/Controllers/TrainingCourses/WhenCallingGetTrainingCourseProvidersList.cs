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
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

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
                        .Excluding(c=>c.DeliveryModels)
                        .Excluding(c => c.EmployerFeedback)
                        .Excluding(c => c.ApprenticeFeedback)
                        .Excluding(c=>c.DeliveryModelsShortestDistance)
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
    }
}