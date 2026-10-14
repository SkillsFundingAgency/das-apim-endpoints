using System.Text;
using System.Text.Json;
using AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;
using StackExchange.Redis;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class LearnerDataCacheServiceTests
{
    private Fixture _fixture;
    private Mock<IDistributedCache> _cache;
    private Mock<ILogger<LearnerDataCacheService>> _logger;
    private LearnerDataCacheService _sut;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _cache = new Mock<IDistributedCache>();
        _logger = new Mock<ILogger<LearnerDataCacheService>>();
        _sut = new LearnerDataCacheService(_cache.Object, _logger.Object, TimeSpan.Zero);
    }

    private static RedisConnectionException ConnectionFailure() =>
        new(ConnectionFailureType.SocketFailure, "An existing connection was forcibly closed by the remote host");

    private static RedisTimeoutException Timeout() => new("Timeout performing HMGET", CommandStatus.Unknown);

    [Test]
    public async Task StoreLearner_RetriesOnRedisConnectionException_ThenSucceeds()
    {
        _cache.SetupSequence(c => c.SetAsync(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Throws(ConnectionFailure())
            .Throws(ConnectionFailure())
            .Returns(Task.CompletedTask);

        await _sut.StoreLearner(_fixture.Create<UpdateLearnerRequest>(), 12345, CancellationToken.None);

        _cache.Verify(c => c.SetAsync(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()),
            Times.Exactly(3));
    }

    [Test]
    public async Task StoreLearner_RethrowsAfterRetriesExhausted()
    {
        _cache.Setup(c => c.SetAsync(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Throws(ConnectionFailure());

        var act = () => _sut.StoreLearner(_fixture.Create<UpdateLearnerRequest>(), 12345, CancellationToken.None);

        await act.Should().ThrowAsync<RedisConnectionException>();
        _cache.Verify(c => c.SetAsync(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()),
            Times.Exactly(4));
    }

    [Test]
    public async Task StoreLearner_DoesNotRetryNonTransientExceptions()
    {
        _cache.Setup(c => c.SetAsync(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Throws(new InvalidOperationException());

        var act = () => _sut.StoreLearner(_fixture.Create<UpdateLearnerRequest>(), 12345, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
        _cache.Verify(c => c.SetAsync(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetLearner_RetriesOnRedisTimeoutException_ThenReturnsValue()
    {
        var expected = _fixture.Create<UpdateLearnerRequest>();

        _cache.SetupSequence(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Throws(Timeout())
            .ReturnsAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(expected)));

        var result = await _sut.GetLearner<UpdateLearnerRequest>(12345, expected.Learner.Uln.ToString(), CancellationToken.None);

        result.Should().BeEquivalentTo(expected);
        _cache.Verify(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Test]
    public async Task GetLearner_RethrowsAfterRetriesExhausted()
    {
        _cache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Throws(ConnectionFailure());

        var act = () => _sut.GetLearner<UpdateLearnerRequest>(12345, "1234567890", CancellationToken.None);

        await act.Should().ThrowAsync<RedisConnectionException>();
        _cache.Verify(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
    }

    [Test]
    public async Task StoreLearner_StoresJsonWithCorrectKeyAndExpiry()
    {
        var ukprn = _fixture.Create<long>();
        var request = _fixture.Create<UpdateLearnerRequest>();
        var expectedKey = $"LearnerDataApprenticeship_{ukprn}_{request.Learner.Uln}";

        byte[]? storedBytes = null;
        DistributedCacheEntryOptions? storedOptions = null;

        _cache.Setup(c => c.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, byte[], DistributedCacheEntryOptions, CancellationToken>((key, bytes, opts, _) =>
            {
                storedBytes = bytes;
                storedOptions = opts;
            })
            .Returns(Task.CompletedTask);

        await _sut.StoreLearner(request, ukprn, CancellationToken.None);

        storedBytes.Should().NotBeNull();
        storedOptions.Should().NotBeNull();
        storedOptions!.AbsoluteExpirationRelativeToNow.Should().Be(TimeSpan.FromHours(4));

        _cache.Verify(c => c.SetAsync(
                expectedKey,
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetLearner_ReturnsNull_OnCacheMiss()
    {
        _cache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        var result = await _sut.GetLearner<UpdateLearnerRequest>(12345, "9999999999", CancellationToken.None);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetLearner_ReturnsDeserializedObject_OnCacheHit()
    {
        var expected = _fixture.Create<UpdateLearnerRequest>();
        var json = JsonSerializer.Serialize(expected);
        var bytes = Encoding.UTF8.GetBytes(json);

        _cache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bytes);

        var result = await _sut.GetLearner<UpdateLearnerRequest>(12345, expected.Learner.Uln.ToString(), CancellationToken.None);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetLearner_ReturnsNull_OnDeserializationFailure()
    {
        var bytes = Encoding.UTF8.GetBytes("INVALID JSON");

        _cache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bytes);

        var result = await _sut.GetLearner<UpdateLearnerRequest>(12345, "1234567890", CancellationToken.None);

        result.Should().BeNull();

        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task GetLearners_ReturnsOnlyNonNullResults()
    {
        var ukprn = 12345;
        var ulns = new[] { "1", "2", "3" };

        var learner1 = _fixture.Create<UpdateLearnerRequest>();
        var learner3 = _fixture.Create<UpdateLearnerRequest>();

        _cache.SetupSequence(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(learner1)))
            .ReturnsAsync((byte[]?)null)
            .ReturnsAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(learner3)));

        var results = await _sut.GetLearners<UpdateLearnerRequest>(ukprn, ulns, CancellationToken.None);

        results.Should().HaveCount(2);
        results.Should().ContainEquivalentOf(learner1);
        results.Should().ContainEquivalentOf(learner3);
    }
}