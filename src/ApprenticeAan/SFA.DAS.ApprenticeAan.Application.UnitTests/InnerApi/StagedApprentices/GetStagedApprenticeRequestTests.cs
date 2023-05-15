using System.Net.Mail;
using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.InnerApi.StagedApprentices;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.InnerApi.StagedApprentices;

public class GetStagedApprenticeRequestTests
{
    [Test, AutoData]
    public void GetUrl_IsCorrect(string lastName, DateTime dateOfBirth, MailAddress email)
    {
        GetStagedApprenticeRequest sut = new(lastName, dateOfBirth, email.Address);

        sut.GetUrl.Should().Be($"stagedapprentices?lastName={lastName}&dateOfBirth={dateOfBirth.Date:yyyy-MM-dd}&email={email.Address}");
    }
}
