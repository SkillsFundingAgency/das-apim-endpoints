using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeships;
using SFA.DAS.Approvals.Enums;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;

public class GetApprenticeshipsRequest(long? providerId, long? accountId, GetApprenticeshipsQuery request) : IGetApiRequest
{
    public long? AccountId { get; set; } = accountId;

    public long? ProviderId { get; set; } = providerId;

    public int PageNumber { get; set; } = request.PageNumber;

    public int PageItemCount { get; set; } = request.PageItemCount;

    public string SortField { get; set; } = request.SortField;

    public bool ReverseSort { get; set; } = request.ReverseSort;

    public string SearchTerm { get; set; } = request.SearchTerm;

    public string EmployerName { get; set; } = request.EmployerName;

    public string ProviderName { get; set; } = request.ProviderName;

    public string CourseName { get; set; } = request.CourseName;

    public ApprenticeshipStatus? Status { get; set; } = request.Status;

    public DateTime? StartDate { get; set; } = request.StartDate;

    public DateTime? EndDate { get; set; } = request.EndDate;

    public int? AccountLegalEntityId { get; set; }

    public DateTime? StartDateRangeFrom { get; set; }

    public DateTime? StartDateRangeTo { get; set; }

    public Alerts? Alert { get; set; } = request.Alert;

    public ConfirmationStatus? ApprenticeConfirmationStatus { get; set; } = request.ApprenticeConfirmationStatus;

    public DeliveryModel? DeliveryModel { get; set; } = request.DeliveryModel;
    public string GetUrl => QueryHelpers.AddQueryString($"api/apprenticeships", CreateFilterQuery(this));

    private static Dictionary<string, string> CreateFilterQuery(GetApprenticeshipsRequest request)
    {
        var queryParameters = new Dictionary<string, string>();

        if (request.AccountId.HasValue)
            queryParameters.Add("accountId", request.AccountId.Value.ToString());

        if (request.ProviderId.HasValue)
            queryParameters.Add("providerId", request.ProviderId.Value.ToString());

        if (!string.IsNullOrEmpty(request.SearchTerm))
            queryParameters.Add("searchTerm", request.SearchTerm);

        if (!string.IsNullOrEmpty(request.EmployerName))
            queryParameters.Add("employerName", request.EmployerName);

        if (request.PageNumber > 0)
            queryParameters.Add("pageNumber", request.PageNumber.ToString());

        if (request.PageItemCount > 0)
            queryParameters.Add("pageItemCount", request.PageItemCount.ToString());

        if (!string.IsNullOrEmpty(request.SortField))
            queryParameters.Add("sortField", request.SortField);

        if (request.ReverseSort)
            queryParameters.Add("reverseSort", request.ReverseSort.ToString().ToLower());

        if (!string.IsNullOrEmpty(request.CourseName))
            queryParameters.Add("courseName", request.CourseName);

        if (!string.IsNullOrEmpty(request.ProviderName))
            queryParameters.Add("providerName", request.ProviderName);

        if (request.Status.HasValue)
            queryParameters.Add("status", request.Status.Value.ToString());

        if (request.StartDate.HasValue)
            queryParameters.Add("startDate", request.StartDate.Value.ToString("u"));

        if (request.EndDate.HasValue)
            queryParameters.Add("endDate", request.EndDate.Value.ToString("u"));

        if (request.StartDateRangeFrom.HasValue)
            queryParameters.Add("startDateRangeFrom", request.StartDateRangeFrom.Value.ToString("u"));

        if (request.StartDateRangeTo.HasValue)
            queryParameters.Add("startDateRangeTo", request.StartDateRangeTo.Value.ToString("u"));

        if (request.Alert.HasValue)
            queryParameters.Add("alert", request.Alert.Value.ToString());

        if (request.ApprenticeConfirmationStatus.HasValue)
            queryParameters.Add("apprenticeConfirmationStatus", request.ApprenticeConfirmationStatus.ToString());

        if (request.DeliveryModel.HasValue)
            queryParameters.Add("deliveryModel", request.DeliveryModel.ToString());

        return queryParameters;
    }
}