using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.Reference;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands;

public class DraftApprenticeshipSetReferenceCommandHandlerTests
{
    private DraftApprenticeshipSetReferenceRequest _request;
    private DraftApprenticeshipSetReferenceCommand command;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient;
    private DraftApprenticeshipSetReferenceCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        _request = fixture.Create<DraftApprenticeshipSetReferenceRequest>();
        command = fixture.Create<DraftApprenticeshipSetReferenceCommand>();
        apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _handler = new DraftApprenticeshipSetReferenceCommandHandler(apiClient.Object);
    }


    [Test]
    public async Task Then_The_Api_Is_Called_With_A_Valid_Request()
    {
        var response = new ApiResponse<DraftApprenticeshipSetReferenceResponse>(new DraftApprenticeshipSetReferenceResponse() { DraftApprenticeshipId = 1 }, System.Net.HttpStatusCode.OK, null);

        apiClient.Setup(x => x.PostWithResponseCode<DraftApprenticeshipSetReferenceResponse>(It.IsAny<DraftApprenticeshipSetReferenceRequest>(), true))
          .ReturnsAsync(response);

        var actual = await _handler.Handle(command, CancellationToken.None);

        Assert.That(actual, Is.Not.Null);

        apiClient.Verify(x => x.PostWithResponseCode<DraftApprenticeshipSetReferenceResponse>(It.Is<DraftApprenticeshipSetReferenceRequest>
            (r => r.DraftApprenticeshipId == command.DraftApprenticeshipId &&
             r.CohortId == command.CohortId &&
             ((DraftApprenticeshipSetReferenceRequest.Body)r.Data).CohortId == command.CohortId &&
             ((DraftApprenticeshipSetReferenceRequest.Body)r.Data).Reference == command.Reference
        ), true));
    }
}
