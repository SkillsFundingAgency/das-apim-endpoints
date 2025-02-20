using System.Reflection;
using SFA.DAS.Earnings.Api.Controllers;

namespace SFA.DAS.Earnings.Api.Learnerdata;

public class LearnerDataStore : ILearnerDataStore
{
    private readonly List<Tuple<uint, uint, uint, uint>> _data = new();

    public LearnerDataStore(string resourcePath)
    {
        var loader = new ResourceLoader();

        try
        {
            using (var stream = Assembly.GetAssembly(typeof(LearnerDataStore)).GetManifestResourceStream(resourcePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    uint count = 0;
                    
                    while ((line = reader.ReadLine()) != null)
                    {
                        var entries = line.Split(',');
                        _data.Add(new Tuple<uint, uint, uint, uint>(++count,uint.Parse(entries[0]), uint.Parse(entries[1]), uint.Parse(entries[2])));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading resource: {ex.Message}");
        }
    }
    public LearnerDataStore(List<Tuple<uint, uint, uint>> data)
    {
        for (int c = 0; c < data.Count; c++)
        {
            _data.Add(new Tuple<uint, uint, uint, uint>((uint)c, data[c].Item1, data[c].Item2, data[c].Item3));
        }
    }
    
    public List<Apprenticeship> Search(uint ukprn, uint academicYear, uint page, uint pageSize)
    {
        return _data
            .Where(s => s.Item2 == ukprn & s.Item3 == academicYear)
            .OrderBy(l => l.Item1)
            .Skip((int)(pageSize * (page-1)))
            .Take((int)pageSize)
            .Select(s => new Apprenticeship
            {
                Uln = s.Item4
            }).ToList();
    }

    public int Count(uint ukprn, uint academicYear)
    {
        return _data.Count(s => s.Item3 == academicYear & s.Item2 == ukprn);
    }
}