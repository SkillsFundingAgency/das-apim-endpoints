using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Requests
{
    public class PostValidateTraineeshipVacancyRequest : IPostApiRequest
    {
        private readonly Guid _id;
        private readonly long? _ukprn;
        private readonly string _email;

        public PostValidateTraineeshipVacancyRequest(Guid id, PostTraineeshipVacancyRequestData postVacancyRequestData)
        {
            _id = id;
            _ukprn = postVacancyRequestData.User.Ukprn;
            _email = postVacancyRequestData.User.Email;
            Data = postVacancyRequestData;
        }

        public string PostUrl => $"api/vacancies/{_id}/ValidateTraineeship?ukprn={_ukprn}&userEmail={_email}";
        public object Data { get; set; }
    }
}