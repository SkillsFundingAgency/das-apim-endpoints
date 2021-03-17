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
        public void Then_The_GetUrl_Is_Correctly_Built_prior_to_20210406(string baseUrl, string companyName, string hashedLegalEntityId)
        {
            _mockDatetimeService.Setup(m => m.Today).Returns(new DateTime(2021, 04, 05));

            var actual = new GetVendorByApprenticeshipLegalEntityId(companyName, hashedLegalEntityId, _mockDatetimeService.Object)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}Finance/{companyName}/vendor/aleid={hashedLegalEntityId}?api-version=2019-06-01");
        }

        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built_prior_on_20210406(string baseUrl, string companyName, string hashedLegalEntityId)
        {
            _mockDatetimeService.Setup(m => m.Today).Returns(new DateTime(2021, 04, 06));

            var actual = new GetVendorByApprenticeshipLegalEntityId(companyName, hashedLegalEntityId, _mockDatetimeService.Object)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}Finance/{companyName}/vendor/aleid={hashedLegalEntityId}?api-version=2021-04-06");
        }

        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built_prior_after_20210406(string baseUrl, string companyName, string hashedLegalEntityId)
        {
            _mockDatetimeService.Setup(m => m.Today).Returns(new DateTime(2021, 04, 07));

            var actual = new GetVendorByApprenticeshipLegalEntityId(companyName, hashedLegalEntityId, _mockDatetimeService.Object)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}Finance/{companyName}/vendor/aleid={hashedLegalEntityId}?api-version=2021-04-06");
        }
    }
}