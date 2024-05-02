using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Apprenticeships.InnerApi
{
	public class GetAcademicYearsRequest : IGetApiRequest
	{
		public readonly string _dateTime;
		public string GetUrl => $"academicyears/{_dateTime}";

		public GetAcademicYearsRequest(DateTime dateTime)
		{
			_dateTime = dateTime.ToString("yyyy-MM-dd");
		}
	}
}
