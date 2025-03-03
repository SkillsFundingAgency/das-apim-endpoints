using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.AccountLegalEntities.Queries.GetAccountLegalEntities;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.EmployerPR.UnitTests.Application.EmployerAccountsLegalEntities.Queries.GetLegalEntities;
public class LegalEntitiesTests
{
    [Test, AutoData]
    public void Operator_ConvertsFrom_AccountLegalEntities(GetAccountLegalEntityResponse source)
    {
        LegalEntity sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.AccountLegalEntityPublicHashedId, Is.EqualTo(source.AccountLegalEntityPublicHashedId));
            Assert.That(sut.AccountLegalEntityName, Is.EqualTo(source.Name));
            Assert.That(sut.AccountLegalEntityId, Is.EqualTo(source.AccountLegalEntityId));
        });
    }
}
