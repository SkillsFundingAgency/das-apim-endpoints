Feature: PutApprentice
  As the portal APPLICATION
  I need to retrieve my apprentice details

  Scenario: The apprentice exists
    Given there is an apprentice put request
    When the apprentice put is requested
    Then the result should contain the apprentice data