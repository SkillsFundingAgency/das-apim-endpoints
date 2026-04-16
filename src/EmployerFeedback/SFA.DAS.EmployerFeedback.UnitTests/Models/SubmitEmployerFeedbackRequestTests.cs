using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.UnitTests.Models
{
    [TestFixture]
    public class SubmitEmployerFeedbackRequestTests
    {
        [Test]
        public void Can_Construct_And_Assign_Properties()
        {
            var userRef = Guid.NewGuid();
            var providerAttributes = new List<ProviderAttributeDto> { new ProviderAttributeDto { AttributeId = 1, AttributeValue = 2 } };
            var request = new SubmitEmployerFeedbackRequest
            {
                UserRef = userRef,
                Ukprn = 123,
                AccountId = 456,
                ProviderRating = OverallRating.Poor,
                FeedbackSource = 3,
                ProviderAttributes = providerAttributes
            };
            Assert.That(request.UserRef, Is.EqualTo(userRef));
            Assert.That(request.Ukprn, Is.EqualTo(123));
            Assert.That(request.AccountId, Is.EqualTo(456));
            Assert.That(request.ProviderRating, Is.EqualTo(OverallRating.Poor));
            Assert.That(request.FeedbackSource, Is.EqualTo(3));
            Assert.That(request.ProviderAttributes, Is.EqualTo(providerAttributes));
        }

        [Test]
        public void ProviderAttributeDto_Can_Construct_And_Assign_Properties()
        {
            var dto = new ProviderAttributeDto { AttributeId = 99, AttributeValue = 7 };
            Assert.That(dto.AttributeId, Is.EqualTo(99));
            Assert.That(dto.AttributeValue, Is.EqualTo(7));
        }
    }
}
