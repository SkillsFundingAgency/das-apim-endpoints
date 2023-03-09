using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands
{
    [TestFixture]
    public class AddDraftApprenticeshipCommandHandlerTests
    {
        private AddDraftApprenticeshipCommandHandler _handler;
        private AddDraftApprenticeshipCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<AddDraftApprenticeshipCommand>();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _handler = new AddDraftApprenticeshipCommandHandler(_commitmentsApiClient.Object);
        }

        [Test]
        public async Task Handle_Cohort_Created()
        {
            var expectedResponse = _fixture.Create<AddDraftApprenticeshipResponse>();
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(
                    It.Is<PostAddDraftApprenticeshipRequest>(r =>
                            ((AddDraftApprenticeshipRequest)r.Data).ActualStartDate == _request.ActualStartDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).Cost == _request.Cost &&
                            ((AddDraftApprenticeshipRequest)r.Data).CourseCode == _request.CourseCode &&
                            ((AddDraftApprenticeshipRequest)r.Data).DateOfBirth == _request.DateOfBirth &&
                            ((AddDraftApprenticeshipRequest)r.Data).DeliveryModel == _request.DeliveryModel &&
                            ((AddDraftApprenticeshipRequest)r.Data).Email == _request.Email &&
                            ((AddDraftApprenticeshipRequest)r.Data).EmploymentEndDate == _request.EmploymentEndDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).EmploymentPrice == _request.EmploymentPrice &&
                            ((AddDraftApprenticeshipRequest)r.Data).EndDate == _request.EndDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).FirstName == _request.FirstName &&
                            ((AddDraftApprenticeshipRequest)r.Data).IgnoreStartDateOverlap == _request.IgnoreStartDateOverlap &&
                            ((AddDraftApprenticeshipRequest)r.Data).IsOnFlexiPaymentPilot == _request.IsOnFlexiPaymentPilot &&
                            ((AddDraftApprenticeshipRequest)r.Data).LastName == _request.LastName &&
                            ((AddDraftApprenticeshipRequest)r.Data).OriginatorReference == _request.OriginatorReference &&
                            ((AddDraftApprenticeshipRequest)r.Data).ProviderId == _request.ProviderId &&
                            ((AddDraftApprenticeshipRequest)r.Data).ReservationId == _request.ReservationId &&
                            ((AddDraftApprenticeshipRequest)r.Data).StartDate == _request.StartDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).Uln == _request.Uln &&
                            ((AddDraftApprenticeshipRequest)r.Data).UserInfo == _request.UserInfo &&
                            ((AddDraftApprenticeshipRequest)r.Data).UserId == _request.UserId
                        ), true
                )).ReturnsAsync(new ApiResponse<AddDraftApprenticeshipResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            var response = await _handler.Handle(_request, CancellationToken.None);

            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
