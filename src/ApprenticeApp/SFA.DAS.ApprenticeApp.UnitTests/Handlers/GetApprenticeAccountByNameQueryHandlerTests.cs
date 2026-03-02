using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetApprenticeAccountByNameQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Call_Api_And_Return_Mapped_Apprentices(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
            GetApprenticeAccountByNameQueryHandler sut)
        {
            // Arrange
            var query = new GetApprenticeAccountByNameQuery
            {
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            var expectedUrl =
                $"apprentices?firstName={query.FirstName}&lastName={query.LastName}&dateOfBirth={query.DateOfBirth.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";


            var apiResponse = new List<ApprenticeAccount>
            {
                new ApprenticeAccount
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Smith",
                    DateOfBirth = query.DateOfBirth,
                    TermsOfUseAccepted = true,
                    Email = new Email { Address = "john.smith@test.com" }
                }
            };

            apiClientMock
                .Setup(c => c.Get<List<ApprenticeAccount>>(
                    It.Is<GetApprenticeAccountByNameRequest>(r =>
                        r.GetUrl == expectedUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Apprentices.Should().HaveCount(1);

            var apprentice = result.Apprentices[0];
            apprentice.ApprenticeId.Should().Be(apiResponse[0].Id);
            apprentice.FirstName.Should().Be(apiResponse[0].FirstName);
            apprentice.LastName.Should().Be(apiResponse[0].LastName);
            apprentice.Email.Should().Be(apiResponse[0].Email.Address);
            apprentice.DateOfBirth.Should().Be(apiResponse[0].DateOfBirth);
            apprentice.TermsOfUseAccepted.Should().BeTrue();

            apiClientMock.Verify(c => c.Get<List<ApprenticeAccount>>(
                It.IsAny<GetApprenticeAccountByNameRequest>()),
                Times.Once);
        }
    }
}