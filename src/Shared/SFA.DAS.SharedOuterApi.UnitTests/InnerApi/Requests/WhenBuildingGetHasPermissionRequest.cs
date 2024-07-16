using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;
using Operation = SFA.DAS.SharedOuterApi.Models.ProviderRelationships.Operation;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;
public class WhenBuildingGetHasPermissionRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(int ukprn, long accountLegalEntityId)
    {
        var actual = new GetHasPermissionRequest(ukprn, accountLegalEntityId, Operation.Recruitment);

        actual.GetUrl.Should().Be($"permissions/has?ukprn={ukprn}&accountLegalEntityId={accountLegalEntityId}&operation={(int)Operation.Recruitment}");
    }
}
