using System;
using System.Linq.Expressions;
using NUnit.Framework;
using FluentValidation.TestHelper;
using SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration;

namespace SFA.DAS.ApprenticeCommitments.UnitTests.Application.Commands.VerifyIdentityRegistrationTests
{

    [TestFixture]

    public class WhenValidatingVerifyIdentityRegistrationCommand
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("bob@domain.com", true)]
        [TestCase("bob@domain", true)]
        public void When_validating_Email(string email, bool expectValid)
        {
            AssertValidationResult(request => request.Email, email, expectValid);
        }

        [Test]
        public void When_empty_RegistrationId_it_fails()
        {
            AssertValidationResult(request => request.ApprenticeId, Guid.Empty, false);
        }

        [Test]
        public void When_empty_UserIdentityId_it_fails()
        {
            AssertValidationResult(request => request.UserIdentityId, Guid.Empty, false);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("  ", false)]
        [TestCase("Bob", true)]
        public void When_validating_FirstName(string firstName, bool expectValid)
        {
            AssertValidationResult(request => request.FirstName, firstName, expectValid);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("  ", false)]
        [TestCase("Bob", true)]
        public void When_validating_LastName(string lastName, bool expectValid)
        {
            AssertValidationResult(request => request.LastName, lastName, expectValid);
        }

        [Test]
        public void When_DateOfBirth_not_set_it_fails()
        {
            AssertValidationResult(request => request.DateOfBirth, default, false);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("  ", false)]
        [TestCase("NE 01 01 01 C", true)]
        [TestCase("NE010101C", true)]
        [TestCase("NE   010101C", true)]
        [TestCase("NE 01  0101C", true)]
        [TestCase("NE0101  01 C", true)]
        [TestCase("BG 01 01 01 C", false)]
        [TestCase("GB 01 01 01 C", false)]
        [TestCase("NK 01 01 01 C", false)]
        [TestCase("KN 01 01 01 C", false)]
        [TestCase("TN 01 01 01 C", false)]
        [TestCase("NT 01 01 01 C", false)]
        [TestCase("ZZ 01 01 01 C", false)]
        [TestCase("AA 01 01 01 E", false)]
        [TestCase("AA 01 01 01 F", false)]
        [TestCase("AA 01 01 01 G", false)]
        public void When_validating_NationalInsuranceNumber(string nationalInsuranceNumber, bool expectValid)
        {
            AssertValidationResult(request => request.NationalInsuranceNumber, nationalInsuranceNumber, expectValid);
        }

        private void AssertValidationResult<T>(Expression<Func<VerifyIdentityRegistrationCommand, T>> property, T value, bool expectedValid)
        {
            // Arrange
            var validator = new VerifyIdentityRegistrationCommandValidator();

            // Act
            if (expectedValid)
            {
                validator.ShouldNotHaveValidationErrorFor(property, value);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(property, value);
            }
        }
    }
}