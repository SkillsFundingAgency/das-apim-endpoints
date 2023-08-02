using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerProfiles.Application.AccountUsers.Commands;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerProfiles.UnitTests.Application.AccountUsers
{
    public class WhenHandlingUpsertAccountCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Data_Returned(
            UpsertAccountCommand request,
            EmployerProfile response,
            [Frozen] Mock<IEmployerAccountsService> accountsApiClient,
            UpsertAccountCommandHandler handler)
        {
            accountsApiClient.Setup(x =>
                    x.PutEmployerAccount(It.Is<EmployerProfile>(c =>
                        c.Email.Equals(request.Email) 
                        && c.UserId.Equals(request.UserId)
                        && c.FirstName.Equals(request.FirstName)
                        && c.LastName.Equals(request.LastName)
                        && c.GovIdentifier.Equals(request.GovIdentifier)
                        && c.CorrelationId.Equals(request.CorrelationId)
                        )))
                .ReturnsAsync(response);

            var actual = await handler.Handle(request, CancellationToken.None);

            actual.Should().BeEquivalentTo(response,
                options => options
                    .Excluding(c => c.FirstName)
                    .Excluding(c => c.LastName)
                    .Excluding(c => c.UserId)
                    .Excluding(c => c.Email)
            );

            actual.FirstName.Should().Be(response.FirstName);
            actual.LastName.Should().Be(response.LastName);
            actual.UserId.Should().Be(response.UserId);
            actual.Email.Should().Be(response.Email);
        }
    }
}