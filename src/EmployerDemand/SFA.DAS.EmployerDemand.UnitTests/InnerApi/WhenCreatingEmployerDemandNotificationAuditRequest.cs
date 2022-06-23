using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingEmployerDemandNotificationAuditRequest
    {
        [Test, AutoData]
        public void Then_Creates_Url_Correctly(Guid id, Guid courseDemandId, NotificationType notificationType)
        {
            //Arrange
            notificationType = NotificationType.StoppedCourseClosed;
            
            //Act
            var actual = new PostEmployerDemandNotificationAuditRequest(id, courseDemandId, notificationType);
            
            //Assert
            actual.PostUrl.Should().Be($"api/Demand/{courseDemandId}/notification-audit/{id}?notificationType={(short)notificationType}");
        }
    }
}