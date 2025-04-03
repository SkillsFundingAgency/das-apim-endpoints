using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.Controllers;
using SFA.DAS.LearnerData.Application;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

    public class WhenReceivingLearnerData
    {
        [Test, MoqAutoData]
        public async Task And_when_working_Then_Accepted_returned(
            long ukprn,
            int academicYear,
            List<LearnerDataRequest> learners,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LearnersController sut)
        {
            learners.ForEach(x => { x.UKPRN = ukprn; });

            var result = await sut.Post(ukprn, academicYear, learners) as AcceptedResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            var response = result.Value as CorrelationResponse;
            response.Should().NotBeNull();
            response.CorrelationId.Should().NotBeEmpty();
            mockMediator.Verify(
                x => x.Send(
                    It.Is<ProcessLearnersCommand>(p =>
                        p.Learners == learners && 
                        p.CorrelationId == response.CorrelationId &&
                        p.AcademicYear == academicYear), It.IsAny<CancellationToken>()), Times.Once);
        }

    [Test, MoqAutoData]
    public async Task And_when_UKPRN_Does_Not_Match_Then_BadRequest_returned(
        long ukprn,
        int academicYear,
        List<LearnerDataRequest> learners,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LearnersController sut)
    {
        learners[0].UKPRN = ukprn + 1;

        var result = await sut.Post(ukprn, academicYear, learners) as BadRequestObjectResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        var response = result.Value as ErrorResponse;
        response.Should().NotBeNull();
        response.Errors.Count.Should().Be(1);
        response.Errors[0].Code.Should().Be("UKPRN");
        response.Errors[0].Message.Should().Be($"Learner data contains different UKPRN to {ukprn}");
        mockMediator.Verify(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }


    [Test, MoqAutoData]
    public async Task And_when_not_working_Then_InternalError_returned(
        long ukprn,
        int academicYear,
        List<LearnerDataRequest> learners,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LearnersController sut)
    {
        learners.ForEach(x => { x.UKPRN = ukprn; });
        mockMediator.Setup(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("test"));

        var result = await sut.Post(ukprn, academicYear, learners) as StatusCodeResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        mockMediator.Verify(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}