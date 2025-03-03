using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar
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
