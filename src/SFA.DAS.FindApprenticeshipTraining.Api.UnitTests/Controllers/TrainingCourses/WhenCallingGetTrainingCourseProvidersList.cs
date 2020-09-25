using System;
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
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenCallingGetTrainingCourseProvidersList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Course_And_Providers_From_Mediator(
            int standardCode,
            string location,
            ProviderCourseSortOrder.SortOrder sortOrder,
            GetTrainingCourseProvidersResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProvidersQuery>(c=>c.Id.Equals(standardCode)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviders(standardCode, location, sortOrder) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseProvidersResponse;
            Assert.IsNotNull(model);
            model.TrainingCourse.Should().BeEquivalentTo(mediatorResult.Course, options=>options
                .Excluding(tc=>tc.ApprenticeshipFunding)
                .Excluding(tc=>tc.StandardDates)
            );
            model.TrainingCourseProviders.Should()
                .BeEquivalentTo(mediatorResult.Providers, 
                    options => options.Excluding(c=>c.Ukprn)
                        .Excluding(c=>c.AchievementRates)
                        .Excluding(c=>c.DeliveryTypes)
                        .Excluding(c=>c.FeedbackAttributes)
                        .Excluding(c=>c.FeedbackRatings)
                    );
            model.Total.Should().Be(mediatorResult.Total);
            model.Location.Location.GeoPoint.Should().BeEquivalentTo(mediatorResult.Location.GeoPoint);
            model.Location.Name.Should().Be(mediatorResult.Location.Name);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int standardCode,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCourseProvidersQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProviders(standardCode, "") as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}