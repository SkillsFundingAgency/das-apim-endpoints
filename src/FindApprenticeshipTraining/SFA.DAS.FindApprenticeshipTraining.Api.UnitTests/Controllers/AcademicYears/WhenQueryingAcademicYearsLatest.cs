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
    public async Task Then_Passes_Query_To_Mediator_And_Returns_AcademicYearsLatest(
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

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

            var model = result.Value as GetAcademicYearsLatestQueryResult;
            Assert.That(model, Is.Not.Null);
            Assert.That(model, Is.EqualTo(expectedResult));
        });
    }
}
