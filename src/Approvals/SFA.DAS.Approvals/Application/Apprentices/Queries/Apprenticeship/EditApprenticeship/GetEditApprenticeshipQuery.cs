using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Constants;
using System.Collections.Generic;

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
        public short Status { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public LearningType? LearningType { get; set; }
        public List<string> LockedManualFieldsAndFeatures { get; set; }
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
            var innerApiTask = _apiClient.GetWithResponseCode<GetApprenticeshipResponse>(innerApiRequest);

            var fieldLocksRequest = new GetEditApprenticeshipFieldAndFeatureLocksRequest(request.ApprenticeshipId);
            var fieldLocksTask = _apiClient.GetWithResponseCode<GetEditApprenticeshipFieldAndFeatureLocksResponse>(fieldLocksRequest);

            await Task.WhenAll(innerApiTask, fieldLocksTask);

            var innerApiResponse = await innerApiTask;
            var fieldLocksResponse = await fieldLocksTask;

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
                HasMultipleDeliveryModelOptions = deliveryModel.Count > 1,
                Status = apprenticeship.Status, 
                LearningType = apprenticeship.LearningType,
                LockedManualFieldsAndFeatures = fieldLocksResponse.Body?.LockedManualFieldsAndFeatures ?? new List<string>()
            };
        }
    }
}
