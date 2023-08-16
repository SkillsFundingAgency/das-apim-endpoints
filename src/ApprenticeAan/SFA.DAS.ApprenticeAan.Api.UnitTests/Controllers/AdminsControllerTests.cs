using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Admins.Queries.Lookup;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers;

public class AdminsControllerTests
{
    [Test, MoqAutoData]
    public async Task CreateApprenticeMember_InvokesCommand(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        LookupAdminMemberRequest request,
        LookupAdminMemberResult expected,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(request, cancellationToken)).ReturnsAsync(expected);

        var response = await sut.Lookup(request, cancellationToken);

        mediatorMock.Verify(m => m.Send(request, cancellationToken));
        response.As<OkObjectResult>().Value.Should().Be(expected);
    }
}