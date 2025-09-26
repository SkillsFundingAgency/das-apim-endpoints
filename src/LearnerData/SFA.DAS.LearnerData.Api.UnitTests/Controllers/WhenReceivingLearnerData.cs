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
using SFA.DAS.LearnerData.Application.ProcessLearners;
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
        [Frozen]  Mock<IValidator<IEnumerable<LearnerDataRequest>>> mockValidator,
        [Greedy] LearnersController sut)
    {
        long ukprn = 12345678;

        learners.ForEach(x =>
        {
            x.UKPRN = ukprn;
            x.ULN = 1234567890;
        });

        mockValidator.Setup(x => x.ValidateAsync(It.IsAny<IEnumerable<LearnerDataRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var result = await sut.Put(ukprn, academicYear, learners) as AcceptedResult;

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
    public async Task And_when_not_working_Then_InternalError_returned(
        int academicYear,
        List<LearnerDataRequest> learners,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<IValidator<IEnumerable<LearnerDataRequest>>> mockValidator,
        [Greedy] LearnersController sut)
    {
        long ukprn = 12345678;

        learners.ForEach(x =>
        {
            x.UKPRN = ukprn;
            x.ULN = 1234567890;
        });
        mockValidator.Setup(x => x.ValidateAsync(It.IsAny<IEnumerable<LearnerDataRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        mockMediator.Setup(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("test"));

        var result = await sut.Put(ukprn, academicYear, learners) as StatusCodeResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        mockMediator.Verify(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_when_validation_check_has_errors_Then_BadRequest_returned(
        int academicYear,
        List<LearnerDataRequest> learners,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<IValidator<IEnumerable<LearnerDataRequest>>> mockValidator,
        [Greedy] LearnersController sut)
    {
        long ukprn = 12345678;

        learners.ForEach(x =>
        {
            x.UKPRN = ukprn;
            x.ULN = 1234567890;
        });
        var errors = new List<ValidationFailure>();
        errors.Add(new ValidationFailure
        {
            ErrorMessage = "This is a test error",
            PropertyName = "TEST"
        });

        mockValidator.Setup(x => x.ValidateAsync(It.IsAny<IEnumerable<LearnerDataRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(errors));


        var result = await sut.Put(ukprn, academicYear, learners) as BadRequestObjectResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        mockMediator.Verify(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}