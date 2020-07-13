//using System.Threading;
//using System.Threading.Tasks;
//using AutoFixture;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using NUnit.Framework;
//using SFA.DAS.EmployerIncentives.Api.Controllers;
//using SFA.DAS.EmployerIncentives.Infrastructure.Api;
//using SFA.DAS.EmployerIncentives.Interfaces;
//using SFA.DAS.EmployerIncentives.Models.PassThrough;

//namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers
//{
//    [TestFixture]
//    [Parallelizable(ParallelScope.All)]
//    public class PassThroughControllerTests
//    {
//        [Test]
//        public async Task When_AddLegalEntity_is_requested_Then_EmployerIncentivesPassThroughService_is_called()
//        {
//            var f = new TestsFixture().SetupEmployerIncentivesServiceForAddLegalEntityCall();

//            await f.Sut.AddLegalEntity(f.AccountId, f.LegalEntityRequest);

//            f.EmployerIncentivesPassThroughServiceMock.Verify(x => x.AddLegalEntity(f.AccountId, f.LegalEntityRequest, It.IsAny<CancellationToken>()));
//        }

//        [Test]
//        public async Task When_AddLegalEntity_is_requested_Then_EmployerIncentivesPassThroughService_returns_Content_and_Statuscode()
//        {
//            var f = new TestsFixture().SetupEmployerIncentivesServiceForAddLegalEntityCall();

//            var result = await f.Sut.AddLegalEntity(f.AccountId, f.LegalEntityRequest);

//            f.VerifyInnerApiStatusCodeAndContentIsReturnFromOuterApi(result);
//        }

//        [Test]
//        public async Task When_RemoveLegalEntity_is_requested_Then_EmployerIncentivesPassThroughService_is_called()
//        {
//            var f = new TestsFixture().SetupEmployerIncentivesServiceForRemoveLegalEntityCall();

//            await f.Sut.RemoveLegalEntity(f.AccountId, f.AccountLegalEntityId);

//            f.EmployerIncentivesPassThroughServiceMock.Verify(x => x.RemoveLegalEntity(f.AccountId, f.AccountLegalEntityId, It.IsAny<CancellationToken>()));
//        }

//        [Test]
//        public async Task When_RemoveLegalEntity_is_requested_Then_EmployerIncentivesPassThroughService_returns_Content_and_Statuscode()
//        {
//            var f = new TestsFixture().SetupEmployerIncentivesServiceForRemoveLegalEntityCall();

//            var result = await f.Sut.RemoveLegalEntity(f.AccountId, f.AccountLegalEntityId);

//            f.VerifyInnerApiStatusCodeAndContentIsReturnFromOuterApi(result);
//        }

//        private class TestsFixture
//        {
//            public Mock<IEmployerIncentivesPassThroughService> EmployerIncentivesPassThroughServiceMock;
//            public PassThroughController Sut;
//            public Fixture Fixture;
//            public long AccountId;
//            public long AccountLegalEntityId;
//            public LegalEntityRequest LegalEntityRequest;
//            public InnerApiResponse InnerApiResponse;

//            public TestsFixture()
//            {
//                Fixture = new Fixture();
//                EmployerIncentivesPassThroughServiceMock = new Mock<IEmployerIncentivesPassThroughService>();

//                Sut = new PassThroughController(EmployerIncentivesPassThroughServiceMock.Object);

//                AccountId = Fixture.Create<long>();
//                AccountLegalEntityId = Fixture.Create<long>();
//                LegalEntityRequest = Fixture.Create<LegalEntityRequest>();
//                InnerApiResponse = Fixture.Build<InnerApiResponse>().Without(x=>x.Json).Create();
//            }

//            public TestsFixture SetupEmployerIncentivesServiceForAddLegalEntityCall()
//            {
//                EmployerIncentivesPassThroughServiceMock
//                    .Setup(x => x.AddLegalEntity(It.IsAny<long>(), It.IsAny<LegalEntityRequest>(), It.IsAny<CancellationToken>()))
//                    .ReturnsAsync(InnerApiResponse);

//                return this;
//            }

//            public TestsFixture SetupEmployerIncentivesServiceForRemoveLegalEntityCall()
//            {
//                EmployerIncentivesPassThroughServiceMock
//                    .Setup(x => x.RemoveLegalEntity(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
//                    .ReturnsAsync(InnerApiResponse);

//                return this;
//            }

//            public void VerifyInnerApiStatusCodeAndContentIsReturnFromOuterApi(IActionResult result)
//            {
//                result.Should().NotBeNull();
//                result.Should().BeOfType<ObjectResult>();
//                var objectResult = (ObjectResult)result;
//                objectResult.StatusCode.Should().Be((int)InnerApiResponse.StatusCode);
//                //objectResult.Value.ToString().Should().Be(InnerApiResponse.Content);
//            }
//        }
//    }
//}
