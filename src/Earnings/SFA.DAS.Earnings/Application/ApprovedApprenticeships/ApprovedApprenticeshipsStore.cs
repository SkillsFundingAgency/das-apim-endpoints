using System.Reflection;

namespace SFA.DAS.Earnings.Application.ApprovedApprenticeships;

public class ApprovedApprenticeshipsStore : IApprovedApprenticeshipsStore
{
    private readonly List<ApprovedApprenticeshipCsvRecord> _data = [];

    public ApprovedApprenticeshipsStore()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourcePath = "SFA.DAS.Earnings.cannedApprovedApprenticeshipsData.csv";

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
                    _data.Add(new ApprovedApprenticeshipCsvRecord(++count, long.Parse(entries[0]), int.Parse(entries[1]), long.Parse(entries[2])
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

    public ApprovedApprenticeshipsStore(List<ApprovedApprenticeshipCsvRecord> data)
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