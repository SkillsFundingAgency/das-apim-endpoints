using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.GetEmployerAccountTaskList;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetEmployerAccountTaskListRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(long accountId, string hashedAccountId)
    {
        var actual = new GetEmployerAccountTaskListRequest(accountId, hashedAccountId);

        actual.GetUrl.Should().Be($"accounts/{accountId}/account-task-list?hashedAccountId={hashedAccountId}");
    }
}