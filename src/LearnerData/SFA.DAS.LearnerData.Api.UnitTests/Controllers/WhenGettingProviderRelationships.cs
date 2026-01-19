using System.Net;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.Controllers;
using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

[TestFixture]
public class WhenGettingProviderRelationships
{
    [Test, MoqAutoData]
    public async Task Then_Ok_response_is_returned_for_given_ukprn(
        int ukprn,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ReferenceDataController>> mockLogger,
        [Greedy] ReferenceDataController sut)
    {
        // Arrange
        var queryResult = CreateProviderRelationQueryResult(ukprn);

        mockMediator
            .Setup(x => x.Send(It.IsAny<GetProviderRelationshipQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((queryResult));

        // Setup fake HttpContext to allow headers to be set
        var context = new DefaultHttpContext();
        sut.ControllerContext = new ControllerContext { HttpContext = context };

        // Act
        var result = await sut.GetProviderRelationshipDetails(ukprn) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var resultBody = result.Value as GetProviderRelationshipQueryResponse;
        resultBody.Should().NotBeNull();

        foreach (var expectedLearner in queryResult?.Employers ?? [])
        {
            if (expectedLearner == null) continue;
            resultBody!.Employers.Should().Contain(x =>
                x.AgreementId == expectedLearner.AgreementId &&
                x.IsFlexiEmployer == expectedLearner.IsFlexiEmployer &&
                x.IsLevy == expectedLearner.IsLevy);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_not_found_response_is_returned_for_given_ukprn(
        int ukprn,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] Mock<ILogger<ReferenceDataController>> mockLogger,
        [Greedy] ReferenceDataController sut)
    {
        mockMediator
            .Setup(x => x.Send(It.IsAny<GetProviderRelationshipQuery>(), It.IsAny<CancellationToken>())).
            ReturnsAsync((GetProviderRelationshipQueryResponse?)null);

        // Setup fake HttpContext to allow headers to be set
        var context = new DefaultHttpContext();
        sut.ControllerContext = new ControllerContext { HttpContext = context };

        // Act
        var result = await sut.GetProviderRelationshipDetails(ukprn) as NotFoundResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    private static GetProviderRelationshipQueryResponse CreateProviderRelationQueryResult(int ukprn)
    {
        var fixture = new Fixture();
        var employers = fixture.Create<EmployerDetails[]>();

        return new GetProviderRelationshipQueryResponse
        {
            Status = ProviderStatusType.Active.ToString(),
            Type = ProviderType.Employer.ToString(),
            UkPRN = ukprn.ToString(),
            Employers = employers
        };
    }
}