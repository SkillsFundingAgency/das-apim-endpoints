using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.AdminAan.Application.Admins.Queries.GetAdminMember;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.Admins.Queries.GetAdminMember;
public class GetAdminMemberHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesGetMemberByEmail(
      [Frozen] Mock<IAanHubRestApiClient> apiClient,
      GetAdminMemberHandler sut,
      string email,
      CancellationToken cancellationToken)
    {
        var query = new GetAdminMemberRequest { Email = email };
        await sut.Handle(query, cancellationToken);
        apiClient.Verify(a => a.GetMemberByEmail(email, cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_ApiClientReturnsResult(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetAdminMemberHandler sut,
        GetAdminMemberResult getMemberResult,
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken)
    {
        var response = new Response<GetAdminMemberResult>(
            "not used",
            new HttpResponseMessage(System.Net.HttpStatusCode.OK),
            () => getMemberResult);
        var query = new GetAdminMemberRequest { Email = email };

        apiClient.Setup(x => x.GetMemberByEmail(email, cancellationToken))
                 .ReturnsAsync(response);

        var actual = await sut.Handle(query, cancellationToken);

        var expected = new GetAdminMemberResult
        {
            MemberId = response.GetContent().MemberId,
            Status = response.GetContent().Status
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test, MoqAutoData]
    public async Task Handle_ApiClientReturnsNotFound_ReturnsNull(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetAdminMemberHandler sut,
        GetAdminMemberResult getMemberResult,
        Guid calendarEventId,
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken)
    {
        var response = new Response<GetAdminMemberResult>(
            "not used",
            new HttpResponseMessage(System.Net.HttpStatusCode.NotFound),
            () => getMemberResult);

        var query = new GetAdminMemberRequest { Email = email };

        apiClient.Setup(x => x.GetMemberByEmail(email, cancellationToken))
            .ReturnsAsync(response);

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().BeNull();
    }
}
