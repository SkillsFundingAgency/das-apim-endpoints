using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.MemberProfiles;
public class MemberProfilesControllerTests
{
    [Test]
    [MoqInlineAutoData(false)]
    [MoqInlineAutoData(true)]
    public async Task When_MediatorCommandSuccessful_Then_ReturnOk(
        bool isPublicView,
        GetMemberProfileWithPreferencesQueryResult response,
        [Frozen] Mock<IMediator> mockMediator,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        mockMediator.Setup(m => m.Send(It.IsAny<GetMemberProfileWithPreferencesQuery>(), cancellationToken)).ReturnsAsync(response);
        var controller = new MemberProfilesController(mockMediator.Object);

        var result = await controller.GetMemberProfileWithPreferences(memberId, cancellationToken, isPublicView);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}
