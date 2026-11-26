using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Apprentices.Commands.ChangePayments;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Commands;

[TestFixture]
public class ChangePaymentsCommandHandlerTests
{
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
    private ChangePaymentsCommandHandler _handler;
    private IPatchApiRequest<PatchApprenticeshipPaymentsApiRequest.Body> _capturedRequest;

    [SetUp]
    public void SetUp()
    {
        _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _apiClient.Setup(x => x.PatchWithResponseCode<PatchApprenticeshipPaymentsApiRequest.Body, NullResponse>(
                It.IsAny<IPatchApiRequest<PatchApprenticeshipPaymentsApiRequest.Body>>(),
                false))
            .Callback((IPatchApiRequest<PatchApprenticeshipPaymentsApiRequest.Body> request, bool _) => _capturedRequest = request)
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

        _handler = new ChangePaymentsCommandHandler(
            _apiClient.Object,
            new ServiceParameters(Party.Employer, 123));
    }

    [Test]
    public async Task Handle_WhenFreezing_PatchesPaymentFreezeDateAndReason()
    {
        var freezeDate = new DateTime(2026, 1, 12);
        var command = new ChangePaymentsCommand
        {
            ApprenticeshipId = 123,
            PaymentFreezeDate = freezeDate,
            FreezePaymentsReason = 1
        };

        await _handler.Handle(command, CancellationToken.None);

        _capturedRequest.Should().BeOfType<PatchApprenticeshipPaymentsApiRequest>();
        _capturedRequest.PatchUrl.Should().Be("api/apprenticeships/123/payments");

        var patchRequest = (PatchApprenticeshipPaymentsApiRequest)_capturedRequest;
        patchRequest.Data.PaymentFreezeDate.Should().Be(freezeDate);
        patchRequest.Data.FreezePaymentsReason.Should().Be(1);
        patchRequest.Data.Party.Should().Be((int)Party.Employer);
    }

    [Test]
    public async Task Handle_WhenUnfreezing_PatchesClearedPaymentFields()
    {
        var command = new ChangePaymentsCommand
        {
            ApprenticeshipId = 456,
            PaymentFreezeDate = null,
            FreezePaymentsReason = null
        };

        await _handler.Handle(command, CancellationToken.None);

        _capturedRequest.Should().BeOfType<PatchApprenticeshipPaymentsApiRequest>();
        _capturedRequest.PatchUrl.Should().Be("api/apprenticeships/456/payments");

        var patchRequest = (PatchApprenticeshipPaymentsApiRequest)_capturedRequest;
        patchRequest.Data.PaymentFreezeDate.Should().BeNull();
        patchRequest.Data.FreezePaymentsReason.Should().BeNull();
        patchRequest.Data.Party.Should().Be((int)Party.Employer);
    }
}
