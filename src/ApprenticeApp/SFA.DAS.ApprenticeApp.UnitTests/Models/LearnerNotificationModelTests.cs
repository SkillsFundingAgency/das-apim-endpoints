using NUnit.Framework;
using NUnit.Framework.Legacy;
using SFA.DAS.ApprenticeApp.Models;
using System;

namespace SFA.DAS.ApprenticeApp.UnitTests.Models
{
    public class LearnerNotificationModelTests
    {
        [Test]
        public void LearnerNotification_model_test()
        {
            var sut = new LearnerNotification
            {
                NotificationId = 1,
                CorrelationId = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),
                LearnerAccountId = new Guid("A1B2C3D4-E5F6-7890-ABCD-EF1234567890"),
                Category = "General",
                Heading = "Test Heading",
                Body = "Test Body",
                StatusId = 1,
                NotificationTime = new DateTime(2025, 06, 01),
                TimeToExpire = new DateTime(2025, 07, 01),
                TimeReceived = new DateTime(2025, 06, 01),
                Link = "https://Test.com"
            };

            ClassicAssert.AreEqual(1, sut.NotificationId);
            ClassicAssert.AreEqual(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), sut.CorrelationId);
            ClassicAssert.AreEqual(new Guid("A1B2C3D4-E5F6-7890-ABCD-EF1234567890"), sut.LearnerAccountId);
            ClassicAssert.AreEqual("General", sut.Category);
            ClassicAssert.AreEqual("Test Heading", sut.Heading);
            ClassicAssert.AreEqual("Test Body", sut.Body);
            ClassicAssert.AreEqual((byte)1, sut.StatusId);
            ClassicAssert.AreEqual(new DateTime(2025, 06, 01), sut.NotificationTime);
            ClassicAssert.AreEqual(new DateTime(2025, 07, 01), sut.TimeToExpire);
            ClassicAssert.AreEqual(new DateTime(2025, 06, 01), sut.TimeReceived);
            ClassicAssert.AreEqual("https://Test.com", sut.Link);
        }

        [Test]
        public void LearnerNotificationStatus_model_test()
        {
            var sut = new LearnerNotificationStatus
            {
                StatusId = 0,
                StatusName = "Pending",
                LastUpdated = new DateTime(2025, 12, 13, 18, 02, 35)
            };

            ClassicAssert.AreEqual((byte)0, sut.StatusId);
            ClassicAssert.AreEqual("Pending", sut.StatusName);
            ClassicAssert.AreEqual(new DateTime(2025, 12, 13, 18, 02, 35), sut.LastUpdated);
        }

        [Test]
        public void UpdateNotificationStatusData_model_test()
        {
            var sut = new UpdateNotificationStatusData
            {
                Status = "Read"
            };

            ClassicAssert.AreEqual("Read", sut.Status);
        }

        [Test]
        public void UpdateNotificationStatusRequest_model_test()
        {
            var sut = new UpdateNotificationStatusRequest
            {
                Status = "Read"
            };

            ClassicAssert.AreEqual("Read", sut.Status);
        }
    }
}