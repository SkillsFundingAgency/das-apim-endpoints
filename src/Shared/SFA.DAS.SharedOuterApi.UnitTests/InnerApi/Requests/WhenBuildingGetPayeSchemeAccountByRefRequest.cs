using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetPayeSchemeAccountByRefRequest
{
    [Test]
    public void Then_The_Request_Is_Correctly_Build()
    {
        string payeScheme = "307/NL8800";

        string expectedPayeScheme = Uri.EscapeDataString(payeScheme);

        var actual = new GetPayeSchemeAccountByRefRequest(payeScheme);

        actual.GetUrl.Should().Be($"api/accounthistories?payeRef={expectedPayeScheme}");
    }
}