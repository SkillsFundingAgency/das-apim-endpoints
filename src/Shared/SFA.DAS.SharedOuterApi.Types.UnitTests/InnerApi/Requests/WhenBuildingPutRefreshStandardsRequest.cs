using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPutRefreshStandardsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Built_And_Data_Passed(RefreshStandardsData data)
        {
            var actual = new RefreshStandardsRequest(data);

            actual.PutUrl.Should().Be("api/standards/refresh");
            var standardsProperty = actual.Data.GetType().GetProperty("Standards");
            standardsProperty.Should().NotBeNull();

            var standardsValue = standardsProperty.GetValue(actual.Data) as IEnumerable<object>;

            standardsValue.Should().NotBeNull();
            standardsValue.Should().AllBeOfType<StandardData>();
            standardsValue.Should().HaveCount(data.Standards.Count);
        }
    }
}