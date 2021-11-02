using System;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetApprenticeRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            //Arrange Act
            var id = Guid.NewGuid();
            var actual = new GetApprenticeRequest(id);
            
            //Assert
            Assert.AreEqual($"apprentices/{id}", actual.GetUrl);
        }
    }
}