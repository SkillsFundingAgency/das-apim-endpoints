using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.Application.Commands.SubmitEmployerFeedback;
using SFA.DAS.EmployerFeedback.Models;
using SubmitEmployerFeedbackRequest = SFA.DAS.EmployerFeedback.InnerApi.Requests.SubmitEmployerFeedbackRequest;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class SubmitEmployerFeedbackRequestTests
    {
        [Test]
        public void Constructor_SetsDataProperty()
        {
            var data = new SubmitEmployerFeedbackRequestData { UserRef = Guid.NewGuid() };
            var request = new SubmitEmployerFeedbackRequest(data);
            Assert.That(request.Data, Is.EqualTo(data));
            Assert.That(request.PostUrl, Is.EqualTo("api/employerfeedbackresult"));
        }

        [Test]
        public void ImplicitOperator_MapsCommandToRequestData()
        {
            var command = new SubmitEmployerFeedbackCommand
            {
                UserRef = Guid.NewGuid(),
                Ukprn = 12345,
                AccountId = 67890,
                ProviderRating = OverallRating.Excellent,
                FeedbackSource = 1,
                ProviderAttributes = new List<ProviderAttributeDto> { new ProviderAttributeDto { AttributeId = 2, AttributeValue = 10 } }
            };
            SubmitEmployerFeedbackRequestData data = command;
            Assert.That(data.UserRef, Is.EqualTo(command.UserRef));
            Assert.That(data.Ukprn, Is.EqualTo(command.Ukprn));
            Assert.That(data.AccountId, Is.EqualTo(command.AccountId));
            Assert.That(data.ProviderRating, Is.EqualTo(command.ProviderRating.ToString()));
            Assert.That(data.FeedbackSource, Is.EqualTo(command.FeedbackSource));
            Assert.That(data.ProviderAttributes[0].AttributeId, Is.EqualTo(command.ProviderAttributes[0].AttributeId));
            Assert.That(data.ProviderAttributes[0].AttributeValue, Is.EqualTo(command.ProviderAttributes[0].AttributeValue));
        }

        [Test]
        public void ProviderAttributeData_ImplicitOperator_MapsDtoCorrectly()
        {
            var dto = new ProviderAttributeDto { AttributeId = 7, AttributeValue = 3 };
            ProviderAttributeData data = dto;
            Assert.That(data.AttributeId, Is.EqualTo(dto.AttributeId));
            Assert.That(data.AttributeValue, Is.EqualTo(dto.AttributeValue));
        }
    }
}
