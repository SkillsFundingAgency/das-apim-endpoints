﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.UpdateDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands
{
    [TestFixture]
    public class UpdateDraftApprenticeshipCommandHandlerTests
    {
        private UpdateDraftApprenticeshipCommandHandler _handler;
        private UpdateDraftApprenticeshipCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<UpdateDraftApprenticeshipCommand>();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _handler = new UpdateDraftApprenticeshipCommandHandler(_commitmentsApiClient.Object);
        }

        [Test]
        public async Task Handle_Update_Draft_Apprenticeship()
        {
            await _handler.Handle(_request, CancellationToken.None);

            _commitmentsApiClient.Verify(x => x.PutWithResponseCode<NullResponse>(
                    It.Is<PutUpdateDraftApprenticeshipRequest>(r =>
                            r.CohortId == _request.CohortId &&
                            r.ApprenticeshipId == _request.ApprenticeshipId &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).ActualStartDate == _request.ActualStartDate &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).Cost == _request.Cost &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).TrainingPrice == _request.TrainingPrice &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).EndPointAssessmentPrice == _request.EndPointAssessmentPrice &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).CourseCode == _request.CourseCode &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).DateOfBirth == _request.DateOfBirth &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).DeliveryModel == _request.DeliveryModel &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).Email == _request.Email &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).EmploymentEndDate == _request.EmploymentEndDate &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).EmploymentPrice == _request.EmploymentPrice &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).EndDate == _request.EndDate &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).FirstName == _request.FirstName &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).IgnoreStartDateOverlap == _request.IgnoreStartDateOverlap &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).IsOnFlexiPaymentPilot == _request.IsOnFlexiPaymentPilot &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).LastName == _request.LastName &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).CourseOption == _request.CourseOption &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).Reference == _request.Reference &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).ReservationId == _request.ReservationId &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).StartDate == _request.StartDate &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).Uln == _request.Uln &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).UserInfo == _request.UserInfo &&
                            ((UpdateDraftApprenticeshipRequest)r.Data).RequestingParty == _request.RequestingParty
                        )
                ), Times.Once);
        }
    }
}
