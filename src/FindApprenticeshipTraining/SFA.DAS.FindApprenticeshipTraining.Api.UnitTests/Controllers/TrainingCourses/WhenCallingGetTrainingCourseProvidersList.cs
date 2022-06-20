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
            [Greedy]TrainingCoursesController controller)
        {
            request.DeliveryModes = new List<DeliveryModeType>();
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProvidersQuery>(c=>
                        c.Id.Equals(id)
                        && c.Location.Equals(request.Location)
                        && c.SortOrder == (short)request.SortOrder
                        && c.Lat.Equals(request.Lat)
                        && c.Lon.Equals(request.Lon)
                        && c.ShortlistUserId.Equals(request.ShortlistUserId)
                        ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviders(id, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseProvidersResponse;
            Assert.IsNotNull(model);
            model.TrainingCourse.Should().BeEquivalentTo((GetTrainingCourseListItem)mediatorResult.Course);
            model.TrainingCourseProviders.Should()
                .BeEquivalentTo(mediatorResult.Providers, 
                    options => options.Excluding(c=>c.Ukprn)
                        .Excluding(c=>c.AchievementRates)
                        .Excluding(c=>c.DeliveryTypes)
                        .Excluding(c=>c.EmployerFeedback)
                        .Excluding(c=>c.ApprenticeFeedback)
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
            ProviderCourseSortOrder.SortOrder sortOrder,
            GetTrainingCourseProvidersResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            request.DeliveryModes = new List<DeliveryModeType>
            {
                DeliveryModeType.Workplace
            };
            provider1.DeliveryTypes = provider1.DeliveryTypes.Select(c =>
            {
                c.DeliveryModes = "100PercentEmployer";
                return c;
            }).ToList();
            provider2.DeliveryTypes = provider2.DeliveryTypes.Select(c =>
            {
                c.DeliveryModes = "DayRelease";
                return c;
            }).ToList();
            mediatorResult.Providers = new List<GetProvidersListItem>{provider1, provider2};
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProvidersQuery>(c=>c.Id.Equals(id)),
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
        public async Task Then_FeedbackRating_Filter_Is_Applied(
            int id,
            GetCourseProvidersRequest request,
            string location,
            GetProvidersListItem provider1,
            GetProvidersListItem provider2,
            GetProvidersListItem provider3,
            GetTrainingCourseProvidersResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            request.EmployerProviderRatings = new List<FeedbackRatingType>
            {
                FeedbackRatingType.Excellent,
                FeedbackRatingType.Good
            };
            request.DeliveryModes = null;
            provider1.EmployerFeedback.FeedbackRatings = new List<GetEmployerFeedbackRatingItem>
            {
                new GetEmployerFeedbackRatingItem
                {
                    FeedbackName = "Excellent",
                    FeedbackCount = 1,
                }
            };
            provider2.EmployerFeedback.FeedbackRatings = new List<GetEmployerFeedbackRatingItem>
            {
                new GetEmployerFeedbackRatingItem
                {
                    FeedbackName = "Poor",
                    FeedbackCount = 1,
                }
            };
            provider3.EmployerFeedback.FeedbackRatings = new List<GetEmployerFeedbackRatingItem>
            {
                new GetEmployerFeedbackRatingItem
                {
                    FeedbackName = "Good",
                    FeedbackCount = 1,
                }
            };
            mediatorResult.Providers = new List<GetProvidersListItem>{provider1, provider2, provider3};
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProvidersQuery>(c=>c.Id.Equals(id)),
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
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCourseProvidersQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProviders(id,request) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}