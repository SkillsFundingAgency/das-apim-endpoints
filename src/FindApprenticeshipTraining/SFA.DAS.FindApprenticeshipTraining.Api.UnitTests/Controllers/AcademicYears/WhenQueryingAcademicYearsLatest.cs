using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.AcademicYears.Queries.GetLatest;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.AcademicYears;
public sealed class WhenQueryingAcademicYearsLatest
{

    [Test]
    [MoqAutoData]
    public async Task GetLatestAcademicYears_CallsMediator_ReturnsAcademicYearsLatest(
        GetAcademicYearsLatestQueryResult expectedResult
    )
    {
        Mock<IMediator> mockMediator = new Mock<IMediator>();
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetAcademicYearsLatestQuery>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(expectedResult);

        var sut = new AcademicYearsController(mockMediator.Object);

        var result = await sut.GetAcademicYearsLatest() as ObjectResult;

        mockMediator.Verify(x => x.Send(It.IsAny<GetAcademicYearsLatestQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        var model = result!.Value as GetAcademicYearsLatestQueryResult;

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            model.Should().NotBeNull();
            model.Should().Be(expectedResult);
        }
    }
}
