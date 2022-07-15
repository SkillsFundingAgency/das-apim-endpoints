using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Apprentices.Commands.ChangeEmployer.Confirm;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Commands
{
    [TestFixture]
    public class ConfirmCommandHandlerTests
    {
        private ConfirmCommandHandler _handler;
        private ConfirmCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private CreateChangeOfPartyRequestRequest _apiRequest;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _request = fixture.Create<ConfirmCommand>();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateChangeOfPartyRequestResponse>(It.IsAny<CreateChangeOfPartyRequestRequest>()))
                .Callback<IPostApiRequest>(request => _apiRequest = request as CreateChangeOfPartyRequestRequest)
                .ReturnsAsync(new ApiResponse<CreateChangeOfPartyRequestResponse>(null, HttpStatusCode.OK, string.Empty));

            _handler = new ConfirmCommandHandler(_commitmentsApiClient.Object);
        }

        [Test]
        public async Task Handle_Creates_Change_Of_Party_Request()
        {
            await _handler.Handle(_request, CancellationToken.None);

            var requestBody = _apiRequest.Data as CreateChangeOfPartyRequestRequest.Body;

            Assert.AreEqual(_request.ApprenticeshipId, _apiRequest.ApprenticeshipId);
            Assert.AreEqual(ChangeOfPartyRequestType.ChangeEmployer, requestBody.ChangeOfPartyRequestType);
            Assert.AreEqual(_request.DeliveryModel, requestBody.DeliveryModel);
            Assert.AreEqual(_request.EmploymentEndDate, requestBody.NewEmploymentEndDate);
            Assert.AreEqual(_request.EmploymentPrice, requestBody.NewEmploymentPrice);
            Assert.AreEqual(_request.EndDate, requestBody.NewEndDate);
            Assert.AreEqual(_request.Price, requestBody.NewPrice);
            Assert.AreEqual(_request.StartDate, requestBody.NewStartDate);
            Assert.AreEqual(_request.UserInfo, requestBody.UserInfo);
            Assert.AreEqual(_request.AccountLegalEntityId, requestBody.NewPartyId);
        }
    }
}
