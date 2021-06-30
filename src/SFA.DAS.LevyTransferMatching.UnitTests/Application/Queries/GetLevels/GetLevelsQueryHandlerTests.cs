﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLevels;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.Tags;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetLevels
{
    [TestFixture]
    public class GetLevelsQueryHandlerTests
    {
        private GetLevelsQueryHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private IEnumerable<ReferenceDataItem> _levels;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _levels = _autoFixture.Create<List<ReferenceDataItem>>();
            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.GetLevels()).ReturnsAsync(_levels);

            _handler = new GetLevelsQueryHandler(_levyTransferMatchingService.Object);
        }

        [Test]
        public async Task Handle_Returns_Levels()
        {
            var result = await _handler.Handle(new GetLevelsQuery(), CancellationToken.None);
            Assert.AreEqual(_levels, result.ReferenceDataItems);
        }
    }
}
