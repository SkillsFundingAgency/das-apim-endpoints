using System;
using System.Net;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetApprenticeshipsCSVRequest
    {
        [Test, MoqAutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
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
            var actual = new GetApprenticeshipsCSVRequest(providerId, 
                searchTerm, 
                employerName, 
                courseName, 
                status, 
                startDate, 
                endDate, 
                alert,
                apprenticeConfirmationStatus, 
                deliveryModel);

            var expectedUrl =
                $"api/apprenticeships/?providerId={providerId}" +
                $"&reverseSort=false" +
                $"&searchTerm={WebUtility.UrlEncode(searchTerm)}" +
                $"&employerName={WebUtility.UrlEncode(employerName)}" +
                $"&courseName={WebUtility.UrlEncode(courseName)}" +
                $"&status={WebUtility.UrlEncode(status?.ToString())}" +
                $"&startDate={WebUtility.UrlEncode(startDate?.ToString("u"))}" +
                $"&endDate={WebUtility.UrlEncode(endDate?.ToString("u"))}" +
                $"&alert={WebUtility.UrlEncode(alert?.ToString())}" +
                $"&apprenticeConfirmationStatus={WebUtility.UrlEncode(apprenticeConfirmationStatus?.ToString())}" +
                $"&deliveryModel={WebUtility.UrlEncode(deliveryModel?.ToString())}";

            //Assert
            actual.GetUrl.Should().Be(expectedUrl);
        }
    }
}
