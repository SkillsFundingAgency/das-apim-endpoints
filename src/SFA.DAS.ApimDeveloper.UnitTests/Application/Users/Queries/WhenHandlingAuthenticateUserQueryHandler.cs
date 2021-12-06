using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.Users.Queries;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.Users.Queries
{
    public class WhenHandlingAuthenticateUserQueryHandler
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_User_Returned(
            GetAuthenticateUserResult apiResponse,
            AuthenticateUserQuery query,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apimDeveloperApiClient,
            AuthenticateUserQueryHandler handler)
        {
            apimDeveloperApiClient.Setup(x =>
                    x.Get<GetAuthenticateUserResult>(It.Is<GetAuthenticateUserRequest>(c =>
                        c.GetUrl.Contains($"?email={HttpUtility.UrlEncode(query.Email)}&password={HttpUtility.UrlEncode(query.Password)}"))))
                .ReturnsAsync(apiResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.User.Should().BeEquivalentTo(apiResponse);
        }
    }
}