using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.SubmitEmployerFeedback;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.SubmitEmployerFeedback
{
    [TestFixture]
    public class SubmitEmployerFeedbackCommandTests
    {
        [Test]
        public void ImplicitOperator_MapsPropertiesCorrectly()
        {
            var request = new SubmitEmployerFeedbackRequest
            {
                UserRef = Guid.NewGuid(),
                Ukprn = 12345,
                AccountId = 67890,
                ProviderRating = OverallRating.Good,
                FeedbackSource = 2,
                ProviderAttributes = new List<ProviderAttributeDto>
                {
                    new ProviderAttributeDto { AttributeId = 1, AttributeValue = 5 }
                }
            };

            SubmitEmployerFeedbackCommand command = request;

            Assert.That(command.UserRef, Is.EqualTo(request.UserRef));
            Assert.That(command.Ukprn, Is.EqualTo(request.Ukprn));
            Assert.That(command.AccountId, Is.EqualTo(request.AccountId));
            Assert.That(command.ProviderRating, Is.EqualTo(request.ProviderRating));
            Assert.That(command.FeedbackSource, Is.EqualTo(request.FeedbackSource));
            Assert.That(command.ProviderAttributes, Is.EqualTo(request.ProviderAttributes));
        }
    }
}
