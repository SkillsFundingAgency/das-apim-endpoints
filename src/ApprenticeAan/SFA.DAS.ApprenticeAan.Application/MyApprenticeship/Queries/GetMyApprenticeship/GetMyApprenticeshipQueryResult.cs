namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship
{
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
}
