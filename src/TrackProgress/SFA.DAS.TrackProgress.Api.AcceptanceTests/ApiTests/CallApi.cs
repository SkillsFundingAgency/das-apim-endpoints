using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgress.Api.AcceptanceTests.TestModels;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Tests;
using System.Text;
using System.Text.Json;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests.ApiTests;

public class CallApi : ApiFixture
{
    [TestCase("")]
    [TestCase(null)]
    [TestCase(" ")]
    public async Task Track_progress_with_single_matching_apprenticeship_and_no_sandbox_mode(string optionToTest)
    {
        var apprenticeship = An.Apprenticeship.WithOption(optionToTest);
        var course = A.Course.WithoutOptions();

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var validDto = BuildValidProgressDtoContentFromCourseResponse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

        response.Should().Be201Created();
    }

    [TestCase("OPTION")]
    public async Task Track_progress_with_single_matching_apprenticeship_and_no_sandbox_mode_options(string optionToTest)
    {
        var apprenticeship = An.Apprenticeship.WithOption(optionToTest);
        var course = A.Course.WithOptions(optionToTest);

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var validDto = BuildValidProgressDtoContentFromCourseResponse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

        response.Should().Be201Created();
    }

    [TestCase(null, true)]
    [TestCase("true", false)]
    [TestCase("false", true)]
    [TestCase("rubbish", true)]
    public async Task Track_progress_with_single_matching_apprenticeship_and_different_sandbox_modes(string? isSandbox, bool updateExpected)
    {
        var course = A.Course;
        var apprenticeship = An.Apprenticeship.WithCourse(course);
        var validDto = BuildValidProgressDtoContentFromCourseResponse(course);

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForSandboxMode, isSandbox);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

        // TODO Ensure an update is called only when it is expected

        response.Should().Be201Created();
    }

    [Test]
    public async Task Track_progress_with_no_matching_apprenticeship()
    {
        factory.WithoutApprenticeship(An.Apprenticeship);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

        response.Should().Be404NotFound();
    }

    [Test]
    public async Task Track_progress_with_multiple_matching_apprenticeships()
    {
        factory.WithApprenticeship(An.Apprenticeship.WithMultipleStages());

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

        response.Should().Be400BadRequest().And.MatchInContent("*Multiple apprenticeship records exist*");
    }

    [Test]
    public async Task Track_progress_called_with_start_date_other_than_01()
    {
        factory.WithApprenticeship(An.Apprenticeship.WithStartDate("2020-01-19"));

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01-19"}/progress", BuildValidProgressDtoContent());

        response.Should().Be400BadRequest().And.MatchInContent("*Invalid start date (must start on the 1st)*");
    }

    [Test]
    public async Task Track_progress_with_non_portable_flexi_delivery_model()
    {
        var course = A.Course;
        var apprenticeship = An.Apprenticeship
            .WithDeliveryModel(DeliveryModel.Regular)
            .WithCourse(course);

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

        response.Should().Be400BadRequest().And.MatchInContent("*Must be a portable flexi-job*");
    }

    [Test]
    public async Task Track_progress_with_not_started_apprenticeship()
    {
        var course = A.Course;
        var apprenticeship = An.Apprenticeship.WithNotStarted().WithCourse(course);

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

        response.Should().Be400BadRequest()
            .And.MatchInContent("*Apprentice status must be Live, Paused, Stopped (provided some delivery took place) or Complete*");
    }

    [Test]
    public async Task Track_progress_with_invalid_Ids()
    {
        var course = A.Course;
        var apprenticeship = An.Apprenticeship.WithCourse(course);

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", BuildProgressDtoContentWithInvalidIds());

        response.Should().Be400BadRequest().And.MatchInContent("*XXXX is not a valid guid*");
    }

    [Test]
    public async Task Track_progress_with_invalid_Values()
    {
        var course = A.Course;
        var apprenticeship = An.Apprenticeship.WithCourse(course);

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", BuildProgressDtoContentWithInvalidValues());

        response.Should().Be400BadRequest()
            .And.MatchInContent("*The progress value (-6) associated with this KSB must be in the range of 0 to 100*");
    }

    [Test]
    public async Task Track_progress_with_no_Ksbs()
    {
        var course = A.Course;
        var apprenticeship = An.Apprenticeship.WithCourse(course);

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", BuildProgressDtoContentWithNoKsbs());

        response.Should().Be400BadRequest().And.MatchInContent("*KSBs are required*");
    }

    [Test]
    public async Task Track_progress_with_null_Ids()
    {
        var course = A.Course;
        var apprenticeship = An.Apprenticeship.WithCourse(course);

        factory.WithApprenticeship(apprenticeship).WithCourse(course);
        var response = await client.PostAsync($"/apprenticeships/{1}/{"2020-01"}/progress", BuildProgressDtoContentWithANullId());
        response.Should().Be400BadRequest().And.MatchInContent("*KSB Ids cannot be null*");
    }

    [Test]
    public async Task Track_progress_with_duplicate_Ids()
    {
        var course = A.Course;
        var apprenticeship = An.Apprenticeship.WithCourse(course);

        factory.WithApprenticeship(apprenticeship).WithCourse(course);
        var response = await client.PostAsync($"/apprenticeships/{1}/{"2020-01"}/progress", BuildProgressDtoContentWithDuplicateIds());
        response.Should().Be400BadRequest().And.MatchInContent("*Ensure that there are no duplicate GUIDs in the progress submission*");
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task Track_progress_with_non_delivery_apprenticesships(int apprenticeshipCount)
    {
        factory.WithApprenticeship(An.Apprenticeship.WithStartAndStopOnSameDay().WithMultipleStages(apprenticeshipCount));
        var response = await client.PostAsync($"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());
        response.Should().Be400BadRequest().And.MatchInContent("*Apprentice status must be Live, Paused, Stopped (provided some delivery took place) or Complete*");
    }

    [Test]
    public async Task Track_progress_with_single_matching_apprenticeship_and_a_subset_of_valid_ksbs()
    {
        var apprenticeship = An.Apprenticeship.WithOption("OPTION1").WithStandard("STD");
        var course = A.Course.WithOptions(apprenticeship.Option).WithStandard(apprenticeship.Standard);
        var validDto = BuildValidSubsetProgressDtoContentFromCourseResponse(course);

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

        response.Should().Be201Created();
    }

    [Test]
    public async Task Track_progress_with_single_matching_apprenticeship_and_an_additional_ksbs_which_doesnt_belong_course()
    {
        var course = A.Course.WithOptions("OPTION1").WithStandard("STD");
        var apprenticeship = An.Apprenticeship.WithCourse(course);
        var validDto = BuildAdditionalKsbToProgressDtoContentFromCourseResponse(course);

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

        response.Should().Be400BadRequest()
            .And.MatchInContent("*This KSB does not match the course option*");
    }

    [Test]
    public async Task Track_progress_with_no_options_but_options_do_exist_for_course()
    {
        var apprenticeship = An.Apprenticeship.WithStandard("STDUID").WithOption("");
        var course = A.Course.WithStandard("STDUID").WithOptions("Option 1", "Option 2");
        var validDto = BuildAdditionalKsbToProgressDtoContentFromCourseResponse(course);

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

        response.Should().Be400BadRequest()
            .And.MatchInContent("*This apprenticeship requires an option to be set to record progress against it*");
    }

    private StringContent BuildValidProgressDtoContentFromCourseResponse(Course course)
    {
        var ksbs = course.Ksbs.Select(x => new ProgressDto.Ksb { Id = x.Id.ToString(), Value = 5 }).ToList();
        var dto = new ProgressDto { Progress = new ProgressDto.ProgressDetails { Ksbs = ksbs } };
        var validDto = BuildProgressDtoContent(dto);

        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildValidSubsetProgressDtoContentFromCourseResponse(Course course)
    {
        var ksbs = course.Ksbs.Select(x => new ProgressDto.Ksb { Id = x.Id.ToString(), Value = 5 }).Take(2).ToList();
        var dto = new ProgressDto { Progress = new ProgressDto.ProgressDetails { Ksbs = ksbs } };
        var validDto = BuildProgressDtoContent(dto);

        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildAdditionalKsbToProgressDtoContentFromCourseResponse(Course course)
    {
        var ksbs = course.Ksbs.Select(x => new ProgressDto.Ksb { Id = x.Id.ToString(), Value = 5 }).Take(2).ToList();
        ksbs.Add(new ProgressDto.Ksb { Id = Guid.NewGuid().ToString(), Value = 9 });
        var dto = new ProgressDto { Progress = new ProgressDto.ProgressDetails { Ksbs = ksbs } };
        var validDto = BuildProgressDtoContent(dto);

        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildValidProgressDtoContent()
    {
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>
                {
                    new() {Id = Guid.NewGuid().ToString(), Value = 5},
                    new() {Id = Guid.NewGuid().ToString(), Value = 6}
                }
            }
        };
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContentWithInvalidIds()
    {
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>
                {
                    new() {Id = "XXXX", Value = 5},
                    new() {Id = "123", Value = 6}
                }
            }
        };
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContentWithInvalidValues()
    {
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>
                {
                    new() {Id = Guid.NewGuid().ToString(), Value = 101},
                    new() {Id = Guid.NewGuid().ToString(), Value = -6}
                }
            }
        };
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContentWithNoKsbs()
    {
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>()
            }
        };
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContentWithANullId()
    {
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>
                {
                    new() {Id = null, Value = 5},
                    new() {Id = Guid.NewGuid().ToString(), Value = 6}
                }
            }
        };
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContentWithDuplicateIds()
    {
        var id = Guid.NewGuid().ToString();
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>
                {
                    new() {Id = Guid.NewGuid().ToString(), Value = 5},
                    new() {Id = id, Value = 33},
                    new() {Id = id, Value = 60}
                }
            }
        };
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContent(ProgressDto dto)
    {
        return new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
    }
}