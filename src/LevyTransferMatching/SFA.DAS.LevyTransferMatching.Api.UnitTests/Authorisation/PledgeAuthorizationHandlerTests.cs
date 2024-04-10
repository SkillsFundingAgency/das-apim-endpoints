using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Authentication;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Authorisation
{
    public class PledgeAuthorizationHandlerTests
    {
        private PledgeAuthorizationHandler _sut;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private DefaultHttpContext _httpContext;
        private long _decodedAccountId;
        private int _decodedPledgeId;
        private readonly Mock<ILevyTransferMatchingService> _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>(MockBehavior.Strict);
        private readonly Fixture _fixture = new Fixture();
        private List<IAuthorizationRequirement> _requirements;
        private readonly ClaimsPrincipal _user = new ClaimsPrincipal();
        private AuthorizationHandlerContext _context;

        [SetUp]
        public void Setup()
        {
            _requirements = new List<IAuthorizationRequirement>
            {
                new PledgeRequirement()
            };

            _decodedAccountId = _fixture.Create<long>();
            _decodedPledgeId = _fixture.Create<int>();

            _levyTransferMatchingService.Setup(x => x.GetPledge(_decodedPledgeId)).ReturnsAsync(new Pledge()
            {
                AccountId = _decodedAccountId
            });

            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            var responseMock = new FeatureCollection();
            _httpContext = new DefaultHttpContext(responseMock);
            _httpContext.Request.RouteValues.Add("AccountId", _decodedAccountId);
            _httpContext.Request.RouteValues.Add("PledgeId", _decodedPledgeId);
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_httpContext);

            _context = new AuthorizationHandlerContext(_requirements, _user, string.Empty);

            _sut = new PledgeAuthorizationHandler(_httpContextAccessor.Object, Mock.Of<ILogger<PledgeAuthorizationHandler>>(), _levyTransferMatchingService.Object);

        }

        [Test]
        public async Task HandleRequirementAsync_Return_True_For_Valid_Account_Pledge_Combination()
        {
            await _sut.HandleAsync(_context);
            Assert.That(_context.HasSucceeded, Is.True);
        }

        [Test]
        public async Task HandleRequirementAsync_Return_False_For_Invalid_Account_Pledge_Combination()
        {
            _httpContext.Request.RouteValues.Remove("AccountId");
            _httpContext.Request.RouteValues.Add("AccountId", 0);

            await _sut.HandleAsync(_context);
            Assert.That(_context.HasSucceeded, Is.False);
        }

        [Test]
        public async Task HandleRequirementAsync_Return_False_When_No_AccountId_RouteValue_Present()
        {
            _httpContext.Request.RouteValues.Remove("AccountId");

            await _sut.HandleAsync(_context);
            Assert.That(_context.HasSucceeded, Is.False);
        }

        [Test]
        public async Task HandleRequirementAsync_Return_False_When_No_PledgeId_RouteValue_Present()
        {
            _httpContext.Request.RouteValues.Remove("PledgeId");

            await _sut.HandleAsync(_context);
            Assert.That(_context.HasSucceeded, Is.False);
        }
    }
}
