//using System;
//using System.Net;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoFixture;
//using FluentAssertions;
//using Moq;
//using Moq.Protected;
//using NUnit.Framework;
//using SFA.DAS.EmployerIncentives.Infrastructure.Api;
//using SFA.DAS.EmployerIncentives.Models.PassThrough;
//using SFA.DAS.EmployerIncentives.Services;

//namespace SFA.DAS.EmployerIncentives.UnitTests.Services
//{
//    [TestFixture]
//    [Parallelizable(ParallelScope.All)]
//    public class EmployerIncentivesPassThroughServiceTests
//    {
//        [Test]
//        public async Task When_Adding_LegalEntity_Then_should_call_PostAsync_with_correct_url()
//        {
//            var f = new TestsFixture().WithJsonResponseFromInnerApi();
//            await f.Sut.AddLegalEntity(f.AccountId, f.LegalEntityRequest);

//            f.VerifyMethodAndPath(HttpMethod.Post, $"accounts/{f.AccountId}/legalentities");
//        }

//        [Test]
//        public async Task When_Adding_LegalEntity_Then_should_return_ApiResponse()
//        {
//            var f = new TestsFixture().WithJsonResponseFromInnerApi();
//            var result = await f.Sut.AddLegalEntity(f.AccountId, f.LegalEntityRequest);

//            f.VerifyApiResponseIsReturned(result);
//        }

//        [Test]
//        public async Task When_Removing_LegalEntity_Then_should_call_DeleteAsync_with_correct_url()
//        {
//            var f = new TestsFixture().WithNoJsonResponseFromInnerApi();
//            await f.Sut.RemoveLegalEntity(f.AccountId, f.AccountLegalEntityId);

//            f.VerifyMethodAndPath(HttpMethod.Delete, $"accounts/{f.AccountId}/legalentities/{f.AccountLegalEntityId}");
//        }

//        [Test]
//        public async Task When_Removing_LegalEntity_Then_should_return_ApiResponse()
//        {
//            var f = new TestsFixture().WithNoJsonResponseFromInnerApi();
//            var result = await f.Sut.RemoveLegalEntity(f.AccountId, f.AccountLegalEntityId);

//            result.Should().NotBeNull();
//            result.StatusCode.Should().Be(f.HttpResponseMessage.StatusCode);
//            result.Json.Should().BeNull();
//        }

//        private class TestsFixture
//        {
//            public Fixture Fixture;
//            public HttpClient HttpClient;
//            public Mock<HttpClientHandler> HttpClientHandlerMock;
//            public HttpResponseMessage HttpResponseMessage;
//            public EmployerIncentivesPassThroughService Sut;
//            public long AccountId;
//            public long AccountLegalEntityId;
//            public LegalEntityRequest LegalEntityRequest;
//            public string BaseUrl = "http://www.test.com/";

//            public TestsFixture()
//            {
//                Fixture = new Fixture();

//                HttpClientHandlerMock = new Mock<HttpClientHandler>();

//                HttpClient = new HttpClient(HttpClientHandlerMock.Object);
//                HttpClient.BaseAddress = new Uri(BaseUrl);

//                Sut = new EmployerIncentivesPassThroughService(HttpClient);

//                AccountId = Fixture.Create<long>();
//                AccountLegalEntityId = Fixture.Create<long>();
//                LegalEntityRequest = Fixture.Create<LegalEntityRequest>();
//            }

//            internal TestsFixture WithJsonResponseFromInnerApi()
//            {
//                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"Test\" : \"XXXX\"}") };
//                HttpClientHandlerMock.Protected()
//                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => true),
//                        ItExpr.IsAny<CancellationToken>()).ReturnsAsync(HttpResponseMessage);
//                return this;
//            }

//            internal TestsFixture WithNoJsonResponseFromInnerApi()
//            {
//                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
//                HttpClientHandlerMock.Protected()
//                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => true),
//                        ItExpr.IsAny<CancellationToken>()).ReturnsAsync(HttpResponseMessage);
//                return this;
//            }

//            internal void VerifyMethodAndPath(HttpMethod method, string path)
//            {
//                var expectedUri = new Uri(new Uri(BaseUrl), path);
//                HttpClientHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(x => x.Method == method && x.RequestUri == expectedUri), ItExpr.IsAny<CancellationToken>());
//            }

//            internal void VerifyApiResponseIsReturned(InnerApiResponse result)
//            {
//                result.Should().NotBeNull();
//                result.StatusCode.Should().Be(HttpResponseMessage.StatusCode);
//                result.Json.Should().NotBeNull();
//                result.Json.RootElement.GetProperty("Test").GetString().Should().Be("XXXX");
//            }
//        }
//    }
//}
