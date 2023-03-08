using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerProfiles.Application.AccountUsers.Commands.UpsertEmployer;

namespace SFA.DAS.EmployerProfiles.UnitTests.Application.AccountUsers
{
    public class WhenHandlingUpsertAccountCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Data_Returned(
            EmployerProfile profile,
            [Frozen] Mock<IEmployerAccountsService> accountsApiClient,
            UpsertAccountCommandHandler handler)
        {
            profile.UserId = Guid.NewGuid().ToString();

            accountsApiClient.Setup(x =>
                    x.PutEmployerAccount(It.Is<EmployerProfile>(c =>
                        c.Email.Equals(profile.Email)
                        && c.FirstName.Equals(profile.FirstName)
                        && c.LastName.Equals(profile.LastName)
                        && c.UserId.Equals(profile.UserId)
                        )))
                .ReturnsAsync(profile);

            var actual = await handler.Handle(new UpsertAccountCommand
            {
                Email = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                GovIdentifier = profile.UserId,
                Id = new Guid(profile.UserId)
            }, CancellationToken.None);

            actual.FirstName.Should().Be(profile.FirstName);
            actual.LastName.Should().Be(profile.LastName);
            actual.Email.Should().Be(profile.Email);
            actual.UserId.Should().Be(profile.UserId);
        }
    }
}