using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;
using SFA.DAS.EmployerAccounts.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetTasks
{
    [TestFixture]
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_NumberTransferPledgeApplicationsToReview_Should_Match_Api_Response(
        [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockLTMApi,
        [Frozen] Mock<ICurrentDateTime> mockCurrentDateTime,
        GetApplicationsResponse ltmApplicationsResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
        {
            mockCurrentDateTime.Setup(m => m.Now).Returns(new DateTime(2024, 01, 18));

            mockLTMApi
                .Setup(m => m.Get<GetApplicationsResponse>(It.Is<GetApplicationsRequest>(r =>
                    r.SenderAccountId == request.AccountId
                    && r.ApplicationStatusFilter == ApplicationStatus.Pending)))
                .ReturnsAsync(ltmApplicationsResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            result.NumberTransferPledgeApplicationsToReview.Should().Be(ltmApplicationsResponse.TotalItems);
        }

        [Test, MoqAutoData]
        public async Task Then_ShowLevyDeclarationTask_Is_True_If_In_DateRange_And_Levy(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockAccountApi,
        [Frozen] Mock<ICurrentDateTime> mockCurrentDateTime,
        GetAccountByIdResponse accountResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
        {
            mockCurrentDateTime.Setup(m => m.Now).Returns(new DateTime(2024, 01, 18));
            accountResponse.ApprenticeshipEmployerType = ApprenticeshipEmployerType.Levy;
            mockAccountApi
                .Setup(m => m.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(r => r.AccountId == request.AccountId)))
                .ReturnsAsync(accountResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            result.ShowLevyDeclarationTask.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_ShowLevyDeclarationTask_Is_False_If_In_DateRange_And_NonLevy(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockAccountApi,
        [Frozen] Mock<ICurrentDateTime> mockCurrentDateTime,
        GetAccountByIdResponse accountResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
        {
            mockCurrentDateTime.Setup(m => m.Now).Returns(new DateTime(2024, 01, 18));
            accountResponse.ApprenticeshipEmployerType = ApprenticeshipEmployerType.NonLevy;
            mockAccountApi
                .Setup(m => m.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(r => r.AccountId == request.AccountId)))
                .ReturnsAsync(accountResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            result.ShowLevyDeclarationTask.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_ShowLevyDeclarationTask_Is_False_If_Out_Of_DateRange_And_Levy(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockAccountApi,
        [Frozen] Mock<ICurrentDateTime> mockCurrentDateTime,
        GetAccountByIdResponse accountResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
        {
            mockCurrentDateTime.Setup(m => m.Now).Returns(new DateTime(2024, 01, 13));
            accountResponse.ApprenticeshipEmployerType = ApprenticeshipEmployerType.Levy;
            mockAccountApi
                .Setup(m => m.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(r => r.AccountId == request.AccountId)))
                .ReturnsAsync(accountResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            result.ShowLevyDeclarationTask.Should().BeFalse();
        }
    }
}