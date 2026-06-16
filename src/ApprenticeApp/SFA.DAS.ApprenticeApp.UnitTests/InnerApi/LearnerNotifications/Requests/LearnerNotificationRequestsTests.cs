using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests;
using SFA.DAS.ApprenticeApp.Models;
using System;

namespace SFA.DAS.ApprenticeApp.UnitTests.InnerApi.LearnerNotifications.Requests
{
    public class LearnerNotificationRequestsTests
    {
        [Test, AutoData]
        public void GetLearnerNotificationsRequestUrlIsCorrectlyBuilt()
        {
            var accountIdentifier = Guid.NewGuid();

            var instance = new GetLearnerNotificationsRequest(accountIdentifier);

            instance.GetUrl.Should().Be($"learner/{accountIdentifier}");
        }

        [Test, AutoData]
        public void GetLearnerNotificationByIdRequestUrlIsCorrectlyBuilt()
        {
            var accountIdentifier = Guid.NewGuid();
            long notificationIdentifier = 12345L;

            var instance = new GetLearnerNotificationByIdRequest(accountIdentifier, notificationIdentifier);

            instance.GetUrl.Should().Be($"learner/{accountIdentifier}/notifications/{notificationIdentifier}");
        }

        [Test, AutoData]
        public void GetLearnerNotificationStatusRequestUrlIsCorrectlyBuilt()
        {
            var accountIdentifier = Guid.NewGuid();
            long notificationIdentifier = 12345L;

            var instance = new GetLearnerNotificationStatusRequest(accountIdentifier, notificationIdentifier);

            instance.GetUrl.Should().Be($"learner/{accountIdentifier}/notifications/{notificationIdentifier}/status");
        }

        [Test, AutoData]
        public void UpdateLearnerNotificationStatusRequestUrlIsCorrectlyBuilt()
        {
            var accountIdentifier = Guid.NewGuid();
            long notificationIdentifier = 12345L;
            var data = new UpdateNotificationStatusData { Status = "Read" };

            var instance = new UpdateLearnerNotificationStatusRequest(accountIdentifier, notificationIdentifier, data);

            instance.PutUrl.Should().Be($"learner/{accountIdentifier}/notifications/{notificationIdentifier}/status");
            instance.Data.Should().BeSameAs(data);
        }
    }
}