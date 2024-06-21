using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.ServiceCheckControllerTests;
public class GetServiceCheckControllerTests
{
    [Test, MoqAutoData]
    public void GetServiceCheck_ReturnsExpectedResponse(
        [Greedy] ServiceCheckController sut)
    {
        var result = sut.Get();

        result.As<OkResult>().Should().NotBeNull();
        result.As<OkResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
