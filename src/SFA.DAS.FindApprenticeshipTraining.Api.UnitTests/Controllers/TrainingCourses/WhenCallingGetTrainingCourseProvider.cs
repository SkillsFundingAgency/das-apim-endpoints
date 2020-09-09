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
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenCallingGetTrainingCourseProvider
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Course_And_Providers_From_Mediator(
            int standardCode,
            int providerId,
            string location,
            GetTrainingCourseProviderResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseProviderQuery>(
                        c
                            =>c.CourseId.Equals(standardCode)
                        && c.ProviderId.Equals(providerId)
                        && c.Location.Equals(location)
                        ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetProviderCourse(standardCode,providerId, location) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseProviderResponse;
            Assert.IsNotNull(model);
            model.TrainingCourse.Should().BeEquivalentTo(mediatorResult.Course, options=>options
                .Excluding(tc=>tc.ApprenticeshipFunding)
                .Excluding(tc=>tc.StandardDates)
            );
            model.TrainingCourseProvider.Should()
                .BeEquivalentTo(mediatorResult.ProviderStandard, 
                    options => options
                        .Excluding(c=>c.ContactUrl)
                        .Excluding(c=>c.StandardId)
                        .Excluding(c=>c.AchievementRates)
                        .Excluding(c=>c.Ukprn)
                        .Excluding(c=>c.DeliveryTypes)
                );
            model.AdditionalCourses.Courses.Should().BeEquivalentTo(mediatorResult.AdditionalCourses);
            model.TrainingCourse.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int standardCode,
            int providerId,
            string location,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCourseProviderQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProviderCourse(standardCode,providerId, location) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}