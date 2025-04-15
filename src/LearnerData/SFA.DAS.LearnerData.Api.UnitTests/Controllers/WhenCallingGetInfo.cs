using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.Controllers;
using SFA.DAS.LearnerData.Api.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;
public class WhenCallingGetInfo
{
    [Test, MoqAutoData]
    public void Then_Version_Is_Returned_From_Config(
        string expectedVersion,
        [Frozen] Mock<IConfiguration> config,
        [Greedy] InfoController sut)
    {
        config.Setup(c => c["ApiVersion"]).Returns(expectedVersion);

        var result = sut.GetInfo() as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var info = result.Value as GetAppInfoResponse;
        info.Should().NotBeNull();
        info.Version.Should().Be(expectedVersion);
    }
}