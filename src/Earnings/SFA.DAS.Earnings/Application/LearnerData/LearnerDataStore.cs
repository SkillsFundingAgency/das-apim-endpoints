using System.Reflection;
using SFA.DAS.Earnings.Api.Learnerdata;

namespace SFA.DAS.Earnings.Application.LearnerData;

public class LearnerDataStore : ILearnerDataStore
{
    private readonly List<LearnerDataCsvRecord> _data = [];

    public LearnerDataStore()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourcePath = "SFA.DAS.Earnings.cannedLearnerData.csv";

            using var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                throw new FileNotFoundException("The embedded resource was not found.", resourcePath);
            }

            using var reader = new StreamReader(stream);
            string line;
            long count = 0;

            while ((line = reader.ReadLine()) != null)
            {
                var entries = line.Split(',');

                if (entries.Length == 3)
                {
                    _data.Add(new LearnerDataCsvRecord(++count, long.Parse(entries[0]), int.Parse(entries[1]), long.Parse(entries[2])
                    ));
                }
                else
                {
                    Console.WriteLine($"Skipping invalid line: {line}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading resource: {ex.Message}");
        }
    }

    public LearnerDataStore(List<LearnerDataCsvRecord> data)
    {
        _data = data;
    }

    public List<long> Search(long ukprn, int academicYear, int page, int pageSize)
    {
        return _data
            .Where(s => s.Ukprn == ukprn && s.AcademicYear == academicYear)
            .OrderBy(l => l.Id)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .Select(s => s.Uln)
            .ToList();
    }

    public int Count(long ukprn, int academicYear)
    {
        return _data.Count(s => s.Ukprn == ukprn && s.AcademicYear == academicYear);
    }
}