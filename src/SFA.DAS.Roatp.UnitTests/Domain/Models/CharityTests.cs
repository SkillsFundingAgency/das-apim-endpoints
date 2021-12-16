using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.Roatp.Domain.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Roatp.UnitTests.Domain.Models
{
    [TestFixture]
    public class CharityTests
    {
        [Test]
        public void Charity_Operator_ConvertsGetCharityResponse()
        {
            var trustee = new CharityTrustee
            {
                TrusteeId = 123,
                Name = Guid.NewGuid().ToString()
            };
            var response = new GetCharityResponse()
            { 
                Id = 1234,
                RegistrationDate = DateTime.Now,
                RegistrationNumber = 1234,
                Name = Guid.NewGuid().ToString(),
                RegistrationStatus = "Registered",
                CharityType = "Other",
                RemovalDate = null,
                Trustees = new List<CharityTrustee> { trustee }
            };

            Charity charity = response;

            Assert.AreEqual(1, charity.Trustees.Count);
            Assert.AreEqual(response.Id.ToString(), charity.CharityNumber);
            Assert.AreEqual(response.RegistrationNumber.ToString(), charity.RegistrationNumber);
            Assert.AreEqual(response.RegistrationDate, charity.RegistrationDate);
            Assert.AreEqual(response.Name, charity.Name);
            Assert.AreEqual(response.RegistrationStatus, charity.Status);
            Assert.AreEqual(response.CharityType, charity.Type);
            Assert.IsNull(charity.RemovalDate);
        }
    }
}
