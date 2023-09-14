using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.MemberProfiles;
public class GetMemberProfileWithPreferencesTests
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
