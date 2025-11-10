using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Common.Constants;
using SFA.DAS.EmployerFeedback.Application.Queries.GetTrainingProviderSearch;
using SFA.DAS.EmployerFeedback.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries
{
    [TestFixture]
    public class GetTrainingProviderSearchQueryHandlerTests
    {
        private Mock<ICacheStorageService> _cache;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitments;
        private Mock<IRoatpV2TrainingProviderService> _roatp;
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _feedback;
        private IOptions<EmployerFeedbackConfiguration> _options;

        private GetTrainingProviderSearchQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _cache = new Mock<ICacheStorageService>();
            _commitments = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _roatp = new Mock<IRoatpV2TrainingProviderService>();
            _feedback = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();

            _options = Options.Create(new EmployerFeedbackConfiguration
            {
                AccountProvidersCourseStatusCompletionLag = 6,
                AccountProvidersCourseStatusStartLag = 12,
                AccountProvidersCourseStatusNewStartWindow = 3
            });

            _sut = new GetTrainingProviderSearchQueryHandler(
                _cache.Object, _commitments.Object, _roatp.Object, _feedback.Object, _options);
        }

        [Test]
        public async Task Handle_MergesCommitmentsWithRoatpNames_OrdersByName_CaseInsensitive()
        {
            // Arrange
            var accountId = 111L;
            var userRef = Guid.NewGuid();

            var roatpProviders = new List<Provider>
            {
                new Provider { Ukprn = 1002, Name = "alpha", ProviderTypeId = ProviderConstants.MainProviderTypeId, StatusId = ProviderConstants.ActiveStatusId },
                new Provider { Ukprn = 1001, Name = "Bravo", ProviderTypeId = ProviderConstants.MainProviderTypeId, StatusId = ProviderConstants.ActiveStatusId },
                new Provider { Ukprn = 2000, Name = "OtherType", ProviderTypeId = 2, StatusId = ProviderConstants.ActiveStatusId },
            };
            _cache.Setup(c => c.RetrieveFromCache<List<Provider>>(GetTrainingProviderSearchQueryHandler.RoatpProvidersCacheKey))
                  .ReturnsAsync(roatpProviders);

            var commitmentsStatus = new GetAccountProvidersCourseStatusResponse
            {
                NewStart = new List<AccountProviderCourse> { new() { Ukprn = 1001 } },
                Active = new List<AccountProviderCourse> { new() { Ukprn = 1002 } },
                Completed = new List<AccountProviderCourse>()
            };
            _commitments
                .Setup(c => c.Get<GetAccountProvidersCourseStatusResponse>(
                    It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .ReturnsAsync(commitmentsStatus);

            var older = new FeedbackResultItem
            {
                DateTimeCompleted = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Local),
                ProviderRating = "Good",
                FeedbackSource = 1
            };
            var newer = new FeedbackResultItem
            {
                DateTimeCompleted = new DateTime(2024, 02, 01, 0, 0, 0, DateTimeKind.Local),
                ProviderRating = "Excellent",
                FeedbackSource = 2
            };
            var latestFeedback = new GetLatestEmployerFeedbackResponse
            {
                AccountId = accountId,
                AccountName = "Acme Ltd",
                EmployerFeedbacks = new List<EmployerFeedbackItem>
                {
                    new EmployerFeedbackItem { Ukprn = 1001, Result = older },
                    new EmployerFeedbackItem { Ukprn = 1001, Result = newer }
                }
            };
            _feedback
                .Setup(f => f.Get<GetLatestEmployerFeedbackResponse>(
                    It.Is<GetLatestEmployerFeedbackRequest>(r => r.AccountId == accountId && r.UserRef == userRef)))
                .ReturnsAsync(latestFeedback);

            // Act
            var result = await _sut.Handle(new GetTrainingProviderSearchQuery(accountId, userRef), CancellationToken.None);

            // Assert
            Assert.That(result.AccountId, Is.EqualTo(accountId));
            Assert.That(result.AccountName, Is.EqualTo("Acme Ltd"));

            Assert.That(result.Providers, Has.Count.EqualTo(2));
            Assert.That(result.Providers.Select(p => p.Ukprn), Is.EquivalentTo(new[] { 1001L, 1002L }));

            // case-insensitive alphabetical: alpha, Bravo
            Assert.That(result.Providers.Select(p => p.ProviderName), Is.EqualTo(new[] { "alpha", "Bravo" }));

            var p1001 = result.Providers.Single(p => p.Ukprn == 1001);
            Assert.Multiple(() =>
            {
                Assert.That(p1001.HasNewStart, Is.True);
                Assert.That(p1001.HasActive, Is.False);
                Assert.That(p1001.HasCompleted, Is.False);
                Assert.That(p1001.Feedback, Is.Not.Null);
                Assert.That(p1001.Feedback!.ProviderRating, Is.EqualTo("Excellent"));
                Assert.That(p1001.Feedback.DateTimeCompleted, Is.EqualTo(new DateTime(2024, 02, 01, 0, 0, 0, DateTimeKind.Local)));
            });

            var p1002 = result.Providers.Single(p => p.Ukprn == 1002);
            Assert.Multiple(() =>
            {
                Assert.That(p1002.HasNewStart, Is.False);
                Assert.That(p1002.HasActive, Is.True);
                Assert.That(p1002.HasCompleted, Is.False);
                Assert.That(p1002.Feedback, Is.Null);
            });
        }

        [Test]
        public async Task Handle_WhenRoatpNameMissing_IncludesProvider_WithEmptyName()
        {
            // Arrange
            var accountId = 222L; var userRef = Guid.NewGuid();

            _cache.Setup(c => c.RetrieveFromCache<List<Provider>>(GetTrainingProviderSearchQueryHandler.RoatpProvidersCacheKey))
                  .ReturnsAsync(new List<Provider>
                  {
                      new Provider { Ukprn = 1001, Name = string.Empty, ProviderTypeId = ProviderConstants.MainProviderTypeId, StatusId = ProviderConstants.ActiveStatusId }
                  });

            _commitments
                .Setup(c => c.Get<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .ReturnsAsync(new GetAccountProvidersCourseStatusResponse
                {
                    Active = new List<AccountProviderCourse> { new() { Ukprn = 1001 } }
                });

            _feedback
                .Setup(f => f.Get<GetLatestEmployerFeedbackResponse>(It.IsAny<GetLatestEmployerFeedbackRequest>()))
                .ReturnsAsync(new GetLatestEmployerFeedbackResponse { EmployerFeedbacks = new List<EmployerFeedbackItem>() });

            // Act
            var result = await _sut.Handle(new GetTrainingProviderSearchQuery(accountId, userRef), CancellationToken.None);

            Assert.That(result.Providers, Has.One.Matches<TrainingProviderSearchResult>(p => p.Ukprn == 1001 && p.ProviderName == string.Empty));
        }

        [Test]
        public async Task Handle_UsesConfigLags_WhenCallingCommitments()
        {
            // Arrange
            _cache.Setup(c => c.RetrieveFromCache<List<Provider>>(GetTrainingProviderSearchQueryHandler.RoatpProvidersCacheKey))
                  .ReturnsAsync(new List<Provider>());

            GetAccountProvidersCourseStatusRequest? captured = null;

            _commitments
                .Setup(c => c.Get<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .Callback<IGetApiRequest>(r => captured = (GetAccountProvidersCourseStatusRequest)r)
                .ReturnsAsync(new GetAccountProvidersCourseStatusResponse());

            _feedback
                .Setup(f => f.Get<GetLatestEmployerFeedbackResponse>(It.IsAny<GetLatestEmployerFeedbackRequest>()))
                .ReturnsAsync(new GetLatestEmployerFeedbackResponse());

            // Act
            await _sut.Handle(new GetTrainingProviderSearchQuery(1, Guid.NewGuid()), CancellationToken.None);

            // Assert
            Assert.That(captured, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(captured!.AccountId, Is.EqualTo(1));
                Assert.That(captured.CompletionLag, Is.EqualTo(_options.Value.AccountProvidersCourseStatusCompletionLag));
                Assert.That(captured.StartLag, Is.EqualTo(_options.Value.AccountProvidersCourseStatusStartLag));
                Assert.That(captured.NewStartWindow, Is.EqualTo(_options.Value.AccountProvidersCourseStatusNewStartWindow));
            });
        }

        [Test]
        public async Task GetActiveMainProvidersAsync_ReturnsCached_WhenPresent_AndSkipsRoatpCall()
        {
            // Arrange
            var cached = new List<Provider> { new Provider { Ukprn = 9999, Name = "Cached", ProviderTypeId = ProviderConstants.MainProviderTypeId, StatusId = ProviderConstants.ActiveStatusId } };
            _cache.Setup(c => c.RetrieveFromCache<List<Provider>>(GetTrainingProviderSearchQueryHandler.RoatpProvidersCacheKey))
                  .ReturnsAsync(cached);

            _commitments
                .Setup(c => c.Get<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .ReturnsAsync(new GetAccountProvidersCourseStatusResponse());

            _feedback
                .Setup(f => f.Get<GetLatestEmployerFeedbackResponse>(It.IsAny<GetLatestEmployerFeedbackRequest>()))
                .ReturnsAsync(new GetLatestEmployerFeedbackResponse());

            // Act
            await _sut.Handle(new GetTrainingProviderSearchQuery(1, Guid.NewGuid()), CancellationToken.None);

            // Assert
            _roatp.Verify(r => r.GetProviders(It.IsAny<bool>()), Times.Never);
            _cache.Verify(c => c.SaveToCache(
                GetTrainingProviderSearchQueryHandler.RoatpProvidersCacheKey,
                It.IsAny<List<Provider>>(),
                It.IsAny<int>(), null),
                Times.Never);
        }

        [Test]
        public async Task GetActiveMainProvidersAsync_WhenServiceThrows_ReturnsEmptyList_AndStillSucceeds()
        {
            // Arrange
            _cache.Setup(c => c.RetrieveFromCache<List<Provider>>(GetTrainingProviderSearchQueryHandler.RoatpProvidersCacheKey))
                  .ReturnsAsync((List<Provider>)null!);

            _roatp.Setup(r => r.GetProviders(true)).ThrowsAsync(new Exception("bang"));

            _commitments
                .Setup(c => c.Get<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .ReturnsAsync(new GetAccountProvidersCourseStatusResponse());

            _feedback
                .Setup(f => f.Get<GetLatestEmployerFeedbackResponse>(It.IsAny<GetLatestEmployerFeedbackRequest>()))
                .ReturnsAsync(new GetLatestEmployerFeedbackResponse());

            // Act
            var result = await _sut.Handle(new GetTrainingProviderSearchQuery(1, Guid.NewGuid()), CancellationToken.None);

            // Assert
            Assert.That(result.Providers, Is.Empty);
            _cache.Verify(c => c.SaveToCache(It.IsAny<string>(), It.IsAny<List<Provider>>(), It.IsAny<int>(), null), Times.Never);
        }

        [Test]
        public async Task Handle_WhenAllFeedbackResultsNull_SetsFeedbackNull()
        {
            // Arrange
            _cache.Setup(c => c.RetrieveFromCache<List<Provider>>(GetTrainingProviderSearchQueryHandler.RoatpProvidersCacheKey))
                  .ReturnsAsync(new List<Provider>
                  {
                      new Provider { Ukprn = 3001, Name = "NoFeedback", ProviderTypeId = ProviderConstants.MainProviderTypeId, StatusId = ProviderConstants.ActiveStatusId }
                  });

            _commitments
                .Setup(c => c.Get<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .ReturnsAsync(new GetAccountProvidersCourseStatusResponse
                {
                    Active = new List<AccountProviderCourse> { new() { Ukprn = 3001 } }
                });

            _feedback
                .Setup(f => f.Get<GetLatestEmployerFeedbackResponse>(It.IsAny<GetLatestEmployerFeedbackRequest>()))
                .ReturnsAsync(new GetLatestEmployerFeedbackResponse
                {
                    EmployerFeedbacks = new List<EmployerFeedbackItem>
                    {
                        new EmployerFeedbackItem { Ukprn = 3001, Result = null }
                    }
                });

            // Act
            var result = await _sut.Handle(new GetTrainingProviderSearchQuery(9, Guid.NewGuid()), CancellationToken.None);

            // Assert
            var p = result.Providers.Single();
            Assert.That(p.Feedback, Is.Null);
        }

        [Test]
        public async Task Handle_IncludesProvidersWithActiveStatus_WhenProviderHasActiveStatusId()
        {
            var accountId = 333L;
            var userRef = Guid.NewGuid();

            _cache.Setup(c => c.RetrieveFromCache<List<Provider>>(GetTrainingProviderSearchQueryHandler.RoatpProvidersCacheKey))
                  .ReturnsAsync(new List<Provider>
                  {
                      new Provider { Ukprn = 1001, Name = "ActiveProvider", ProviderTypeId = ProviderConstants.MainProviderTypeId, StatusId = ProviderConstants.ActiveStatusId }
                  });

            _commitments
                .Setup(c => c.Get<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .ReturnsAsync(new GetAccountProvidersCourseStatusResponse
                {
                    Active = new List<AccountProviderCourse> { new() { Ukprn = 1001 } }
                });

            _feedback
              .Setup(f => f.Get<GetLatestEmployerFeedbackResponse>(It.IsAny<GetLatestEmployerFeedbackRequest>()))
              .ReturnsAsync(new GetLatestEmployerFeedbackResponse { EmployerFeedbacks = new List<EmployerFeedbackItem>() });

            var result = await _sut.Handle(new GetTrainingProviderSearchQuery(accountId, userRef), CancellationToken.None);

            Assert.That(result.Providers, Has.Count.EqualTo(1));
            Assert.That(result.Providers, Has.One.Matches<TrainingProviderSearchResult>(p => p.Ukprn == 1001 && p.ProviderName == "ActiveProvider"));
        }

        [Test]
        public async Task Handle_IncludesProvidersWithActiveButNotTakingOnApprenticesStatus_WhenProviderHasActiveButNotTakingStatusId()
        {
            var accountId = 333L;
            var userRef = Guid.NewGuid();

            _cache.Setup(c => c.RetrieveFromCache<List<Provider>>(GetTrainingProviderSearchQueryHandler.RoatpProvidersCacheKey))
                  .ReturnsAsync(new List<Provider>
                  {
                      new Provider { Ukprn = 1002, Name = "ActiveButNotTakingProvider", ProviderTypeId = ProviderConstants.MainProviderTypeId, StatusId = ProviderConstants.ActiveButNotTakingOnApprenticesStatusId }
                  });

            _commitments
                .Setup(c => c.Get<GetAccountProvidersCourseStatusResponse>(It.IsAny<GetAccountProvidersCourseStatusRequest>()))
                .ReturnsAsync(new GetAccountProvidersCourseStatusResponse
                {
                    Active = new List<AccountProviderCourse> { new() { Ukprn = 1002 } }
                });

            _feedback
              .Setup(f => f.Get<GetLatestEmployerFeedbackResponse>(It.IsAny<GetLatestEmployerFeedbackRequest>()))
              .ReturnsAsync(new GetLatestEmployerFeedbackResponse { EmployerFeedbacks = new List<EmployerFeedbackItem>() });

            var result = await _sut.Handle(new GetTrainingProviderSearchQuery(accountId, userRef), CancellationToken.None);

            Assert.That(result.Providers, Has.Count.EqualTo(1));
            Assert.That(result.Providers, Has.One.Matches<TrainingProviderSearchResult>(p => p.Ukprn == 1002 && p.ProviderName == "ActiveButNotTakingProvider"));
        }
    }
}
