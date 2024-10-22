using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar
{
	public class GetAcademicYearByDateRequest : IGetApiRequest
	{
		public readonly string _dateTime;
		public string GetUrl => $"academicyears?date={_dateTime}";

		public GetAcademicYearByDateRequest(DateTime dateTime)
		{
			_dateTime = dateTime.ToString("yyyy-MM-dd");
		}
	}
}
