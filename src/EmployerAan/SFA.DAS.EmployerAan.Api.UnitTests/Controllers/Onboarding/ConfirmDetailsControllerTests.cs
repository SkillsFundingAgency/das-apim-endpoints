using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers.Onboarding;
using SFA.DAS.EmployerAan.Application.Onboarding.ConfirmDetails.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.Onboarding;

[TestFixture]
public class ConfirmDetailsControllerTests
{
    [Theory, MoqAutoData]
    public async Task Get_ReturnsOkResult_WithExpectedResult(
        long employerAccountId,
        GetOnboardingConfirmDetailsQueryResult expectedResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ConfirmDetailsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(It.Is<GetOnboardingConfirmDetailsQuery>(q => q.EmployerAccountId == employerAccountId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await controller.Get(employerAccountId);
        
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().Be(expectedResult);
    }
}