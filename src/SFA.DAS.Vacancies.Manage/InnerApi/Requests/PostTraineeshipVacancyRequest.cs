using System;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Requests
{
    public class PostTraineeshipVacancyRequest : IPostApiRequest
    {
        private readonly Guid _id;
        private readonly int _ukprn;
        private readonly string _email;

        public PostTraineeshipVacancyRequest(Guid id, int ukprn, string email, PostTraineeshipVacancyRequestData postVacancyRequestData)
        {
            _id = id;
            _ukprn = ukprn;
            _email = email;
            Data = postVacancyRequestData;
        }

        public string PostUrl => $"api/Vacancies/CreateTraineeship/{_id}?ukprn={_ukprn}&userEmail={_email}";
        public object Data { get; set; }
    }
}