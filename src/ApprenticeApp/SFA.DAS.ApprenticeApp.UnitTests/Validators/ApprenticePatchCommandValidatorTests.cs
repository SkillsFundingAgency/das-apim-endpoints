using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts;
using System;
using System.Text.Json;

namespace SFA.DAS.ApprenticeApp.UnitTests.Validators
{
    [TestFixture]
    public class ApprenticePatchCommandValidatorTests
    {
        private ApprenticePatchCommandValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ApprenticePatchCommandValidator();
        }

        [Test]
        public void Should_have_no_errors_for_a_valid_email_patch()
        {
            var command = CreateCommand("""
            [
              { "op": "replace", "path": "/email", "value": "new.email@example.com" }
            ]
            """);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_fail_when_patch_is_not_an_array()
        {
            var command = CreateCommand("""
            { "op": "replace", "path": "/email", "value": "new.email@example.com" }
            """);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Patch)
                .WithErrorMessage("Patch must be an array of operations.");
        }

        [Test]
        public void Should_fail_when_operation_is_not_replace()
        {
            var command = CreateCommand("""
            [
              { "op": "add", "path": "/email", "value": "new.email@example.com" }
            ]
            """);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Patch)
                .WithErrorMessage("Operation 'add' is not allowed.");
        }

        [Test]
        public void Should_fail_when_path_is_not_allowed()
        {
            var command = CreateCommand("""
            [
              { "op": "replace", "path": "/emailAddress", "value": "new.email@example.com" }
            ]
            """);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Patch)
                .WithErrorMessage("Patch path '/emailAddress' is not allowed.");
        }

        [Test]
        public void Should_fail_when_email_is_invalid()
        {
            var command = CreateCommand("""
            [
              { "op": "replace", "path": "/email", "value": "not-an-email" }
            ]
            """);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Patch)
                .WithErrorMessage("/email must be a valid email address.");
        }

        private static ApprenticePatchCommand CreateCommand(string patchJson)
        {
            return new ApprenticePatchCommand
            {
                ApprenticeId = Guid.NewGuid(),
                Patch = JsonSerializer.Deserialize<JsonElement>(patchJson)
            };
        }
    }
}
