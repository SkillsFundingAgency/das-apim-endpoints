using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddEmail;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands;

public class DraftApprenticeshipAddEmailCommandHandlerTests
{
    private DraftApprenticeshipAddEmailRequest _request;
    private DraftApprenticeshipAddEmailCommand command;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient;
    private DraftApprenticeshipAddEmailCommandHandler _handler;
    private ServiceParameters _serviceParameters;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        _request = fixture.Create<DraftApprenticeshipAddEmailRequest>();
        command = fixture.Create<DraftApprenticeshipAddEmailCommand>();
        _serviceParameters = fixture.Create<ServiceParameters>();
        apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _handler = new DraftApprenticeshipAddEmailCommandHandler(apiClient.Object, _serviceParameters);
    }

    [Test]
    public async Task Then_The_Api_Is_Called_With_A_Valid_Request()
    {
        apiClient.Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<DraftApprenticeshipAddEmailRequest>()));

        await _handler.Handle(command, CancellationToken.None);        

        apiClient.Verify(x => x.PutWithResponseCode<NullResponse>(It.Is<DraftApprenticeshipAddEmailRequest>
            (r => r.DraftApprenticeshipId == command.DraftApprenticeshipId &&
             r.CohortId == command.CohortId &&
             ((DraftApprenticeshipAddEmailRequest.Body)r.Data).Email == command.Email
        )));
    }
}