using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeships;
using SFA.DAS.Approvals.Enums;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;

public class GetApprenticeshipsRequest(long providerId, GetApprenticeshipsQuery request) : IGetApiRequest
{
    public long? AccountId { get; set; }

    public long? ProviderId { get; set; } = providerId;

    public int PageNumber { get; set; } = request.PageNumber;

    public int PageItemCount { get; set; } = request.PageItemCount;

    public string SortField { get; set; } = request.SortField;

    public bool ReverseSort { get; set; } = request.ReverseSort;

    public string SearchTerm { get; set; } = request.SearchTerm;

    public string EmployerName { get; set; } = request.EmployerName;

    public string ProviderName { get; set; }

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
    public string GetUrl => $"api/apprenticeships?providerId={ProviderId}{CreateFilterQuery(this)}";

    private static string CreateFilterQuery(GetApprenticeshipsRequest request)
    {
        var queryParameters = new List<string>();

        if (!string.IsNullOrEmpty(request.SearchTerm))
            queryParameters.Add($"searchTerm={WebUtility.UrlEncode(request.SearchTerm)}");

        if (!string.IsNullOrEmpty(request.EmployerName))
            queryParameters.Add($"employerName={WebUtility.UrlEncode(request.EmployerName)}");

        if (request.PageNumber > 0)
            queryParameters.Add($"pageNumber={request.PageNumber}");

        if (request.PageItemCount > 0)
            queryParameters.Add($"pageItemCount={request.PageItemCount}");

        if (!string.IsNullOrEmpty(request.SortField))
            queryParameters.Add($"sortField={WebUtility.UrlEncode(request.SortField)}");

        if (request.ReverseSort)
            queryParameters.Add($"reverseSort={request.ReverseSort.ToString().ToLower()}");

        if (!string.IsNullOrEmpty(request.CourseName))
            queryParameters.Add($"courseName={WebUtility.UrlEncode(request.CourseName)}");

        if (!string.IsNullOrEmpty(request.ProviderName))
            queryParameters.Add($"providerName={WebUtility.UrlEncode(request.ProviderName)}");

        if (request.Status.HasValue)
            queryParameters.Add($"status={WebUtility.UrlEncode(request.Status.Value.ToString())}");

        if (request.StartDate.HasValue)
            queryParameters.Add($"startDate={WebUtility.UrlEncode(request.StartDate.Value.ToString("u"))}");

        if (request.EndDate.HasValue)
            queryParameters.Add($"endDate={WebUtility.UrlEncode(request.EndDate.Value.ToString("u"))}");

        if (request.StartDateRangeFrom.HasValue)
            queryParameters.Add($"startDateRangeFrom={WebUtility.UrlEncode(request.StartDateRangeFrom.Value.ToString("u"))}");

        if (request.StartDateRangeTo.HasValue)
            queryParameters.Add($"startDateRangeTo={WebUtility.UrlEncode(request.StartDateRangeTo.Value.ToString("u"))}");

        if (request.Alert.HasValue)
            queryParameters.Add($"alert={WebUtility.UrlEncode(request.Alert.Value.ToString())}");

        if (request.ApprenticeConfirmationStatus.HasValue)
            queryParameters.Add($"apprenticeConfirmationStatus={WebUtility.UrlEncode(request.ApprenticeConfirmationStatus.ToString())}");

        if (request.DeliveryModel.HasValue)
            queryParameters.Add($"deliveryModel={WebUtility.UrlEncode(request.DeliveryModel.ToString())}");

        return queryParameters.Any() ? "&" + string.Join("&", queryParameters) : string.Empty;
    }
}