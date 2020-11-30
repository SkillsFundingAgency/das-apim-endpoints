using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Exceptions;

namespace SFA.DAS.SharedOuterApi.UnitTests.Exceptions
{
    public class WhenConstructingEntityNotFoundException
    {
        [Test]
        public void And_Using_Default_Ctor_Then_Sets_Message_From_T()
        {
            var exception = new EntityNotFoundException<TestEntity>();

            exception.Message.Should().Be($"Entity of type [{nameof(TestEntity)}] cannot be found");
        }

        [Test, AutoData]
        public void And_Using_Message_Ctor_Then_Sets_Message_From_Param(
            string message)
        {
            var exception = new EntityNotFoundException<TestEntity>(message);

            exception.Message.Should().Be(message);
        }

        [Test, AutoData]
        public void And_Using_InnerException_Ctor_Then_Sets_Message_From_T(
            Exception innerException)
        {
            var exception = new EntityNotFoundException<TestEntity>(innerException);

            exception.Message.Should().Be($"Entity of type [{nameof(TestEntity)}] cannot be found");
            exception.InnerException.Should().Be(innerException);
        }

        [Test, AutoData]
        public void And_Using_Message_And_InnerException_Ctor_Then_Sets_Message_From_Param(
            string message,
            Exception innerException)
        {
            var exception = new EntityNotFoundException<TestEntity>(message, innerException);

            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
        }
    }

    public class TestEntity
    {
    }
}