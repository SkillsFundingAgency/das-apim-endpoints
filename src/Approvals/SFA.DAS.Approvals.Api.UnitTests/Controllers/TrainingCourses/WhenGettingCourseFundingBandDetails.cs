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
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.Courses.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.TrainingCourses
{
    public class WhenGettingCourseFundingBandDetails
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_FundingBand_From_Mediator(
                   string courseCode,
                   DateTime startDate,
                   GetFundingBandResult mediatorResult,
                   [Frozen] Mock<IMediator> mockMediator,
                   [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetFundingBandQuery>(x => x.CourseCode == courseCode && x.StartDate == startDate),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetFundingBand(courseCode, startDate) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetFundingBandResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_FundingBand_Is_Returned_From_Mediator(
            string courseCode,
            DateTime startDate,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetFundingBandQuery>(x => x.CourseCode == courseCode && x.StartDate == startDate),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetFundingBandResult)null);

            var controllerResult = await controller.GetFundingBand(courseCode, startDate) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_InternalServerError(
            string courseCode,
            DateTime startDate,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetFundingBandQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetFundingBand(courseCode, startDate) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
