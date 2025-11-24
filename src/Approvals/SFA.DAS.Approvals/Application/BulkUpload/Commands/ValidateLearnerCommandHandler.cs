using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi;
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
    ICourseTypeRulesService courseTypeRulesService)
    : IRequestHandler<ValidateLearnerCommand, LearnerValidateApiResponse>
{
    public async Task<LearnerValidateApiResponse> Handle(ValidateLearnerCommand command, CancellationToken cancellationToken)
    {

        var learnerData = await learnerDataClient.Get<GetLearnerForProviderResponse>(
            new GetLearnerForProviderRequest(
                command.ProviderId,
                command.LearnerDataId
            ));

        var standardResult = await providerStandardsService.GetStandardsDataForProviderAndCourse(command.ProviderId, learnerData.StandardCode);

        var courseTypeRules = await courseTypeRulesService.GetCourseTypeRulesAsync(learnerData.StandardCode.ToString());

        LearnerDataEnhanced learner = new LearnerDataEnhanced
        {
            Uln = learnerData.Uln,
            FirstName = learnerData.FirstName,
            LastName = learnerData.LastName,
            Email = learnerData.Email,
            Dob = learnerData.Dob,
            EpaoPrice = learnerData.EpaoPrice,
            TrainingPrice = learnerData.TrainingPrice,
            Cost = learnerData.TrainingPrice + learnerData.EpaoPrice,
            CourseCode = learnerData.StandardCode.ToString(),
            StandardsCode = standardResult?.IfateReferenceNumber,
            DeliveryModel = learnerData.IsFlexiJob ? DeliveryModel.FlexiJobAgency : DeliveryModel.Regular,
            StartDate = new DateTime(learnerData.StartDate.Year, learnerData.StartDate.Month, 1),
            ActualStartDate = learnerData.StartDate.Date,
            PlannedEndDate = new DateTime(learnerData.PlannedEndDate.Year, learnerData.PlannedEndDate.Month, 1),
            MinimumAgeAtApprenticeshipStart = courseTypeRules?.LearnerAgeRules?.MinimumAge,
            MaximumAgeAtApprenticeshipStart = courseTypeRules?.LearnerAgeRules?.MaximumAge
        };

        ValidateLearnerApiRequest request = new ValidateLearnerApiRequest
        {
            ProviderId = command.ProviderId,
            LearnerDataId = command.LearnerDataId,
            Learner = learner, //(must contain Max and Min Ages, OTJTH, StandardCode, RPL, and any other static info)
        };


        var response = await apiClient.PostWithResponseCode<LearnerValidateApiResponse>(
            new PostValidateLearnerRequest(command.ProviderId, command.LearnerDataId, request));

        return response.Body;
    }
}


public class LearnerValidateApiResponse
{
    public List<LearnerError> CriticalErrors { get; set; }
    public LearnerValidation LearnerValidation { get; set; }
}

public class LearnerValidation
{
    public LearnerValidation(long learnerDataId, List<LearnerError> errors)
    {
        LearnerDataId = learnerDataId;
        Errors = errors;
    }

    public long LearnerDataId { get; set; }
    public List<LearnerError> Errors { get; set; }
}

public class LearnerError
{
    public string Property { get; set; }
    public string ErrorText { get; set; }
}