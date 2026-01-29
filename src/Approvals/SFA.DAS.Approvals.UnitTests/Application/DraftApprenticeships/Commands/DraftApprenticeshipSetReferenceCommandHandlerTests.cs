using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.Reference;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands;

public class DraftApprenticeshipSetReferenceCommandHandlerTests
{
    private DraftApprenticeshipSetReferenceRequest _request;
    private DraftApprenticeshipSetReferenceCommand command;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient;
    private DraftApprenticeshipSetReferenceCommandHandler _handler;
    private ServiceParameters serviceParameters; 

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        _request = fixture.Create<DraftApprenticeshipSetReferenceRequest>();
        command = fixture.Create<DraftApprenticeshipSetReferenceCommand>();
        apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        serviceParameters = new ServiceParameters(Party.Provider, 1100090);
        _handler = new DraftApprenticeshipSetReferenceCommandHandler(apiClient.Object, serviceParameters);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_A_Valid_Request()
    {
        apiClient.Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<DraftApprenticeshipSetReferenceRequest>()));

        await _handler.Handle(command, CancellationToken.None);

        apiClient.Verify(x => x.PutWithResponseCode<NullResponse>(It.Is<DraftApprenticeshipSetReferenceRequest>
            (r => r.DraftApprenticeshipId == command.DraftApprenticeshipId &&
             r.CohortId == command.CohortId &&
             ((DraftApprenticeshipSetReferenceRequest.Body)r.Data).Party == serviceParameters.CallingParty &&
             ((DraftApprenticeshipSetReferenceRequest.Body)r.Data).Reference == command.Reference)
            ));
    }
}
