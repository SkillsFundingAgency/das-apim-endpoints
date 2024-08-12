using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.InnerApi;

namespace SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.UnitTests.InnerApi;

public class WhenBuildingPutApprenticeApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_And_Data_Is_Set(PutApprenticeApiRequestData requestData)
    {
        var actual = new PutApprenticeApiRequest(requestData);

        actual.PutUrl.Should().Be("/apprentices");
        ((PutApprenticeApiRequestData)actual.Data).Should().BeEquivalentTo(requestData);
    }
}