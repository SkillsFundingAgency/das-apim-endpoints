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
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCoursesList;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenCallingGetTrainingCoursesList
    {
        [Test, MoqAutoData, Ignore("coming soon")]
        public async Task Then_Gets_Training_Courses_From_Mediator()
        {

        }

        [Test, MoqAutoData, Ignore("coming soon")]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCoursesListQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetList() as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
