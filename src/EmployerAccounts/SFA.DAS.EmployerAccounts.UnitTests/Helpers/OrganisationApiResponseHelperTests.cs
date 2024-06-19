using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Exceptions;
using SFA.DAS.EmployerAccounts.Helpers;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.UnitTests.Helpers
{
    [TestFixture]
    public class OrganisationApiResponseHelperTests
    {
        [Test]
        public void CheckApiResponseStatus_ShouldThrowOrganisationNotFoundException_WhenHttpStatusCodeIsNotFound()
        {
            // Arrange
            var organisationType = OrganisationType.Company;
            var identifier = "123";

            Action act = () => OrganisationApiResponseHelper.CheckApiResponseStatus(HttpStatusCode.NotFound, organisationType, identifier, null);

            act.Should().Throw<OrganisationNotFoundException>().WithMessage($"Did not find an organisation type {organisationType} with identifier {identifier}");
        }

        [Test]
        public void CheckApiResponseStatus_ShouldThrowInvalidGetOrganisationException_WhenHttpStatusCodeIsBadRequest()
        {
            // Arrange
            var organisationType = OrganisationType.Company;
            var identifier = "123";

            Action act = () => OrganisationApiResponseHelper.CheckApiResponseStatus(HttpStatusCode.BadRequest, organisationType, identifier, null);

            act.Should().Throw<InvalidGetOrganisationException>();
        }
    }
}
