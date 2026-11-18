using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Assessor.Api.Models.Users;

namespace SFA.DAS.Assessor.Api.UnitTests.Models.Users
{
    public class WhenValidatingCreateOrUpdateUserRequest
    {
        private CreateOrUpdateUserRequest _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CreateOrUpdateUserRequest
            {
                DfeSignInUserId = "subject",
                EmailAddress = "user@example.com",
                PhoneNumber = "07123456789"
            };
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("07123456789")]
        public void Then_A_Valid_Request_Is_Accepted(string phoneNumber)
        {
            _sut.PhoneNumber = phoneNumber;

            var errors = new List<ValidationResult>();

            var actual = Validator.TryValidateObject(
                _sut,
                new ValidationContext(_sut),
                errors,
                validateAllProperties: true);

            actual.Should().BeTrue();
            errors.Should().BeEmpty();
        }

        [TestCase(nameof(CreateOrUpdateUserRequest.DfeSignInUserId), null)]
        [TestCase(nameof(CreateOrUpdateUserRequest.DfeSignInUserId), "")]
        [TestCase(nameof(CreateOrUpdateUserRequest.DfeSignInUserId), " ")]
        [TestCase(nameof(CreateOrUpdateUserRequest.EmailAddress), null)]
        [TestCase(nameof(CreateOrUpdateUserRequest.EmailAddress), "")]
        [TestCase(nameof(CreateOrUpdateUserRequest.EmailAddress), " ")]
        public void Then_Required_Fields_Are_Validated(
            string propertyName,
            string value)
        {
            typeof(CreateOrUpdateUserRequest)
                .GetProperty(propertyName)
                .SetValue(_sut, value);

            var errors = new List<ValidationResult>();

            var actual = Validator.TryValidateObject(
                _sut,
                new ValidationContext(_sut),
                errors,
                validateAllProperties: true);

            actual.Should().BeFalse();

            errors.Should().Contain(error =>
                error.MemberNames.Contains(propertyName));
        }

        [TestCase(nameof(CreateOrUpdateUserRequest.DfeSignInUserId), 256)]
        [TestCase(nameof(CreateOrUpdateUserRequest.EmailAddress), 257)]
        [TestCase(nameof(CreateOrUpdateUserRequest.PhoneNumber), 21)]
        public void Then_Fields_Exceeding_Maximum_Length_Are_Rejected(
            string propertyName,
            int length)
        {
            typeof(CreateOrUpdateUserRequest)
                .GetProperty(propertyName)
                .SetValue(_sut, new string('x', length));

            var errors = new List<ValidationResult>();

            var actual = Validator.TryValidateObject(
                _sut,
                new ValidationContext(_sut),
                errors,
                validateAllProperties: true);

            actual.Should().BeFalse();

            errors.Should().Contain(error =>
                error.MemberNames.Contains(propertyName));
        }

        [Test]
        public void Then_Fields_At_Maximum_Length_Are_Accepted()
        {
            _sut.DfeSignInUserId = new string('x', 255);
            _sut.EmailAddress = new string('a', 244) + "@example.com";
            _sut.PhoneNumber = new string('1', 20);

            var errors = new List<ValidationResult>();

            var actual = Validator.TryValidateObject(
                _sut,
                new ValidationContext(_sut),
                errors,
                validateAllProperties: true);

            actual.Should().BeTrue();
            errors.Should().BeEmpty();
        }
    }
}