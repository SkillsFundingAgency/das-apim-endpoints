using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.InnerApi.Requests;

public class WhenBuildingGetVacanciesByAccountIdApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Build(long accountId)
    {
        var actual = new GetVacanciesByAccountIdApiRequest(accountId);
        
        actual.GetUrl.Should().Be($"api/accounts/{accountId}/vacancies?page=1&pageSize=1&sortColumn=CreatedDate&sortOrder=Desc&filterBy=All&searchTerm=");
    }
}