using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingUpdateIncentiveApplicationRequest
    {
        [Test, AutoData]
        public void Then_The_PutUrl_Is_Correctly_Build(UpdateIncentiveApplicationRequestData data, string baseUrl)
        {

            var actual = new UpdateIncentiveApplicationRequest
            {
                Data = data,
                BaseUrl = baseUrl
            };

            actual.PutUrl.Should().Be($"{baseUrl}applications/{data.IncentiveApplicationId}");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}