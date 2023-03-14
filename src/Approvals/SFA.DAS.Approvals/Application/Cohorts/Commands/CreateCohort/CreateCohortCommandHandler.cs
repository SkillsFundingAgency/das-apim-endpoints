using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Application.Shared.LearnerDetailsValidation;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort
{
    public class CreateCohortCommandHandler : IRequestHandler<CreateCohortCommand, CreateCohortResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly ILearnerDetailsValidator _detailsValidator;

        public CreateCohortCommandHandler(ILearnerDetailsValidator detailsValidator)
        {
            _detailsValidator = detailsValidator;
        }

        public CreateCohortCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateCohortResult> Handle(CreateCohortCommand request, CancellationToken cancellationToken)
        {
            //TODO: Verify if we need to add UKPRN to request?
            var validateDetailsRequest = new ValidateLearnerDetailsRequest() { Uln = request.Uln, FirstName = request.FirstName, LastName = request.LastName };
            var validationResult = await _detailsValidator.Validate(validateDetailsRequest);

            var createCohortRequest = new CreateCohortRequest
            {
                AccountId = request.AccountId,
                AccountLegalEntityId = request.AccountLegalEntityId,
                ActualStartDate = request.ActualStartDate,
                Cost = request.Cost,
                CourseCode = request.CourseCode,
                DateOfBirth = request.DateOfBirth,
                DeliveryModel = request.DeliveryModel,
                Email = request.Email,
                EmploymentEndDate = request.EmploymentEndDate,
                EmploymentPrice = request.EmploymentPrice,
                EndDate = request.EndDate,
                FirstName = request.FirstName,
                IgnoreStartDateOverlap = request.IgnoreStartDateOverlap,
                IsOnFlexiPaymentPilot = request.IsOnFlexiPaymentPilot,
                LastName = request.LastName,
                OriginatorReference = request.OriginatorReference,
                PledgeApplicationId = request.PledgeApplicationId,
                ProviderId = request.ProviderId,
                ReservationId = request.ReservationId,
                StartDate = request.StartDate,
                TransferSenderId = request.TransferSenderId,
                Uln = request.Uln,
                UserInfo = request.UserInfo,
                LearnerVerificationResponse = validationResult
            };
            var createCohortResponse = await _apiClient.PostWithResponseCode<CreateCohortResponse>(new PostCreateCohortRequest(createCohortRequest));
            createCohortResponse.EnsureSuccessStatusCode();

            return new CreateCohortResult
            {
                CohortId = createCohortResponse.Body.CohortId,
                CohortReference = createCohortResponse.Body.CohortReference
            };
        }
    }
}