using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ProviderPR.Api.Controllers;
using SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationshipsByUkprnPayeAorn;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderPR.Api.UnitTests.Controllers.EmployerAccountControllerTests;
public class GetRelationsByUkprnAornPayeTests
{
    [Test, MoqAutoData]
    public async Task Get_InvokesQueryHandler(
     [Frozen] Mock<IMediator> mediatorMock,
     [Greedy] EmployerAccountController sut,
     GetRelationshipsByUkprnPayeAornResult result,
     long ukprn,
     string aorn,
     string encodedPaye,
     CancellationToken cancellationToken)
    {
        var payeScheme = Uri.UnescapeDataString(encodedPaye);
        var query = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, payeScheme);

        mediatorMock.Setup(m => m.Send(query, cancellationToken))
            .ReturnsAsync(result);

        await sut.GetRelationshipByUkprnAornPaye(ukprn, aorn, encodedPaye, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetRelationshipsByUkprnPayeAornQuery>(x => x.Ukprn == ukprn && x.Aorn == aorn && x.Paye == payeScheme),
                cancellationToken));
    }

    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployerAccountController sut,
        long ukprn,
        string aorn,
        string encodedPaye,
        CancellationToken cancellationToken)
    {
        GetRelationshipsByUkprnPayeAornResult result = new GetRelationshipsByUkprnPayeAornResult();

        var payeScheme = Uri.UnescapeDataString(encodedPaye);
        var query = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, payeScheme);

        mediatorMock.Setup(m => m.Send(query, cancellationToken))
            .ReturnsAsync(result);

        var response = await sut.GetRelationshipByUkprnAornPaye(ukprn, aorn, encodedPaye, cancellationToken);

        response.As<OkObjectResult>().Should().NotBeNull();
        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(result);
    }
}
