using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.EditApprenticeship
{
    public class GetEditApprenticeshipQuery : IRequest<GetEditApprenticeshipQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }

    public class GetEditApprenticeshipQueryResult
    {
        public string CourseName { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
        public bool IsFundedByTransfer { get; set; }
    }

    public class GetEditApprenticeshipQueryHandler : IRequestHandler<GetEditApprenticeshipQuery, GetEditApprenticeshipQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ServiceParameters _serviceParameters;

        public GetEditApprenticeshipQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetEditApprenticeshipQueryResult> Handle(GetEditApprenticeshipQuery request, CancellationToken cancellationToken)
        {
            var innerApiRequest = new GetApprenticeshipRequest(request.ApprenticeshipId);
            var innerApiResponse = await _apiClient.GetWithResponseCode<GetApprenticeshipResponse>(innerApiRequest);

            if (innerApiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            innerApiResponse.EnsureSuccessStatusCode();
            var apprenticeship = innerApiResponse.Body;

            if (!apprenticeship.CheckParty(_serviceParameters))
            {
                return null;
            }

            var deliveryModel = await _deliveryModelService.GetDeliveryModels(apprenticeship);

            return new GetEditApprenticeshipQueryResult
            {
                CourseName = apprenticeship.CourseName,
                IsFundedByTransfer = apprenticeship.TransferSenderId.HasValue,
                HasMultipleDeliveryModelOptions = deliveryModel.Count > 1
            };
        }
    }
}
