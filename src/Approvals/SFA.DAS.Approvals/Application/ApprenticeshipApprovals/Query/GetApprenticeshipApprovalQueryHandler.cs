using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Query;

public class GetApprenticeshipApprovalQueryHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient) : IRequestHandler<GetApprenticeshipApprovalQuery, GetApprenticeshipApprovalResponse>
{
    public async Task<GetApprenticeshipApprovalResponse> Handle(GetApprenticeshipApprovalQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.GetWithResponseCode<GetApprenticeshipApprovalResponse>(new GetApprenticeshipApprovalRequest(request.ApprenticeshipId, request.ApprovalRequestId));

        if (result.StatusCode == HttpStatusCode.NotFound) 
            return null;

        if(result.StatusCode == HttpStatusCode.OK)
        {
            var response = result.Body;

            if (response.AccountId != request.EmployerAccountId)
                throw new UnauthorizedAccessException("This Employer does not have access to this apprenticeship approval.");

            var course = await apiClient.Get<GetTrainingProgrammeResponse>(new GetTrainingProgrammeRequest(response.CourseCode));

            if (course == null || !response.StartDate.HasValue)
                return response;    

            var fundingCap = MaxFundingOn(course.TrainingProgramme.FundingPeriods, response.StartDate.Value);

            response.FundingCap = fundingCap;
            response.ExceedsFundingCap = IsFundingBandExceeded(response, fundingCap);

            return response;
        }

        throw new Exception("An unexpected Status code was returned from the API.");
    }

    public static int? MaxFundingOn(List<TrainingProgrammeFundingPeriod> funding, DateTime effectiveDate)
    {
        if (funding == null || !funding.Any()) return null;

        var match = funding.FirstOrDefault(c =>
            c.EffectiveFrom <= effectiveDate
            && (c.EffectiveTo == null || c.EffectiveTo >= effectiveDate));

        if (match == null)
            match = funding.FirstOrDefault(c => c.EffectiveTo == null);

        return match?.FundingCap
               ?? funding.FirstOrDefault()?.FundingCap
               ?? null;
    }

    public static bool IsFundingBandExceeded(GetApprenticeshipApprovalResponse funding, int? fundingBand)
    {

        if (fundingBand == null)
            return false;
        var tnp1 = funding.Items.FirstOrDefault(c => c.FieldName == "TNP1");
        var tnp2 = funding.Items.FirstOrDefault(c => c.FieldName == "TNP2");

        return ToInt(tnp1?.NewValue) + ToInt(tnp2?.NewValue) > fundingBand;
    }

    public static int ToInt(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;
        if (int.TryParse(value, out var result))
            return result;
        return 0;
    }
}