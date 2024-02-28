using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.ChangeEmployer.Confirm
{
    public class CreateChangeOfEmployerCommandHandler : IRequestHandler<CreateChangeOfEmployerCommand, Unit>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;

        public CreateChangeOfEmployerCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<Unit> Handle(CreateChangeOfEmployerCommand request, CancellationToken cancellationToken)
        {
            var body = new CreateChangeOfPartyRequestRequest.Body
            {
                ChangeOfPartyRequestType = ChangeOfPartyRequestType.ChangeEmployer,
                NewPartyId = request.AccountLegalEntityId,
                NewPrice = request.Price,
                NewStartDate = request.StartDate,
                NewEndDate = request.EndDate,
                NewEmploymentEndDate = request.EmploymentEndDate,
                NewEmploymentPrice = request.EmploymentPrice,
                DeliveryModel = request.DeliveryModel,
                HasOverlappingTrainingDates = request.HasOverlappingTrainingDates,
                UserInfo = request.UserInfo
            };

            var apiRequest = new CreateChangeOfPartyRequestRequest(request.ApprenticeshipId, body);

            var response = await _commitmentsApiClient.PostWithResponseCode<CreateChangeOfPartyRequestResponse>(apiRequest, false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
