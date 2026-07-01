using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetChangePayments;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using Party = SFA.DAS.Approvals.InnerApi.Responses.Party;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries;

[TestFixture]
public class GetChangePaymentsQueryHandlerTests
{
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
    private GetChangePaymentsQueryHandler _handler;
    private GetApprenticeshipResponse _apprenticeship;
    private GetChangePaymentsQuery _query;

    [SetUp]
    public void SetUp()
    {
        var fixture = new Fixture();

        _apprenticeship = fixture.Build<GetApprenticeshipResponse>()
            .With(x => x.EmployerAccountId, 123)
            .Without(x => x.PaymentFreezeDate)
            .Create();

        _query = fixture.Create<GetChangePaymentsQuery>();

        _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipResponse>(It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));

        var serviceParameters = new ServiceParameters((Approvals.Application.Shared.Enums.Party)Party.Employer, 123);
        _handler = new GetChangePaymentsQueryHandler(_apiClient.Object, serviceParameters);
    }

    [Test]
    public async Task Handle_MapsApprenticeshipDetails()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.FirstName.Should().Be(_apprenticeship.FirstName);
        result.LastName.Should().Be(_apprenticeship.LastName);
        result.Uln.Should().Be(_apprenticeship.Uln);
        result.CourseName.Should().Be(_apprenticeship.CourseName);
    }

    [Test]
    public async Task Handle_WhenPaymentsNotFrozen_MapsFreezeStatusFalse()
    {
        _apprenticeship.PaymentFreezeDate = null;

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.FreezeStatus.Should().BeFalse();
        result.PaymentFreezeDate.Should().BeNull();
    }

    [Test]
    public async Task Handle_WhenPaymentsFrozen_MapsPaymentFreezeDate()
    {
        var freezeDate = new DateTime(2026, 1, 5);
        _apprenticeship.PaymentFreezeDate = freezeDate;

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.FreezeStatus.Should().BeTrue();
        result.PaymentFreezeDate.Should().Be(freezeDate);
    }

    [Test]
    public async Task Handle_WhenApprenticeshipNotFound_ReturnsNull()
    {
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(null, HttpStatusCode.NotFound, string.Empty));

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().BeNull();
    }
}
