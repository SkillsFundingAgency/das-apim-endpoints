using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses.Domain.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Roatp.UnitTests.Domain.Models
{
    [TestFixture]
    public class ProviderTests
    {
        [Test]
        public void Provider_Operator_ConvertsGetProviderResponse()
        {
            var response = new GetProviderResponse()
            { 
                Id = 1234,
                Ukprn = 12345,
                LegalName = "Test",
                TradingName = "Test",
                Email = "t@t.com",
                Phone = "13123143",
                Website = "www.google.com",
                MarketingInfo = "Test",
                EmployerSatisfaction = (decimal?)1.1,
                LearnerSatisfaction = (decimal?)1.2,
                IsImported = true,
                HasConfirmedLocations = true,
                HasConfirmedDetails = true,
            };

            Provider provider = response;

            Assert.AreEqual(response.Id, provider.Id);
            Assert.AreEqual(response.Ukprn, provider.Ukprn);
            Assert.AreEqual(response.LegalName, provider.LegalName);
            Assert.AreEqual(response.TradingName, provider.TradingName);
            Assert.AreEqual(response.Email, provider.Email);
            Assert.AreEqual(response.Phone, provider.Phone);
            Assert.AreEqual(response.Website, provider.Website);
            Assert.AreEqual(response.MarketingInfo, provider.MarketingInfo);
            Assert.AreEqual(response.EmployerSatisfaction, provider.EmployerSatisfaction);
            Assert.AreEqual(response.LearnerSatisfaction, provider.LearnerSatisfaction);
            Assert.AreEqual(response.IsImported, provider.IsImported);
            Assert.AreEqual(response.HasConfirmedLocations, provider.HasConfirmedLocations);
            Assert.AreEqual(response.HasConfirmedDetails, provider.HasConfirmedDetails);

        }
    }
}
