using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Models
{
    public class WhenMappingToApprenticeshipDtoFromMediatorResult
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ApprenticeshipResponse source)
        {
            var actual = (IncentiveClaimApprenticeshipDto)source;

            actual.ApprenticeshipId.Should().Be(source.Id);
            actual.FirstName.Should().Be(source.FirstName);
            actual.LastName.Should().Be(source.LastName);
            actual.Uln.Should().Be(source.Uln);
            actual.DateOfBirth.Should().Be(source.DateOfBirth);
            ((short)actual.ApprenticeshipEmployerTypeOnApproval).Should().Be((short)source.ApprenticeshipEmployerTypeOnApproval.Value);//TODO check this is correct
            actual.UKPRN.Should().Be(source.ProviderId);
        }
    }
}
