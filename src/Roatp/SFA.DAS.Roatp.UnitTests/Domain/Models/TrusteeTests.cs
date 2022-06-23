using System;
using NUnit.Framework;
using SFA.DAS.Roatp.Domain.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Roatp.UnitTests.Domain.Models
{
    [TestFixture]
    public class TrusteeTests
    {
        [Test]
        public void Trustee_Operator_ConvertsCharityTrustee()
        {
            var charityTrustee = new CharityTrustee
            {
                TrusteeId = 123,
                Name = Guid.NewGuid().ToString()
            };

            Trustee trustee = charityTrustee;

            Assert.AreEqual(charityTrustee.TrusteeId.ToString(), trustee.Id);
            Assert.AreEqual(charityTrustee.Name, trustee.Name);
        }
    }
}
