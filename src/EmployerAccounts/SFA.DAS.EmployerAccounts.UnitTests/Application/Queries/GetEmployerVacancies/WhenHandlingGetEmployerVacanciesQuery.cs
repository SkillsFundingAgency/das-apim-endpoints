using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerVacancies;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetEmployerVacancies;

public class WhenHandlingGetEmployerVacanciesQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Data_Returned_From_Client_If_Single_Vacancy(
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> apiClient,
        GetPagedVacancySummaryApiResponse apiResponse,
        GetEmployerVacanciesQuery query,
        GetEmployerVacanciesQueryHandler handler)
    {
        apiResponse.PageInfo.TotalCount = 1;
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetPagedVacancySummaryApiResponse>(
                    It.Is<GetVacanciesByAccountIdApiRequest>(c => c.GetUrl.Contains(query.AccountId.ToString()))))
            .ReturnsAsync(new ApiResponse<GetPagedVacancySummaryApiResponse>(apiResponse, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Vacancies.Should().BeEquivalentTo(apiResponse.Items);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Empty_List_Returned_If_Not_Single_Vacancy(
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> apiClient,
        GetPagedVacancySummaryApiResponse apiResponse,
        GetEmployerVacanciesQuery query,
        GetEmployerVacanciesQueryHandler handler)
    {
        apiResponse.PageInfo.TotalCount = 2;
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetPagedVacancySummaryApiResponse>(
                    It.Is<GetVacanciesByAccountIdApiRequest>(c => c.GetUrl.Contains(query.AccountId.ToString()))))
            .ReturnsAsync(new ApiResponse<GetPagedVacancySummaryApiResponse>(apiResponse, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Vacancies.Should().BeEmpty();
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Empty_List_Returned_If_Error(
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> apiClient,
        GetPagedVacancySummaryApiResponse apiResponse,
        GetEmployerVacanciesQuery query,
        GetEmployerVacanciesQueryHandler handler)
    {
        apiResponse.PageInfo.TotalCount = 2;
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetPagedVacancySummaryApiResponse>(
                    It.Is<GetVacanciesByAccountIdApiRequest>(c => c.GetUrl.Contains(query.AccountId.ToString()))))
            .ReturnsAsync(new ApiResponse<GetPagedVacancySummaryApiResponse>(null, HttpStatusCode.InternalServerError, "Something happened"));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Vacancies.Should().BeEmpty();
    }
}