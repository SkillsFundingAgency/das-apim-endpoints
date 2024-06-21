using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.PingControllerTests;
public class GetPingTests
{
    [Test, MoqAutoData]
    public void GetPing_ReturnsExpectedResponse(
        [Greedy] PingController sut)
    {
        var result = sut.Get();

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be("Pong");
    }
}
