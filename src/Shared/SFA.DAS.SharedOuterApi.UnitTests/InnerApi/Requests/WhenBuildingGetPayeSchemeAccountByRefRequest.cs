using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetPayeSchemeAccountByRefRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string payeScheme)
    {
        var actual = new GetPayeSchemeAccountByRefRequest(payeScheme);

        actual.GetUrl.Should().Be($"api/accounthistories?payeRef={payeScheme}");
    }
}