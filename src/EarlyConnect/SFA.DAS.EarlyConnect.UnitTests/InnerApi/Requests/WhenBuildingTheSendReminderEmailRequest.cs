using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheSendReminderEmailRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(ReminderEmail reminderEmail)
        {
            var actual = new SendReminderEmailRequest(reminderEmail);

            actual.PostUrl.Should().Be("/api/student-triage-data/reminder");
        }
    }
}