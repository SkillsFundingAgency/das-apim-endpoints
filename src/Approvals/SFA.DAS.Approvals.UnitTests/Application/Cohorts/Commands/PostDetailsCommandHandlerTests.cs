using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Cohorts.Commands;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Cohorts;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts.Commands
{
    [TestFixture]
    public class PostDetailsCommandHandlerTests
    {
        private PostDetailsCommandHandler _handler;
        private ServiceParameters _serviceParameters;
        private PostDetailsCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private GetCohortResponse _cohort;
        private ApproveCohortRequest _approveCohortRequest;
        private SendCohortRequest _sendCohortRequest;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _request = fixture.Create<PostDetailsCommand>();
            _serviceParameters = new ServiceParameters(Approvals.Application.Shared.Enums.Party.Employer, fixture.Create<long>());

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _cohort = fixture.Create<GetCohortResponse>();
            _cohort.AccountId = _serviceParameters.CallingPartyId;

            _commitmentsApiClient.Setup(x => x.GetWithResponseCode<GetCohortResponse>(It.IsAny<GetCohortRequest>()))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(_cohort, HttpStatusCode.OK, string.Empty));

            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<EmptyResponse>(It.IsAny<ApproveCohortRequest>(), false))
                .Callback((IPostApiRequest request, bool includeResponse) => _approveCohortRequest = request as ApproveCohortRequest)
                .ReturnsAsync(new ApiResponse<EmptyResponse>(null, HttpStatusCode.OK, string.Empty));

            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<EmptyResponse>(It.IsAny<SendCohortRequest>(), false))
                .Callback((IPostApiRequest request, bool includeResponse) => _sendCohortRequest = request as SendCohortRequest)
                .ReturnsAsync(new ApiResponse<EmptyResponse>(null, HttpStatusCode.OK, string.Empty));

            _handler = new PostDetailsCommandHandler(_commitmentsApiClient.Object, _serviceParameters);
        }

        [Test]
        public void Handle_When_Cohort_Does_Not_Exist_ResourceNotFoundException_Is_Thrown()
        {
            _commitmentsApiClient.Setup(x => x.GetWithResponseCode<GetCohortResponse>(It.IsAny<GetCohortRequest>()))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(null, HttpStatusCode.NotFound, string.Empty));

            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _handler.Handle(_request, CancellationToken.None));
        }

        [Test]
        public void Handle_When_Cohort_Does_Not_Belong_To_Employer_ResourceNotFoundException_Is_Thrown()
        {
            _cohort.AccountId += 1;
            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _handler.Handle(_request, CancellationToken.None));
        }

        [Test]
        public void Handle_When_Cohort_Does_Not_Belong_To_Provider_ResourceNotFoundException_Is_Thrown()
        {
            _serviceParameters = new ServiceParameters(Approvals.Application.Shared.Enums.Party.Provider, 1);
            _handler = new PostDetailsCommandHandler(_commitmentsApiClient.Object, _serviceParameters);

            _cohort.ProviderId = 2;
            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _handler.Handle(_request, CancellationToken.None));
        }

        [Test]
        public async Task Handle_Creates_Approval_Request()
        {
            _request.SubmissionType = CohortSubmissionType.Approve;

            await _handler.Handle(_request, CancellationToken.None);

            var requestBody =_approveCohortRequest.Data as ApproveCohortRequest.Body;

            Assert.That(_approveCohortRequest.CohortId, Is.EqualTo(_request.CohortId));
            Assert.That(Approvals.Application.Shared.Enums.Party.Employer, Is.EqualTo(requestBody.RequestingParty));
            Assert.That(_request.Message, Is.EqualTo(requestBody.Message));
            Assert.That(_request.UserInfo.UserId, Is.EqualTo(requestBody.UserInfo.UserId));
            Assert.That(_request.UserInfo.UserDisplayName, Is.EqualTo(requestBody.UserInfo.UserDisplayName));
            Assert.That(_request.UserInfo.UserEmail, Is.EqualTo(requestBody.UserInfo.UserEmail));
        }

        [Test]
        public async Task Handle_Creates_Send_Request()
        {
            _request.SubmissionType = CohortSubmissionType.Send;

            await _handler.Handle(_request, CancellationToken.None);

            var requestBody = _sendCohortRequest.Data as SendCohortRequest.Body;

            Assert.That(_sendCohortRequest.CohortId, Is.EqualTo(_request.CohortId));
            Assert.That(Approvals.Application.Shared.Enums.Party.Employer, Is.EqualTo(requestBody.RequestingParty));
            Assert.That(_request.Message, Is.EqualTo(requestBody.Message));
            Assert.That(_request.UserInfo.UserId, Is.EqualTo(requestBody.UserInfo.UserId));
            Assert.That(_request.UserInfo.UserDisplayName, Is.EqualTo(requestBody.UserInfo.UserDisplayName));
            Assert.That(_request.UserInfo.UserEmail, Is.EqualTo(requestBody.UserInfo.UserEmail));
        }
    }
}
