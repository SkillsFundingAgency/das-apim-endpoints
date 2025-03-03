using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMemberByMemberId;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.MembersControllerTests;

public class GetMemberByIdTests
{
    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkRespons(
    [Frozen] Mock<IMediator> mediatorMock,
    Guid memberId,
    GetMemberByIdQueryResult queryResult,
    [Greedy] MembersController sut,
    CancellationToken cancellationToken)
    {
        // Arrange
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberByIdQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
    .ReturnsAsync(queryResult);

        // Act
        var result = await sut.GetMemberById(memberId, cancellationToken);

        // Assert
        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
