using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.Controllers;
using SFA.DAS.LearnerData.Application.CreateLearner;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

public class WhenCreatingLearner
{
    [Test, MoqAutoData]
    public async Task And_when_working_Then_Accepted_returned(
        CreateLearnerRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen]  Mock<IValidator<CreateLearnerRequest>> mockValidator,
        [Greedy] LearnersController sut)
    {
        long ukprn = 12345678;

        request.Learner.Uln = "1234567890";

        mockValidator.Setup(x => x.ValidateAsync(It.IsAny<CreateLearnerRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var result = await sut.CreateLearningRecord(ukprn, request) as AcceptedResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
        var response = result.Value as CorrelationResponse;
        response.Should().NotBeNull();
        response.CorrelationId.Should().NotBeEmpty();
        mockMediator.Verify(
            x => x.Send(
                It.Is<CreateLearnerCommand>(p =>
                    p.Request == request && 
                    p.CorrelationId == response.CorrelationId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_when_not_working_Then_InternalError_returned(
        CreateLearnerRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<IValidator<CreateLearnerRequest>> mockValidator,
        [Greedy] LearnersController sut)
    {
        long ukprn = 12345678;

        request.Learner.Uln = "1234567890";

        mockValidator.Setup(x => x.ValidateAsync(It.IsAny<CreateLearnerRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        mockMediator.Setup(x => x.Send(It.IsAny<CreateLearnerCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("test"));

        var result = await sut.CreateLearningRecord(ukprn, request) as StatusCodeResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        mockMediator.Verify(x => x.Send(It.IsAny<CreateLearnerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}