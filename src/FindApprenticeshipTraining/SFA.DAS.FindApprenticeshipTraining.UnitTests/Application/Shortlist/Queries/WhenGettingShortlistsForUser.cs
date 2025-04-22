using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistsForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Queries;

public class WhenGettingShortlistsForUser
{
    [Test, MoqAutoData]
    public async Task Then_Gets_The_Shortlists_From_RoatpApi(
         GetShortlistsForUserQuery query,
         GetShortlistsForUserResponse apiResponse,
         [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
         GetShortlistsForUserQueryHandler handler)
    {
        mockRoatpV2ApiClient.Setup(x =>
                x.Get<GetShortlistsForUserResponse>(It.Is<GetShortlistsForUserRequest>(r => r.GetUrl.Contains(query.UserId.ToString()))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(apiResponse);
    }
}
