using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.Learners.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.SelectFundingOptions;

public class WhenGettingLearnersForProvider
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_Learners_From_Mediator(
        long providerId,
        long accountLegalEntityId,
        string searchTerm,
        string sortColumn,
        bool sortDescending,
        int page,
        GetLearnersForProviderQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectIlrLearnersController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.IsAny<GetLearnersForProviderQuery>(),
                It.IsAny<CancellationToken>()))
            .Callback((object o, CancellationToken ct) =>
            {
                var q = (GetLearnersForProviderQuery) o;
                q.ProviderId.Should().Be(providerId);
                q.AccountLegalEntityId.Should().Be(accountLegalEntityId);
                q.SearchTerm.Should().Be(searchTerm);
                q.SortField.Should().Be(sortColumn);
                q.SortDescending.Should().Be(sortDescending);
                q.Page.Should().Be(page);
            })
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Get(providerId, accountLegalEntityId, searchTerm, sortColumn, sortDescending, page) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetLearnersForProviderQueryResult;
        Assert.That(model, Is.Not.Null);
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_InternalServerError(
        long providerId,
        long accountLegalEntityId,
        string searchTerm,
        string sortColumn,
        bool sortDescending,
        int page,
        GetLearnersForProviderQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectIlrLearnersController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.IsAny<GetLearnersForProviderQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.Get(providerId, accountLegalEntityId, searchTerm, sortColumn, sortDescending, page) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}