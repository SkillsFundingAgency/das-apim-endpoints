using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetVendorByApprenticeshipLegalEntityIdRequest
    {
        private readonly Mock<IDateTimeService> _mockDatetimeService;

        public WhenBuildingTheGetVendorByApprenticeshipLegalEntityIdRequest()
        {
            _mockDatetimeService = new Mock<IDateTimeService>();
        }

      
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly(string baseUrl, string companyName, string hashedLegalEntityId, string apiVersion)
        {
            var actual = new GetVendorByApprenticeshipLegalEntityId(companyName, hashedLegalEntityId, apiVersion)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}Finance/{companyName}/vendor/aleid={hashedLegalEntityId}?api-version={apiVersion}");
        }
    }
}