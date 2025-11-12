using SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetUkrlp;
public class GetUkrlpQueryHandlerTests
{
    [Test]
    public void Test()
    {
        GetUkrlpQueryHandler sut = new();
        var ukprn = 1234;
        var query = new GetUkrlpQuery(ukprn);
    }
}
