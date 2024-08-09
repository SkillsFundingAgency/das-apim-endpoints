using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;

namespace SFA.DAS.ApprenticePortal.UnitTests.InnerApi.ApprenticeAccounts.Requests;

public class PutApprenticeApiRequestTests
{
    [Test, AutoData]
    public void Then_The_Url_And_Data_Is_Set(PutApprenticeApiRequestData requestData)
    {
        var actual = new PutApprenticeApiRequest(requestData);

        actual.PutUrl.Should().Be("/apprentices");
        ((PutApprenticeApiRequestData)actual.Data).Should().BeEquivalentTo(requestData);
    }
}