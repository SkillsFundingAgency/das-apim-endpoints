using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingPostLevyDeclarationSummaryByDateRequest
{
    [Test, MoqAutoData]
    public void Then_Builds_Request_With_The_Date_Range_And_Paye_Schemes(long accountId, DateTime fromDate, DateTime toDate, List<string> emprefs)
    {
        // Act
        var request = new PostLevyDeclarationSummaryByDate(accountId, fromDate, toDate, emprefs);

        // Assert
        request.AccountId.Should().Be(accountId);
        request.FromDate.Should().Be(fromDate);
        request.ToDate.Should().Be(toDate);
        request.PostUrl.Should().Be($"api/levy-declarations/{accountId}/summaryByDate?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}");
        request.Emprefs.Should().BeSameAs(emprefs);
        request.Data.Should().BeSameAs(emprefs);
    }
}
