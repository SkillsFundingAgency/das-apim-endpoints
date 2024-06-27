using SFA.DAS.ProviderFeedback.Application.InnerApi.Responses;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackForAcademicYear
{
    public class GetProviderFeedbackForAcademicYearResult
    {
        public GetProviderStandardForAcademicYearItem ProviderStandard { get; set; }
    }

    public static class GetProviderMockData
    {
        public static GetProviderFeedbackForAcademicYearResult GetProviderFeedbackForAcademicYearResultMock()
        {
            return new GetProviderFeedbackForAcademicYearResult
            {
                ProviderStandard = new GetProviderStandardForAcademicYearItem
                {
                    Ukprn = 10000528,
                    EmployerFeedback = GetEmployerFeedbackForAcademicYearResponseMock(),
                    ApprenticeFeedback = GetApprenticeFeedbackForAcademicYearResponseMock()
                }
            };
        }

        private static GetEmployerFeedbackForAcademicYearResponse GetEmployerFeedbackForAcademicYearResponseMock()
        {
            return new GetEmployerFeedbackForAcademicYearResponse
            {
                Ukprn = 10000528,
                Stars = 3,
                ReviewCount = 11,
                ProviderAttribute = new List<GetEmployerFeedbackForAcademicYearAttributeItem>()
                {
                    new GetEmployerFeedbackForAcademicYearAttributeItem
                    {
                        Weakness = 0,
                        Strength = 1,
                        Name = "Testing Attribute Name"
                    },
                    new GetEmployerFeedbackForAcademicYearAttributeItem
                    {
                        Weakness = 1,
                        Strength = 0,
                        Name = "Working with small numbers of apprentices"
                    },
                    new GetEmployerFeedbackForAcademicYearAttributeItem
                    {
                        Weakness = 1,
                        Strength = 0,
                        Name = "Initial assessment of apprentices"
                    },
                    new GetEmployerFeedbackForAcademicYearAttributeItem
                    {
                        Weakness = 0,
                        Strength = 1,
                        Name = "Adapting to my needs"
                    },
                    new GetEmployerFeedbackForAcademicYearAttributeItem
                    {
                        Weakness = 0,
                        Strength = 1,
                        Name = "Training facilities"
                    }
                },
                TimePeriod = "AY2324"
            };
        }

        private static GetApprenticeFeedbackForAcademicYearResponse GetApprenticeFeedbackForAcademicYearResponseMock()
        {
            return new GetApprenticeFeedbackForAcademicYearResponse
            {
                Ukprn = 10000528,
                Stars = 3,
                ReviewCount = 11,
                ProviderAttribute = new List<GetApprenticeFeedbackForAcademicYearAttributeItem>()
                    {
                        new GetApprenticeFeedbackForAcademicYearAttributeItem
                        {
                            Disagree = 0,
                            Agree = 1,
                            Name = "Testing Attribute Name"
                        },
                        new GetApprenticeFeedbackForAcademicYearAttributeItem
                        {
                               Disagree = 0,
                            Agree = 1,
                            Name = "Working with small numbers of apprentices"
                        },
                        new GetApprenticeFeedbackForAcademicYearAttributeItem
                        {
                            Disagree = 0,
                            Agree = 1,
                            Name = "Initial assessment of apprentices"
                        },
                        new GetApprenticeFeedbackForAcademicYearAttributeItem
                        {
                            Disagree = 0,
                            Agree = 1,
                            Name = "Adapting to my needs"
                        },
                        new GetApprenticeFeedbackForAcademicYearAttributeItem
                        {
                            Disagree = 0,
                            Agree = 1,
                            Name = "Training facilities"
                        }
                    },
                TimePeriod = "AY2324"
            };
        }
    }
}