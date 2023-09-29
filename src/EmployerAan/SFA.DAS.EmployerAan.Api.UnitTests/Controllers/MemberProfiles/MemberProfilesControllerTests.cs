using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.EmployerAan.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.MemberProfiles;

public class MemberProfilesControllerTests
{
    [Test]
    [MoqInlineAutoData(false)]
    [MoqInlineAutoData(true)]
    public async Task When_MediatorCommandSuccessful_Then_ReturnOk(
        bool isPublicView,
        GetMemberProfileWithPreferencesQueryResult memberProfileWithPreferencesQueryResult,
        GetEmployerMemberSummaryQueryResult employerMemberSummaryQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        long accountId,
        Guid memberId,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        memberProfileWithPreferencesQueryResult.AccountId = accountId;
        GetEmployerMemberSummaryQuery employerMemberSummaryQuery = new(accountId);
        GetMemberProfileWithPreferencesModel response = new(memberProfileWithPreferencesQueryResult, employerMemberSummaryQueryResult);

        mockMediator.Setup(m => m.Send(It.IsAny<GetMemberProfileWithPreferencesQuery>(), cancellationToken)).ReturnsAsync(memberProfileWithPreferencesQueryResult);
        mockMediator.Setup(m => m.Send(employerMemberSummaryQuery, cancellationToken)).ReturnsAsync(employerMemberSummaryQueryResult);

        var sut = new MemberProfilesController(mockMediator.Object);
        var result = await sut.GetMemberProfileWithPreferences(memberId, requestedByMemberId, cancellationToken, isPublicView);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}
