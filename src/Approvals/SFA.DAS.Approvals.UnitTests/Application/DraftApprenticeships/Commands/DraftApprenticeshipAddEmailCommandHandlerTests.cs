using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddEmail;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands;

public class DraftApprenticeshipAddEmailCommandHandlerTests
{
    private DraftApprenticeshipAddEmailRequest _request;
    private DraftApprenticeshipAddEmailCommand command;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient;
    private DraftApprenticeshipAddEmailCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        _request = fixture.Create<DraftApprenticeshipAddEmailRequest>();
        command = fixture.Create<DraftApprenticeshipAddEmailCommand>();
        apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _handler = new DraftApprenticeshipAddEmailCommandHandler(apiClient.Object);
    }


    [Test]
    public async Task Then_The_Api_Is_Called_With_A_Valid_Request()
    {
        var response = new ApiResponse<DraftApprenticeshipAddEmailResponse>(new DraftApprenticeshipAddEmailResponse() { DraftApprenticeshipId = 1}, System.Net.HttpStatusCode.OK, null);

        apiClient.Setup(x => x.PostWithResponseCode<DraftApprenticeshipAddEmailResponse>(It.IsAny<DraftApprenticeshipAddEmailRequest>(), true))
          .ReturnsAsync(response);

        var actual = await _handler.Handle(command, CancellationToken.None);

        Assert.That(actual, Is.Not.Null);

        apiClient.Verify(x => x.PostWithResponseCode<DraftApprenticeshipAddEmailResponse>(It.Is<DraftApprenticeshipAddEmailRequest>
            (r => r.DraftApprenticeshipId == command.DraftApprenticeshipId &&
             r.CohortId == command.CohortId &&
             ((DraftApprenticeshipAddEmailRequest.Body)r.Data).CohortId == command.CohortId &&
             ((DraftApprenticeshipAddEmailRequest.Body)r.Data).Email == command.Email
        ), true));
    }
}

