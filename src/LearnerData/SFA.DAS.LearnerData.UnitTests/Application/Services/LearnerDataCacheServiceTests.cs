using System.Text;
using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;

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
        _sut = new LearnerDataCacheService(_cache.Object, _logger.Object);
    }

    [Test]
    public async Task StoreLearner_StoresJsonWithCorrectKeyAndExpiry()
    {
        var ukprn = _fixture.Create<long>();
        var request = _fixture.Create<UpdateLearnerRequest>();
        var expectedKey = $"LearnerData_{ukprn}_{request.Learner.Uln}";

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

        var result = await _sut.GetLearner(12345, "9999999999", CancellationToken.None);

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

        var result = await _sut.GetLearner(12345, expected.Learner.Uln.ToString(), CancellationToken.None);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetLearner_ReturnsNull_OnDeserializationFailure()
    {
        var bytes = Encoding.UTF8.GetBytes("INVALID JSON");

        _cache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bytes);

        var result = await _sut.GetLearner(12345, "1234567890", CancellationToken.None);

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

        var results = await _sut.GetLearners(ukprn, ulns, CancellationToken.None);

        results.Should().HaveCount(2);
        results.Should().ContainEquivalentOf(learner1);
        results.Should().ContainEquivalentOf(learner3);
    }
}