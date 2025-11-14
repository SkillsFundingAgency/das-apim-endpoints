using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands;

public class ValidateLearnerCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    IReservationApiClient<ReservationApiConfiguration> reservationApiClient,
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataClient,
    IProviderStandardsService providerStandardsService,
    IBulkCourseMetadataService bulkCourseMetadataService,
    IAddCourseTypeDataToCsvService courseTypesToCsvService)
    : IRequestHandler<ValidateLearnerCommand, Unit>
{
    public async Task<Unit> Handle(ValidateLearnerCommand command, CancellationToken cancellationToken)
    {
        var providerStandardResults = await providerStandardsService.GetStandardsData(command.ProviderId);

        var learnerData = await learnerDataClient.Get<GetLearnerForProviderResponse>(
            new GetLearnerForProviderRequest(
                command.ProviderId,
                command.LearnerDataId
            ));

        // We need to pick up the MinimumAgeAtApprenticeshipStart and MaximumAgeAtApprenticeshipStart from Course Type Data and pass that with the learnerData record (plus otjtraininghours
        // ideally we would do this in the Add Learner Data service
        //var codes = new List<string> { learnerData.StandardCode.ToString() };
        //var otjTrainingHours = await bulkCourseMetadataService.GetOtjTrainingHoursForBulkUploadAsync(codes);

        ValidateLearnerApiRequest request = new ValidateLearnerApiRequest
        {
            ProviderId = command.ProviderId,
            LearnerDataId = command.LearnerDataId,
            Learner = learnerData, //(must contain Max and Min Ages, OTJTH, StandardCode, RPL, and any other static info)
            ProviderStandardsData = providerStandardResults,
        };

        //if (!providerStandardResults.IsMainProvider)
        //{
        //    providerStandardResults.Standards = null;
        //}

        await apiClient.PostWithResponseCode<object>(
            new PostValidateLearnerRequest(command.ProviderId, request));
        return Unit.Value;
    }
}