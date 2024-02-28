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

            Assert.That(1, Is.EqualTo(charity.Trustees.Count));
            Assert.That(response.Id.ToString(), Is.EqualTo(charity.CharityNumber));
            Assert.That(response.RegistrationNumber.ToString(), Is.EqualTo(charity.RegistrationNumber));
            Assert.That(response.RegistrationDate, Is.EqualTo(charity.RegistrationDate));
            Assert.That(response.Name, Is.EqualTo(charity.Name));
            Assert.That(response.RegistrationStatus, Is.EqualTo(charity.Status));
            Assert.That(response.CharityType, Is.EqualTo(charity.Type));
            Assert.That(charity.RemovalDate, Is.Null);
        }
    }
}
