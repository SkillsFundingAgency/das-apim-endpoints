using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.ProviderPermissions.Queries;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Approvals.UnitTests.Application.ProviderPermissions.Queries;

public class WhenGettingHasPermission
{
    [Test, AutoData]
    public async Task Then_The_PR_Api_Is_Invoked_And_Has_Permisions_Returned(
        GetHasPermissionQuery query,
        bool expected,
        CancellationToken cancellationToken)
    {
        Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> apiClientMock = new();
        apiClientMock.Setup(a => a.Get<bool>(It.Is<GetHasPermissionRequest>(r => r.GetUrl.Equals($"permissions/has?ukprn={query.Ukprn}&accountLegalEntityId={query.AccountLegalEntityId}&operations={(int)Operation.CreateCohort}")))).ReturnsAsync(expected);

        GetHasPermissionQueryHandler sut = new(apiClientMock.Object);

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }
}
