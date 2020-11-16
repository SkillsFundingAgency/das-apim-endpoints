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
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenCallingGetTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Course_And_Providers_Count_From_Mediator(
            int standardCode,
            double lat,
            double lon,
            GetTrainingCourseResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTrainingCourseQuery>(c=>
                        c.Id.Equals(standardCode)
                        && c.Lat.Equals(lat)
                        && c.Lon.Equals(lon)
                        ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Get(standardCode, lat, lon) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTrainingCourseResponse;
            Assert.IsNotNull(model);
            model.TrainingCourse.Should().BeEquivalentTo(mediatorResult.Course, options => options
                .Excluding(tc => tc.ApprenticeshipFunding)
                .Excluding(tc => tc.StandardDates)
                .Excluding(tc => tc.Duties)
                .Excluding(tc => tc.Skills)
                .Excluding(tc => tc.CoreAndOptions));

            model.ProvidersCount.TotalProviders.Should().Be(mediatorResult.ProvidersCount);
            model.ProvidersCount.ProvidersAtLocation.Should().Be(mediatorResult.ProvidersCountAtLocation);

        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int standardCode,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCourseQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(standardCode) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}