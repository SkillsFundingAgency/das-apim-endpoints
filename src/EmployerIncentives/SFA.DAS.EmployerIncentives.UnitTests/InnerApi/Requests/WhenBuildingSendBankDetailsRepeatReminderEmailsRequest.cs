using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingSendBankDetailsRepeatReminderEmailsRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(DateTime applicationCutOffDate)
        {
            var request = new SendBankDetailsRepeatReminderEmailsRequest(applicationCutOffDate);
            var actual = new PostBankDetailsRepeatReminderEmailsRequest { Data = request };

            request.ApplicationCutOffDate.Should().Be(applicationCutOffDate);
            actual.PostUrl.Should().Be("api/EmailCommand/bank-details-repeat-reminders");
        }
    }
}