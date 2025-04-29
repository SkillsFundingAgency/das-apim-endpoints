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
            int academicYear,
            List<LearnerDataRequest> learners,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LearnersController sut)
        {
            long ukprn = 12345678;

            learners.ForEach(x =>
            {
                x.UKPRN = ukprn;
                x.ULN = 1234567890;
            });

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
        int academicYear,
        List<LearnerDataRequest> learners,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LearnersController sut)
    {
        long ukprn = 12345678;

        learners.ForEach(x =>
        {
            x.UKPRN = ukprn;
            x.ULN = 1234567890;
        });

        learners[0].UKPRN = ukprn + 1;

        var result = await sut.Post(ukprn, academicYear, learners) as BadRequestObjectResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        var response = result.Value as ErrorResponse;
        response.Should().NotBeNull();
        var errors = response.Errors.ToList();
        errors.Count.Should().Be(1);
        errors[0].Code.Should().Be("UKPRN");
        errors[0].Message.Should().Be($"Learner data contains different UKPRN to {ukprn}");
        mockMediator.Verify(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task And_when_ULN_Does_Not_Match_Then_BadRequest_returned(
        int academicYear,
        List<LearnerDataRequest> learners,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LearnersController sut)
    {
        long ukprn = 12345678;

        learners.ForEach(x =>
        {
            x.UKPRN = ukprn;
            x.ULN = 12345;
        });

        var result = await sut.Post(ukprn, academicYear, learners) as BadRequestObjectResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        var response = result.Value as ErrorResponse;
        response.Should().NotBeNull();
        var errors = response.Errors.ToList();
        errors.Count.Should().Be(1);
        errors[0].Code.Should().Be("ULN");
        errors[0].Message.Should().Be("Learner data contains incorrect ULNs");
        mockMediator.Verify(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task And_when_ConsumerReference_Is_Too_Big(
        int academicYear,
        List<LearnerDataRequest> learners,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LearnersController sut)
    {
        long ukprn = 12345678;

        learners.ForEach(x =>
        {
            x.UKPRN = ukprn;
            x.ULN = 1234567890;
            x.ConsumerReference = new string('C', 101);
        });

        var result = await sut.Post(ukprn, academicYear, learners) as BadRequestObjectResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        var response = result.Value as ErrorResponse;
        response.Should().NotBeNull();
        var errors = response.Errors.ToList();
        errors.Count.Should().Be(1);
        errors[0].Code.Should().Be("ConsumerReference");
        errors[0].Message.Should().Be("Learner data contains incorrect ConsumerReference (>100 chars)");
        mockMediator.Verify(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task And_when_not_working_Then_InternalError_returned(
        int academicYear,
        List<LearnerDataRequest> learners,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LearnersController sut)
    {
        long ukprn = 12345678;

        learners.ForEach(x =>
        {
            x.UKPRN = ukprn;
            x.ULN = 1234567890;
        });
        mockMediator.Setup(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("test"));

        var result = await sut.Post(ukprn, academicYear, learners) as StatusCodeResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        mockMediator.Verify(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}