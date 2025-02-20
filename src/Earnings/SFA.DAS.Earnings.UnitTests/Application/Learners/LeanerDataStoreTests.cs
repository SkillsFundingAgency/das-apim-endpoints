using SFA.DAS.Earnings.Api.Controllers;
using SFA.DAS.Earnings.Api.Learnerdata;

namespace SFA.DAS.Earnings.UnitTests.Application.Learners;

public class LeanerDataStoreTests
{     
    [Test]
    public void Pagination_happy_path_returns_correct_data()
    {
        var dict = new List<Tuple<uint, uint, uint>>
        {
            new(21222, 2021, 100),
            new(21222, 2021, 200),
            new(21222, 2021, 300),
            new(21222, 2021, 400),
            new(21222, 2021, 500),
            new(21222, 2021, 600),
            new(21222, 2021, 700),
            new(21222, 2021, 800),
            new(21222, 2021, 900),
            new(21222, 2021, 1000),
            new(21222, 2021, 1100)
        };
        
        var store = new LearnerDataStore(dict);

        var results = store.Search(21222, 2021, 1, 3);
        
        Assert.That(results.Count, Is.EqualTo(3));
        Assert.That(results[0].Uln, Is.EqualTo(100));
    }
    
    [Test]
    public void Sending_invalid_data_to_paging()
    {
        var dict = new List<Tuple<uint, uint, uint>>
        {
            new(21222, 2021, 100),
            new(21222, 2021, 200),
            new(21222, 2021, 300),
            new(21222, 2021, 400),
            new(21222, 2021, 500),
            new(21222, 2021, 600),
            new(21222, 2021, 700),
            new(21222, 2021, 800),
            new(21222, 2021, 900),
            new(21222, 2021, 1000),
            new(21222, 2021, 1100)
        };
        
        var store = new LearnerDataStore(dict);

        var results = store.Search(21222, 2021, 100, 3);
        Assert.That(results.Count, Is.EqualTo(0));

        results = store.Search(21222, 2021, 1, 100);
        Assert.That(results.Count, Is.EqualTo(11));
    }
    
    [Test]
    public void Count_returns_correct_data()
    {
        var dict = new List<Tuple<uint, uint, uint>>
        {
            new( 21222, 2021, 100),
            new( 21222, 2021, 200),
            new( 21222, 2021, 300),
            new( 21222, 2021, 400),
            new( 21222, 2021, 500),
            new( 21222, 2021, 600),
            new( 21222, 2021, 700),
            new( 21222, 2021, 800),
            new( 21222, 2021, 900),
            new( 21222, 2021, 1000),
            new( 21222, 2021, 1100),
            new( 21223, 2021, 1001),
            new( 21223, 2021, 2001),
            new( 21223, 2021, 3001),
            new( 21223, 2021, 4001),
            new( 21224, 2021, 5001),
            new( 21224, 2021, 6001),
            new( 21224, 2021, 7001),
            new( 21224, 2021, 8001),
            new( 21224, 2021, 9001),
            new( 21223, 2021, 10001),
            new( 21223, 2223, 6001),
            new( 21223, 2223, 70011),
            new( 21223, 2223, 80011),
            new( 21223, 2223, 90011),
            new( 21223, 2223, 100011),
            new( 21223, 2223, 1000112)
        };

        var store = new LearnerDataStore(dict);

        var results = store.Search(21222, 2021, 1, 3);

        Assert.That(store.Count(21222, 2021), Is.EqualTo(11));
        Assert.That(store.Count(21223, 2021), Is.EqualTo(5));
        Assert.That(store.Count(21223, 2223), Is.EqualTo(6));
    }
}