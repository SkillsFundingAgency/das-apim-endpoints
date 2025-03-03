using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Queries.ValidateRequest;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public class ValidateCreateAccountRequestTests
{
    [Test, MoqAutoData]
    public async Task ValidateRequest_ReturnsValidationResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RequestsController sut,
        Guid requestId,
        ValidatePermissionsRequestQueryResult expected,
        CancellationToken cancellationToken
        )
    {
        mediatorMock.Setup(a => a.Send(It.Is<ValidatePermissionsRequestQuery>(q => q.RequestId == requestId), cancellationToken)).ReturnsAsync(expected);

        var result = await sut.ValidateCreateAccountRequest(requestId, cancellationToken);

        var actual = result.As<OkObjectResult>().Value.As<ValidatePermissionsRequestQueryResult>();
        actual.Should().Be(expected);
    }
}
