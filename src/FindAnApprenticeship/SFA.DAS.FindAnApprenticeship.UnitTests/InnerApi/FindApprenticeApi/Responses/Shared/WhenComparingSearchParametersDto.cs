using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.FindApprenticeApi.Responses.Shared;

public class WhenComparingSearchParametersDto
{
    [Test, MoqAutoData]
    public void Then_If_Values_Match_Then_Dtos_Are_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            SelectedRouteIds = searchParametersDto.SelectedRouteIds?.Select(x => x).ToList(),
            SelectedLevelIds = searchParametersDto.SelectedLevelIds?.Select(x => x).ToList()
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeTrue();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Selected_Routes_Do_Not_Match_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            SelectedRouteIds = [4, 5000],
            SelectedLevelIds = searchParametersDto.SelectedLevelIds?.Select(x => x).ToList()
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Selected_Levels_Do_Not_Match_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            SelectedRouteIds = searchParametersDto.SelectedRouteIds?.Select(x => x).ToList(),
            SelectedLevelIds = [999999]
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Selected_Levels_Are_Null_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            SelectedRouteIds = searchParametersDto.SelectedRouteIds?.Select(x => x).ToList(),
            SelectedLevelIds = null
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Selected_RouteIds_Are_Null_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            SelectedRouteIds = null,
            SelectedLevelIds = searchParametersDto.SelectedLevelIds?.Select(x => x).ToList()
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_SearchTerm_Does_Not_Match_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            SearchTerm = "qwerty"
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Distance_Does_Not_Match_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            Distance = null
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Location_Does_Not_Match_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            Location = null
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Latitude_Does_Not_Match_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            Latitude = null
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Longitude_Does_Not_Match_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            Longitude = null
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_DisabilityConfident_Does_Not_Match_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // arrange
        var dto = searchParametersDto with
        {
            DisabilityConfident = !searchParametersDto.DisabilityConfident
        };
        
        // act
        var result = dto.Equals(searchParametersDto); 
        
        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Source_Is_Null_Then_Dtos_Are_Not_Equal(SearchParametersDto searchParametersDto)
    {
        // act
        var result = searchParametersDto.Equals(null); 
        
        // assert
        result.Should().BeFalse();
    }
}