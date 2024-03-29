﻿using System.Net;
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
        private CreateChangeOfEmployerCommandHandler _handler;
        private CreateChangeOfEmployerCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private CreateChangeOfPartyRequestRequest _apiRequest;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _request = fixture.Create<CreateChangeOfEmployerCommand>();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateChangeOfPartyRequestResponse>(It.IsAny<CreateChangeOfPartyRequestRequest>(), false))
                .Callback((IPostApiRequest request, bool includeResponse) => _apiRequest = request as CreateChangeOfPartyRequestRequest)
                .ReturnsAsync(new ApiResponse<CreateChangeOfPartyRequestResponse>(null, HttpStatusCode.OK, string.Empty));

            _handler = new CreateChangeOfEmployerCommandHandler(_commitmentsApiClient.Object);
        }

        [Test]
        public async Task Handle_Creates_Change_Of_Party_Request()
        {
            await _handler.Handle(_request, CancellationToken.None);

            var requestBody = _apiRequest.Data as CreateChangeOfPartyRequestRequest.Body;

            Assert.That(_apiRequest.ApprenticeshipId, Is.EqualTo(_request.ApprenticeshipId));
            Assert.That(requestBody.ChangeOfPartyRequestType, Is.EqualTo(ChangeOfPartyRequestType.ChangeEmployer));
            Assert.That(requestBody.DeliveryModel, Is.EqualTo(_request.DeliveryModel));
            Assert.That(requestBody.NewEmploymentEndDate, Is.EqualTo(_request.EmploymentEndDate));
            Assert.That(requestBody.NewEmploymentPrice, Is.EqualTo(_request.EmploymentPrice));
            Assert.That(requestBody.NewEndDate, Is.EqualTo(_request.EndDate));
            Assert.That(requestBody.NewPrice, Is.EqualTo(_request.Price));
            Assert.That(requestBody.NewStartDate, Is.EqualTo(_request.StartDate));
            Assert.That(requestBody.HasOverlappingTrainingDates, Is.EqualTo(_request.HasOverlappingTrainingDates));
            Assert.That(requestBody.UserInfo, Is.EqualTo(_request.UserInfo));
            Assert.That(requestBody.NewPartyId, Is.EqualTo(_request.AccountLegalEntityId));
        }
    }
}
