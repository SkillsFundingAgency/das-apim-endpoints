using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddEmail;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeships;

public class WhenAddEmail
{
    private DraftApprenticeshipController _controller;
    private Mock<IMediator> _mediator;
    private AddDraftApprenticeEmailRequest _request;
    private DraftApprenticeshipAddEmailCommand _command;
    private Fixture _fixture;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _request = _fixture.Create<AddDraftApprenticeEmailRequest>();
        _command = _fixture.Create<DraftApprenticeshipAddEmailCommand>();
        _mediator = new Mock<IMediator>();
        _controller = new DraftApprenticeshipController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
    }

    [Test]
    public async Task AddEmailIsSubmittedCorrectly()
    {
        var cohortId = _fixture.Create<long>();
        var providerId = _fixture.Create<long>();
        var draftApprenticeshipId = _fixture.Create<long>();
        var email = "test@test.com";

        var response = new ApiResponse<DraftApprenticeshipAddEmailResponse>(null, System.Net.HttpStatusCode.OK, string.Empty);

        _mediator.Setup(x => x.Send(It.Is<DraftApprenticeshipAddEmailCommand>(y =>
             y.CohortId == cohortId &&
             y.DraftApprenticeshipId == draftApprenticeshipId &&
             y.Email == email
         ), It.IsAny<CancellationToken>()));

        var result = await _controller.AddEmail(providerId,cohortId, draftApprenticeshipId, _request);

        result.Should().BeOfType<OkResult>();
    }
}


