using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.ProviderUsers.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.ProviderUsers.Commands
{
    public class WhenSendingProviderEmails
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_A_Valid_Request(
            ProviderEmailCommand request,
            [Frozen] Mock<IProviderAccountApiClient<ProviderAccountApiConfiguration>> apiClient,
            ProviderEmailCommandHandler handler
          )
        {
            var response = new ApiResponse<object>(null, System.Net.HttpStatusCode.OK, string.Empty);
            apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostSendProviderEmailsRequest>(), false)).ReturnsAsync(response);
            var actual = await handler.Handle(request, CancellationToken.None);
            Assert.That(actual, Is.Not.Null);
            apiClient.Verify(x=>x.PostWithResponseCode<object>( It.Is<PostSendProviderEmailsRequest>(r=>r.ProviderId == request.ProviderId && ((ProviderEmailRequest)r.Data) == request.ProviderEmailRequest), false)); 
        }
    }
}
