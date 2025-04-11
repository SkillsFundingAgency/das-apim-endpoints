using FluentAssertions;
using SFA.DAS.Earnings.Application.ApprovedApprenticeships;

namespace SFA.DAS.Earnings.UnitTests.Application.ApprovedApprenticeships;

public class ApprovedApprenticeshipsStoreTests
{     
    [Test]
    public void Pagination_happy_path_returns_correct_data()
    {
        var learnerData = new List<ApprovedApprenticeshipCsvRecord>
        {
            new(1, 21222, 2021, 100),
            new(2, 21222, 2021, 200),
            new(3, 21222, 2021, 300),
            new(4, 21222, 2021, 400),
            new(5, 21222, 2021, 500),
            new(6, 21222, 2021, 600),
            new(7, 21222, 2021, 700),
            new(8, 21222, 2021, 800),
            new(9, 21222, 2021, 900),
            new(11, 21222, 2021, 1000),
            new(12, 21222, 2021, 1100)
        };
        
        var store = new ApprovedApprenticeshipsStore(learnerData);

        var results = store.Search(21222, 2021, 1, 3);

        results.Count.Should().Be(3);
        results[0].Should().Be(100);
    }
    
    [Test]
    public void Sending_invalid_data_to_paging()
    {
        var learnerData = new List<ApprovedApprenticeshipCsvRecord>
        {
            new(1, 21222, 2021, 100),
            new(2, 21222, 2021, 200),
            new(3, 21222, 2021, 300),
            new(4, 21222, 2021, 400),
            new(5, 21222, 2021, 500),
            new(6, 21222, 2021, 600),
            new(7, 21222, 2021, 700),
            new(8, 21222, 2021, 800),
            new(9, 21222, 2021, 900),
            new(10,21222, 2021, 1000),
            new(11, 21222, 2021, 1100)
        };
        
        var store = new ApprovedApprenticeshipsStore(learnerData);

        var results = store.Search(21222, 2021, 100, 3);
        results.Should().BeEmpty();
    }
    
    [Test]
    public void Count_returns_correct_data()
    {
        var learnerData = new List<ApprovedApprenticeshipCsvRecord>
        {
            new(1, 21222, 2021, 100),
            new(2, 21222, 2021, 200),
            new(3, 21222, 2021, 300),
            new(4, 21222, 2021, 400),
            new(5, 21222, 2021, 500),
            new(6, 21222, 2021, 600),
            new(7, 21222, 2021, 700),
            new(8, 21222, 2021, 800),
            new(9, 21222, 2021, 900),
            new(10, 21222, 2021, 1000),
            new(11, 21222, 2021, 1100),
            new(12, 21223, 2021, 1001),
            new(13, 21223, 2021, 2001),
            new(14, 21223, 2021, 3001),
            new(15, 21223, 2021, 4001),
            new(16, 21224, 2021, 5001),
            new(17, 21224, 2021, 6001),
            new(18, 21224, 2021, 7001),
            new(19, 21224, 2021, 8001),
            new(20, 21224, 2021, 9001),
            new(21, 21223, 2021, 10001),
            new(22, 21223, 2223, 6001),
            new(23, 21223, 2223, 70011),
            new(24, 21223, 2223, 80011),
            new(25, 21223, 2223, 90011),
            new(26, 21223, 2223, 100011),
            new(27, 21223, 2223, 1000112)
        };

        var store = new ApprovedApprenticeshipsStore(learnerData);

        Assert.That(store.Count(21222, 2021), Is.EqualTo(11));
        Assert.That(store.Count(21223, 2021), Is.EqualTo(5));
        Assert.That(store.Count(21223, 2223), Is.EqualTo(6));
    }
}