using AutoFixture;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheValidateUlnOverlapOnStartDateQueryRequest
    {
        [Test, MoqAutoData]
        public void Then_The_Url_Is_Correctly_Built(long providerId,
            string uln,
            string startDate,
            string endDate)
        {
            //Arrange Act
            var actual = new ValidateUlnOverlapOnStartDateQueryRequest(providerId, uln, startDate, endDate);

            //Assert
            Assert.That(actual.GetUrl, Is.EqualTo($"api/overlapping-training-date-request/{providerId}/validateUlnOverlap?uln={uln}&startDate={startDate}&endDate={endDate}"));
        }
    }
}