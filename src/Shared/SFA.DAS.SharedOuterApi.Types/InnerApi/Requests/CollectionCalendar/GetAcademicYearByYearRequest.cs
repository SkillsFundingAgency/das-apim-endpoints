using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.CollectionCalendar
{
	public class GetAcademicYearByYearRequest : IGetApiRequest
	{
		private readonly int _year;
		public string GetUrl => $"academicyears/{_year}";

		public GetAcademicYearByYearRequest(int year)
        {
            _year = year;
        }
	}
}
