using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.Users.Queries.GetUser;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.Users.Queries
{
    public class WhenHandlingGetUserQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_User_Returned(
            GetUserResponse apiResponse,
            GetUserQuery query,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apimDeveloperApiClient,
            GetUserQueryHandler handler)
        {
            apimDeveloperApiClient.Setup(x =>
                    x.GetWithResponseCode<GetUserResponse>(It.Is<GetUserRequest>(c => 
                        c.GetUrl.Equals(new GetUserRequest(query.Email).GetUrl))))
                .ReturnsAsync(new ApiResponse<GetUserResponse>(apiResponse, HttpStatusCode.OK, ""));
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.User.Should().BeEquivalentTo(apiResponse);
        }
        
        [Test, MoqAutoData]
        public async Task And_404_Then_Null_Returned(
            GetUserResponse apiResponse,
            GetUserQuery query,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apimDeveloperApiClient,
            GetUserQueryHandler handler)
        {
            apimDeveloperApiClient.Setup(x =>
                    x.GetWithResponseCode<GetUserResponse>(It.IsAny<GetUserRequest>()))
                .ReturnsAsync(new ApiResponse<GetUserResponse>(apiResponse, HttpStatusCode.NotFound, "An Error"));
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.User.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void And_Other_Error_Then_HttpRequestContentException_Thrown(
            GetUserResponse apiResponse,
            GetUserQuery query,
            [Frozen] Mock<IApimDeveloperApiClient<ApimDeveloperApiConfiguration>> apimDeveloperApiClient,
            GetUserQueryHandler handler)
        {
            apimDeveloperApiClient.Setup(x =>
                    x.GetWithResponseCode<GetUserResponse>(It.IsAny<GetUserRequest>()))
                .ReturnsAsync(new ApiResponse<GetUserResponse>(apiResponse, HttpStatusCode.BadRequest, "An Error"));
            
            Assert.ThrowsAsync<HttpRequestContentException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}