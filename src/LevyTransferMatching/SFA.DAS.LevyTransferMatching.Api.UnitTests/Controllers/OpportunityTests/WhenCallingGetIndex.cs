using AutoFixture;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Opportunity;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.OpportunityTests;

[TestFixture]
public class WhenCallingGetIndex
{
    private readonly Fixture _fixture = new Fixture();

    [Test, MoqAutoData]
    public async Task Returns_Ok_And_Response(
        GetIndexQueryResult getIndexQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] OpportunityController opportunityController
        )
    {
        var sectorsList = _fixture.Create<IEnumerable<string>>();
        mockMediator
            .Setup(x => x.Send(
                It.IsAny<GetIndexQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(getIndexQueryResult);

        var controllerResult = await opportunityController.GetIndex(sectorsList);
        var okObjectResult = controllerResult as OkObjectResult;
        var responseObject = okObjectResult.Value as GetIndexResponse;

        responseObject.Opportunities.Should().BeEquivalentTo(getIndexQueryResult.Items);
        responseObject.TotalOpportunities.Should().Be(getIndexQueryResult.TotalItems);
        responseObject.Page.Should().Be(getIndexQueryResult.Page);
        responseObject.TotalPages.Should().Be(getIndexQueryResult.TotalPages);
        responseObject.PageSize.Should().Be(getIndexQueryResult.PageSize);
        responseObject.Sectors.Should().BeEquivalentTo(getIndexQueryResult.Sectors);
        responseObject.JobRoles.Should().BeEquivalentTo(getIndexQueryResult.JobRoles);
        responseObject.Levels.Should().BeEquivalentTo(getIndexQueryResult.Levels);
        okObjectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}