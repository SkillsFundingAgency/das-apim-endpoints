using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Api.Mappers
{
    public static class StudentDataMapper
    {
        public static StudentDataList MapFromCreateStudentDataRequest(this CreateStudentDataPostRequest request, int LogId,string dataSource)
        {
            var studentDataList = new List<StudentData>();

            foreach (StudentRequestModel dto in request.ListOfStudentData)
            {
                var studentData = new StudentData
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    DateOfBirth = dto.DateOfBirth,
                    Email = dto.Email,
                    DataSource = dataSource,
                    SchoolName = "",
                    Postcode = dto.Postcode,
                    Industry = dto.Industry,
                    DateOfInterest = dto.DateOfInterest,
                    LogId = LogId
                };

                studentDataList.Add(studentData);
            }

            return new StudentDataList { ListOfStudentData = studentDataList };
        }
    }
}
