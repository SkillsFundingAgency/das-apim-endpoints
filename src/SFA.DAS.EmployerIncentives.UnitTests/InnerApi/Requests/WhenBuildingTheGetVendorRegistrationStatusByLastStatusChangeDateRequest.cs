using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Extensions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using System;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetVendorRegistrationStatusByLastStatusChangeDateRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(DateTime fromDate, DateTime toDate, string baseUrl)
        {
            var actual = new GetVendorRegistrationStatusByLastStatusChangeDateRequest(fromDate, toDate)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}Finance/Registrations?DateTimeFrom={fromDate.ToIsoDateTime()}&DateTimeTo={toDate.ToIsoDateTime()}&api-version=2019-06-01");
        }
    }
}
