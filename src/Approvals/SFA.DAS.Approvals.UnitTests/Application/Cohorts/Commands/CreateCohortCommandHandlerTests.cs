using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Application.Shared.LearnerDetailsValidation;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts.Commands
{
    [TestFixture]
    public class CreateCohortCommandHandlerTests
    {
        private CreateCohortCommandHandler _handler;
        private CreateCohortCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Mock<ILearnerDetailsValidator> _learnerDetailsValidator;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<CreateCohortCommand>();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _learnerDetailsValidator = new Mock<ILearnerDetailsValidator>();
            _learnerDetailsValidator
                .Setup(x => x.Validate(It.IsAny<ValidateLearnerDetailsRequest>()))
                .ReturnsAsync(It.IsAny<LearnerVerificationResponse>());

            _handler = new CreateCohortCommandHandler(_commitmentsApiClient.Object, _learnerDetailsValidator.Object);
        }

        [Test]
        public async Task Handle_Details_Validator_Called()
        {
            _commitmentsApiClient
            .Setup(x => x.PostWithResponseCode<CreateCohortResponse>(It.IsAny<PostCreateCohortRequest>(), true))
                .ReturnsAsync(_fixture.Create<ApiResponse<CreateCohortResponse>>());

            await _handler.Handle(_request, CancellationToken.None);

            _learnerDetailsValidator.Verify(x => x.Validate(
                It.Is<ValidateLearnerDetailsRequest>(r =>
                r.Uln == _request.Uln &&
                r.FirstName == _request.FirstName &&
                r.LastName == _request.LastName)
                ), Times.Once);
        }

        [Test]
        public async Task Handle_Cohort_Created()
        {
            var expectedResponse = _fixture.Create<CreateCohortResponse>();
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                    It.Is<PostCreateCohortRequest>(r =>
                            ((CreateCohortRequest)r.Data).AccountId == _request.AccountId &&
                            ((CreateCohortRequest)r.Data).AccountLegalEntityId == _request.AccountLegalEntityId &&
                            ((CreateCohortRequest)r.Data).ActualStartDate == _request.ActualStartDate &&
                            ((CreateCohortRequest)r.Data).Cost == _request.Cost &&
                            ((CreateCohortRequest)r.Data).CourseCode == _request.CourseCode &&
                            ((CreateCohortRequest)r.Data).DateOfBirth == _request.DateOfBirth &&
                            ((CreateCohortRequest)r.Data).DeliveryModel == _request.DeliveryModel &&
                            ((CreateCohortRequest)r.Data).Email == _request.Email &&
                            ((CreateCohortRequest)r.Data).EmploymentEndDate == _request.EmploymentEndDate &&
                            ((CreateCohortRequest)r.Data).EmploymentPrice == _request.EmploymentPrice &&
                            ((CreateCohortRequest)r.Data).EndDate == _request.EndDate &&
                            ((CreateCohortRequest)r.Data).FirstName == _request.FirstName &&
                            ((CreateCohortRequest)r.Data).IgnoreStartDateOverlap == _request.IgnoreStartDateOverlap &&
                            ((CreateCohortRequest)r.Data).IsOnFlexiPaymentPilot == _request.IsOnFlexiPaymentPilot &&
                            ((CreateCohortRequest)r.Data).LastName == _request.LastName &&
                            ((CreateCohortRequest)r.Data).OriginatorReference == _request.OriginatorReference &&
                            ((CreateCohortRequest)r.Data).PledgeApplicationId == _request.PledgeApplicationId &&
                            ((CreateCohortRequest)r.Data).ProviderId == _request.ProviderId &&
                            ((CreateCohortRequest)r.Data).ReservationId == _request.ReservationId &&
                            ((CreateCohortRequest)r.Data).StartDate == _request.StartDate &&
                            ((CreateCohortRequest)r.Data).TransferSenderId == _request.TransferSenderId &&
                            ((CreateCohortRequest)r.Data).Uln == _request.Uln &&
                            ((CreateCohortRequest)r.Data).UserInfo == _request.UserInfo
                        ), true
                )).ReturnsAsync(new ApiResponse<CreateCohortResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            var response = await _handler.Handle(_request, CancellationToken.None);

            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}