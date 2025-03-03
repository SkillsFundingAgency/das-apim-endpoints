using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.User;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetUserByRefRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string userRef)
    {
        var actual = new GetUserByRefRequest(userRef);

        actual.GetUrl.Should().Be($"api/user/by-ref/{userRef}");
    }
}