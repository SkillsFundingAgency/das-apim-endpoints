using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship
{
    public class GetMyApprenticeshipsQueryResponse
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<MyApprenticeshipResponse>? MyApprenticeships { get; set; }
    }
    public class GetMyApprenticeshipQueryResult
    {
        public static implicit operator GetMyApprenticeshipQueryResult(GetMyApprenticeshipsQueryResponse response)
        {
            return new GetMyApprenticeshipQueryResult
            {

                ApprenticeId = response.ApprenticeId,
                DateOfBirth = response.DateOfBirth,
                Email = response.Email,
                FirstName = response.FirstName,
                LastName = response.LastName
            };
        }

        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

        public MyApprenticeship? MyApprenticeship { get; set; }
    }

    public class GetStandardResponse{
        public string Title { get; set; }
        public int Level { get; set; }
        public string Route { get; set; }
        public StandardVersionDetail VersionDetail { get; set; }
     }

    public class GetFrameworkResponse
    {
        public string Title { get; set; }
        public int Level { get; set; }
        public string FrameworkName { get; set; }
        public int Duration { get; set; }
    }

    public class MyApprenticeshipResponse
    {
        public long? Uln { get; set; }
        public long? ApprenticeshipId { get; set; }
        public string? EmployerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? TrainingProviderId { get; set; }
        public string? TrainingProviderName { get; set; }

        public string? TrainingCode { get; set; }
        public string? StandardUId { get; set; }
    }

    public class MyApprenticeship
    {

        public static implicit operator MyApprenticeship(MyApprenticeshipResponse myApprenticeshipResponse)
        {
            return new MyApprenticeship
            {
                Uln = myApprenticeshipResponse.Uln,
                ApprenticeshipId = myApprenticeshipResponse.ApprenticeshipId,
                EmployerName = myApprenticeshipResponse.EmployerName,
                StartDate = myApprenticeshipResponse.StartDate,
                EndDate = myApprenticeshipResponse.EndDate,
                TrainingProviderId = myApprenticeshipResponse.TrainingProviderId,
                TrainingProviderName = myApprenticeshipResponse.TrainingProviderName
            };
        }

        //MFCMFC
        // public static implicit operator MyApprenticeship(CreateMyApprenticeshipCommand command)
        // {
        //     return new MyApprenticeship
        //     {
        //         Id = command.Id,
        //         ApprenticeId = command.ApprenticeId,
        //         Uln = command.Uln,
        //         ApprenticeshipId = command.ApprenticeshipId,
        //         EmployerName = command.EmployerName,
        //         StartDate = command.StartDate,
        //         EndDate = command.EndDate,
        //         StandardUId = command.StandardUId,
        //         TrainingCode = command.TrainingCode,
        //         TrainingProviderId = command.TrainingProviderId,
        //         TrainingProviderName = command.TrainingProviderName,
        //         CreatedOn = command.CreatedOn
        //     };
        // }

        // public Guid Id { get; set; }
        // public Guid ApprenticeId { get; set; }
        public long? Uln { get; set; }
        public long? ApprenticeshipId { get; set; }
        public string? EmployerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? TrainingProviderId { get; set; }
        public string? TrainingProviderName { get; set; }

        public TrainingCourse? TrainingCourse { get; set; }
        // public string? TrainingCode { get; set; }
        // public string? StandardUId { get; set; }
        // public DateTime CreatedOn { get; set; }
    }
}
