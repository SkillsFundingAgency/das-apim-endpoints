using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetAllLearnersRequest
    {
        [Test]
        public void With_No_Date_Then_The_Url_Is_Correctly_Constructed()
        {
            //Act
            var actual = new GetAllLearnersRequest(null, 1, 10);

            //Assert
            actual.GetUrl.Should().Be("api/learners?sinceTime=&batch_number=1&batch_size=10");
        }

        [Test]
        public void With_Date_Then_The_Url_Is_Correctly_Constructed()
        {
            //Act
            var actual = new GetAllLearnersRequest(new System.DateTime(2021,03, 01), 1, 10);

            //Assert
            actual.GetUrl.Should().Be("api/learners?sinceTime=2021-03-01&batch_number=1&batch_size=10");
        }
    }
}
