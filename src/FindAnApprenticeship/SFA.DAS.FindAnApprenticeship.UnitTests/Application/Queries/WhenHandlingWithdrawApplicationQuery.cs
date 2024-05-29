using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;

public class WhenHandlingWithdrawApplicationQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Withdraw_Application_Details_Returned(
        WithdrawApplicationQuery query,
        GetApplicationApiResponse getApplicationApiResponse,
        GetApprenticeshipVacancyItemResponse getApprenticeshipVacancyItemResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        WithdrawApplicationQueryHandler handler)
    {
        getApplicationApiResponse.Status = "Submitted";
        candidateApiClient.Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(x => 
                    x.GetUrl.Contains(query.ApplicationId.ToString())
                    && x.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(getApplicationApiResponse);
        findApprenticeshipApiClient
            .Setup(x => x.Get<GetApprenticeshipVacancyItemResponse>(
                It.Is<GetVacancyRequest>(c => c.GetUrl.Contains(getApplicationApiResponse.VacancyReference))))
            .ReturnsAsync(getApprenticeshipVacancyItemResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.ApplicationId.Should().Be(getApplicationApiResponse.Id);
        actual.ClosingDate.Should().Be(getApprenticeshipVacancyItemResponse.ClosingDate);
        actual.EmployerName.Should().Be(getApprenticeshipVacancyItemResponse.EmployerName);
        actual.SubmittedDate.Should().Be(getApplicationApiResponse.SubmittedDate);
        actual.AdvertTitle.Should().Be(getApprenticeshipVacancyItemResponse.Title);
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_CandidateId_Does_Not_Match_On_Application_Then_Null_Returned(
        WithdrawApplicationQuery query,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        WithdrawApplicationQueryHandler handler)
    {
        candidateApiClient.Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(x => 
                    x.GetUrl.Contains(query.ApplicationId.ToString())
                    && x.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync((GetApplicationApiResponse)null);
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().BeEquivalentTo(new WithdrawApplicationQueryResult());
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Application_Does_Not_Have_A_Status_Of_Submitted_Then_Null_Returned(
        WithdrawApplicationQuery query,
        GetApplicationApiResponse getApplicationApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        WithdrawApplicationQueryHandler handler)
    {
        getApplicationApiResponse.Status = "Something";
        candidateApiClient.Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(x => 
                    x.GetUrl.Contains(query.ApplicationId.ToString())
                    && x.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(getApplicationApiResponse);
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().BeEquivalentTo(new WithdrawApplicationQueryResult());
    }
}