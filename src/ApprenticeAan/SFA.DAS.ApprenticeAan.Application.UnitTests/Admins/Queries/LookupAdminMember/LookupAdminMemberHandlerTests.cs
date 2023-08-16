using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Application.Admins.Queries.Lookup;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Admins.Queries.LookupAdminMember;
public class LookupAdminMemberHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesGetMemberByEmail(
      [Frozen] Mock<IAanHubRestApiClient> apiClient,
      LookupAdminMemberHandler sut,
      string email,
      string firstName,
      string lastName,
      CancellationToken cancellationToken)
    {
        var query = new LookupAdminMemberRequest { Email = email, FirstName = firstName, LastName = lastName };
        await sut.Handle(query, cancellationToken);
        apiClient.Verify(a => a.GetMemberByEmail(email, cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_ApiClientReturnsResult(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        LookupAdminMemberHandler sut,
        GetMemberResult getMemberResult,
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken)
    {
        var response = new Response<GetMemberResult>(
            "not used",
            new HttpResponseMessage(System.Net.HttpStatusCode.OK),
            () => getMemberResult);
        var query = new LookupAdminMemberRequest { Email = email, FirstName = firstName, LastName = lastName };

        apiClient.Setup(x => x.GetMemberByEmail(email, cancellationToken))
                 .ReturnsAsync(response);

        var actual = await sut.Handle(query, cancellationToken);

        var expected = new LookupAdminMemberResult
        {
            MemberId = response.GetContent().MemberId,
            Status = response.GetContent().Status
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test, MoqAutoData]
    public async Task Handle_ApiClientReturnsNotFound_ReturnsNull(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        LookupAdminMemberHandler sut,
        GetMemberResult getMemberResult,
        Guid calendarEventId,
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken)
    {
        var response = new Response<GetMemberResult>(
            "not used",
            new HttpResponseMessage(System.Net.HttpStatusCode.NotFound),
            () => getMemberResult);

        var query = new LookupAdminMemberRequest { Email = email, FirstName = firstName, LastName = lastName };

        apiClient.Setup(x => x.GetMemberByEmail(email, cancellationToken))
            .ReturnsAsync(response);


        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().BeNull();
    }
}
