using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.Reference;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeships;

public class WhenAddReference
{
    private DraftApprenticeshipController _controller;
    private Mock<IMediator> _mediator;
    private DraftApprenticeshipSetReferenceRequest _request;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _request = _fixture.Create<DraftApprenticeshipSetReferenceRequest>();
        _mediator = new Mock<IMediator>();
        _controller = new DraftApprenticeshipController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
    }

    [Test]
    public async Task AddReferenceIsSubmittedCorrectly()
    {
        var cohortId = _fixture.Create<long>();
        var draftApprenticeshipId = _fixture.Create<long>();
        var email = _fixture.Create<string>();
        var providerId = _fixture.Create<long>();

        var response = new ApiResponse<DraftApprenticeshipSetReferenceResponse>(null, System.Net.HttpStatusCode.OK, string.Empty);

        _mediator.Setup(x => x.Send(It.Is<DraftApprenticeshipSetReferenceCommand>(y =>
             y.CohortId == cohortId &&
             y.DraftApprenticeshipId == draftApprenticeshipId &&
             y.Reference == email
         ), It.IsAny<CancellationToken>()));

        var result = await _controller.SetReference(providerId,cohortId, draftApprenticeshipId, _request);

        result.Should().BeOfType<OkResult>();
    }
}
