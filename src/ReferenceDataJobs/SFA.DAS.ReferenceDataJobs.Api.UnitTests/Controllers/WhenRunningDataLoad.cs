using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceDataJobs.Api.Controllers;
using SFA.DAS.ReferenceDataJobs.Application.Commands;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ReferenceDataJobs.Api.UnitTests.Controllers;

public class WhenRunningDataLoad
{
    [Test, MoqAutoData]
    public async Task And_when_working_Then_Ok_returned(
        long vacancyReference,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] DataLoadController sut)
    {

        var result = await sut.Post(CancellationToken.None) as OkResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mockMediator.Verify(x=>x.Send(It.IsAny<StartDataLoadsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_when_not_working_Then_internalerror_returned(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] DataLoadController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<StartDataLoadsCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("test"));

        var result = await sut.Post(CancellationToken.None) as StatusCodeResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        mockMediator.Verify(x => x.Send(It.IsAny<StartDataLoadsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}