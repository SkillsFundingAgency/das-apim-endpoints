using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class GetApprenticeshipsCSVRequest : IGetApiRequest
    {
        public long? ProviderId { get; set; }

        public string SearchTerm { get; set; }

        public string EmployerName { get; set; }

        public string CourseName { get; set; }

        public ApprenticeshipStatus? Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Alerts? Alert { get; set; }

        public ConfirmationStatus? ApprenticeConfirmationStatus { get; set; }

        public DeliveryModel? DeliveryModel { get; set; }

        public GetApprenticeshipsCSVRequest(
        long? providerId,
        string searchTerm,
        string employerName,
        string courseName,
        ApprenticeshipStatus? status,
        DateTime? startDate,
        DateTime? endDate,
        Alerts? alert,
        ConfirmationStatus? apprenticeConfirmationStatus,
        DeliveryModel? deliveryModel
            )
        {
            ProviderId = providerId;
            SearchTerm = searchTerm;
            EmployerName = employerName;
            CourseName = courseName;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
            Alert = alert;
            ApprenticeConfirmationStatus = apprenticeConfirmationStatus;
            DeliveryModel = deliveryModel;
        }

        public string GetUrl => $"api/apprenticeships/?providerId={ProviderId}&reverseSort=false{CreateFilterQuery(this)}";

        private static string CreateFilterQuery(GetApprenticeshipsCSVRequest request)
        {
            var queryParameters = new List<string>();

            if (!string.IsNullOrEmpty(request.SearchTerm))
                queryParameters.Add($"searchTerm={WebUtility.UrlEncode(request.SearchTerm)}");

            if (!string.IsNullOrEmpty(request.EmployerName))
                queryParameters.Add($"employerName={WebUtility.UrlEncode(request.EmployerName)}");

            if (!string.IsNullOrEmpty(request.CourseName))
                queryParameters.Add($"courseName={WebUtility.UrlEncode(request.CourseName)}");

            if (request.Status.HasValue)
                queryParameters.Add($"status={WebUtility.UrlEncode(request.Status.Value.ToString())}");

            if (request.StartDate.HasValue)
                queryParameters.Add($"startDate={WebUtility.UrlEncode(request.StartDate.Value.ToString("u"))}");

            if (request.EndDate.HasValue)
                queryParameters.Add($"endDate={WebUtility.UrlEncode(request.EndDate.Value.ToString("u"))}");

            if (request.Alert.HasValue)
                queryParameters.Add($"alert={WebUtility.UrlEncode(request.Alert.Value.ToString())}");

            if (request.ApprenticeConfirmationStatus.HasValue)
                queryParameters.Add($"apprenticeConfirmationStatus={WebUtility.UrlEncode(request.ApprenticeConfirmationStatus.ToString())}");

            if (request.DeliveryModel.HasValue)
                queryParameters.Add($"deliveryModel={WebUtility.UrlEncode(request.DeliveryModel.ToString())}");

            return queryParameters.Any() ? "&" + string.Join("&", queryParameters) : string.Empty;
        }
    }
}
