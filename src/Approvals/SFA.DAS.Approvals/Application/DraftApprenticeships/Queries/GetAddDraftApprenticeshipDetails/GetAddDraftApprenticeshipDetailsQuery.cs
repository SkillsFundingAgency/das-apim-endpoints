﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetAddDraftApprenticeshipDetails
{
    public class GetAddDraftApprenticeshipDetailsQuery : IRequest<GetAddDraftApprenticeshipDetailsQueryResult>
    {
        public long CohortId { get; set; }
        public string CourseCode { get; set; }
        public DateTime? StartDate { get; set; }
    }

    public class GetAddDraftApprenticeshipDetailsQueryResult
    {
        public long AccountLegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public string ProviderName { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
        public bool IsFundedByTransfer { get; set; }
        public long? TransferSenderId { get; set; }
        public string StandardPageUrl { get; set; }
        public int? ProposedMaxFunding { get; set; }
    }

    public class GetAddDraftApprenticeshipDetailsQueryHandler : IRequestHandler<GetAddDraftApprenticeshipDetailsQuery, GetAddDraftApprenticeshipDetailsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ServiceParameters _serviceParameters;

        public GetAddDraftApprenticeshipDetailsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetAddDraftApprenticeshipDetailsQueryResult> Handle(GetAddDraftApprenticeshipDetailsQuery request, CancellationToken cancellationToken)
        {
            var cohortRequest = new GetCohortRequest(request.CohortId);
            var cohortResponseTask = _apiClient.GetWithResponseCode<GetCohortResponse>(cohortRequest);
            var courseTask = _apiClient.Get<GetTrainingProgrammeResponse>(new GetCalculatedVersionOfTrainingProgrammeRequest(request.CourseCode, request.StartDate));

            await Task.WhenAll(cohortResponseTask, courseTask);
            var cohortResponse = await cohortResponseTask;
            var course = (await courseTask)?.TrainingProgramme;

            if (cohortResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            cohortResponse.EnsureSuccessStatusCode();

            var cohort = cohortResponse.Body;

            if (!cohort.CheckParty(_serviceParameters))
            {
                return null;
            }
            var deliveryModels = await _deliveryModelService.GetDeliveryModels(cohort.ProviderId,
                request.CourseCode, cohort.AccountLegalEntityId, null);

            return new GetAddDraftApprenticeshipDetailsQueryResult
            {
                AccountLegalEntityId = cohort.AccountLegalEntityId,
                LegalEntityName = cohort.LegalEntityName,
                ProviderName = cohort.ProviderName,
                HasMultipleDeliveryModelOptions = deliveryModels.Count > 1,
                IsFundedByTransfer = cohort.IsFundedByTransfer,
                TransferSenderId = cohort.TransferSenderId,
                StandardPageUrl = course?.StandardPageUrl,
                ProposedMaxFunding = course?.FundingPeriods.GetFundingBandForDate(request.StartDate)
            };
        }
    }
}
